﻿namespace CJStudio.Dash.Player {
    using System.Collections.Generic;

    using Eccentric.Utils;

    using UnityEngine.InputSystem;
    using UnityEngine;
    [System.Serializable]
    class Dash : PlayerComponent {
        public event System.Action<DashProps> Aim = null;
        public event System.Action AimEnded = null;
        public event System.Action<DashProps> EnergyChange = null;
        public bool IsDashing => bUsingDash;
        DashProps props = null;
        Timer timer = null;
        DashStats stats = null;
        bool bUsingDash = false;
        bool bCanDash = false;
        bool bAim = false;
        bool bUsingEnergy = false;
        float energy = 0f;
        float charge = 0f;
        float velocity = 0f;
        Vector2 direction = Vector2.zero;
        float normalTimeScale = 0f;
        public Dash (Player player, DashStats stats) : base (player) {
            this.stats = stats;
            energy = stats.BasicEnergy;
            timer = new Timer ( );
            normalTimeScale = Time.timeScale;
            props = new DashProps ( );
            EnergyChanging ( );
            props.MaxCharge = stats.MaxCharge;
            props.MaxEnergy = stats.BasicEnergy;
        }

        override public void Tick ( ) {
            CheckCollision ( );
            if (bAim && !bUsingDash) {
#if UNITY_EDITOR
                Debug.DrawRay (Player.Tf.position, direction * stats.RayForBreakableItem, Color.blue);
#endif
                if (bUsingEnergy) {
                    energy -= Time.unscaledDeltaTime;
                    EnergyChanging ( );
                    if (energy <= 0f) {
                        ResetState ( );
                        return;
                    }
                }
                if (timer.IsFinished) {
                    bUsingEnergy = true;
                    timer.Reset (stats.BasicChargeTime);
                    charge = stats.MinCharge;
                }
                charge += Time.unscaledDeltaTime;
                Aiming ( );
            }

        }

        override public void FixedTick ( ) {
            //Start perform Dash
            if (bUsingDash) {
                UsingDash ( );
            }
        }

        public void AddEnergy (float supplement) {
            float newEnergy = this.energy + supplement;
            energy = Mathf.Clamp (newEnergy, 0f, stats.BasicEnergy);
            EnergyChanging ( );

        }

        void EnergyChanging ( ) {
            props.EnergyRemain = energy;
            if (EnergyChange != null)
                EnergyChange (props);
        }

        void Aiming ( ) {
            props.Charge = charge;
            props.Direction = direction;
            props.Pos = Player.Tf.position;
            if (Aim != null)
                Aim (props);
        }

        void AimAnimEnded ( ) {
            if (AimEnded != null)
                AimEnded ( );
        }

        //Call this method when player press use button to reset related value
        void UseDash ( ) {
            if (bAim && !bUsingDash) {
                Time.timeScale = normalTimeScale;
                float distance = stats.ChargeMultiplier * charge / stats.MaxCharge;
                velocity = distance / stats.AnimTime;
                bUsingDash = true;
                timer.Reset (stats.AnimTime);
                AimAnimEnded ( );
                Player.Anim.SetBool ("dash", true);
            }
        }

        void UsingDash ( ) {
            if (timer.IsFinished) {
                ResetState ( );
                bUsingDash = false;
                bCanDash = false;
                Player.Anim.SetBool ("dash", false);
            }
            // if touch breakable object reset the state and can using dash again
            #region CHECK_BREAKABLE_OBJECT
            RaycastHit2D result = Physics2D.Raycast (Player.Tf.position, direction, stats.RayForBreakableItem, stats.BreakableItemLayer);
            if (result.collider != null && result.collider.tag == "Breakable") {
                result.collider.GetComponent<BreakableItem> ( ).Break ( );
                ResetState ( );
                bUsingDash = false;
                bCanDash = true;
                Player.Anim.SetBool ("dash", false);
            }
            #endregion
            if (direction.x != 0f) {
                bool bFaceRight = direction.x > 0f;
                Render.ChangeDirectionXWithSpriteRender (bFaceRight, Player.Rend, true);
            }
            Player.Rb.MovePosition (Player.Rb.position + direction * velocity * Time.fixedDeltaTime);
        }

        //Call this method to reset all vars before a new Dash
        void ResetState ( ) {
            bAim = false;
            Player.Anim.SetBool ("aim", false);
            bUsingEnergy = false;
            charge = stats.MinCharge;
            velocity = 0f;
            direction = Vector2.zero;
            Time.timeScale = normalTimeScale;
            AimAnimEnded ( );
        }

        //Check if player bomb into any collider which can reset its dash state
        void CheckCollision ( ) {
            if (Player.RayCastController.IsCollide)
                bCanDash = true;
        }

        void OnDashBtnStarted (InputAction.CallbackContext ctx) {
            if (!bUsingDash && bCanDash) {
                ResetState ( );
                bAim = true;
                Player.Anim.SetBool ("aim", true);
                timer.Reset (stats.BasicChargeTime);
                direction = ctx.ReadValue<Vector2> ( ).normalized;
                Time.timeScale = stats.AimTimeScale;
            }
        }
        void OnDashBtnPerformed (InputAction.CallbackContext ctx) {
            if (bAim && !bUsingDash) {
                direction = ctx.ReadValue<Vector2> ( ).normalized;
            }
        }
        void OnDashBtnCanceled (InputAction.CallbackContext ctx) {
            if (!bUsingDash) {
                ResetState ( );
            }
        }
        void OnUseBtnPressed (InputAction.CallbackContext ctx) {
            UseDash ( );
        }
        override public void OnEnable ( ) {
            Control.GamePlay.Use.started += OnUseBtnPressed;
            Control.GamePlay.Dash.started += OnDashBtnStarted;
            Control.GamePlay.Dash.performed += OnDashBtnPerformed;
            Control.GamePlay.Dash.canceled += OnDashBtnCanceled;
        }

        override public void OnDisable ( ) {
            Control.GamePlay.Use.started -= OnUseBtnPressed;
            Control.GamePlay.Dash.started -= OnDashBtnStarted;
            Control.GamePlay.Dash.performed -= OnDashBtnPerformed;
            Control.GamePlay.Dash.canceled -= OnDashBtnCanceled;
        }
    }

    [System.Serializable]
    class DashStats : PlayerStats {
        public float BasicChargeTime => MaxCharge - MinCharge;
        public float MinCharge = 1f;
        public float MaxCharge = 5f;
        public float BasicEnergy = 10f;
        public float ChargeMultiplier = 0f;
        public float AnimTime = .3f;
        public float AimTimeScale = .01f;
        public LayerMask BreakableItemLayer = 0;
        public float RayForBreakableItem = 1.0f;
    }
    class DashProps {
        public float Charge { get; set; }
        public float EnergyRemain { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Pos { get; set; }
        public float MaxCharge { get; set; }
        public float MaxEnergy { get; set; }

    }

}
