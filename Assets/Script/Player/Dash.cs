namespace CJStudio.Dash.Player {
    using System.Collections.Generic;

    using Eccentric.Utils;

    using UnityEngine.InputSystem;
    using UnityEngine;
    [System.Serializable]
    class Dash : PlayerComponent {
        public event System.Action<DashProps> DashPrepare = null;
        public event System.Action DashEnded = null;
        public event System.Action<DashProps> EnergyChange = null;
        DashProps props = null;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        Timer timer = null;
        DashStats stats = null;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        bool bUsingDash = false;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        bool bCanDash = false;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        bool bAim = false;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        bool bUsingEnergy = false;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        float energy = 0f;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        float charge = 0f;
        float maxCharge = 0f;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        float velocity = 0f;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        Vector2 direction = Vector2.zero;
        float normalTimeScale = 0f;
        public Dash (Player player, DashStats stats) : base (player) {
            this.stats = stats;
            energy = stats.BasicEnergy;
            timer = new Timer ( );
            maxCharge = stats.BasicChargeTime;
            normalTimeScale = Time.timeScale;
            props = new DashProps ( );
            if (EnergyChange != null)
                EnergyChange (props);
            props.MaxCharge = stats.BasicChargeTime;
            props.MaxEnergy = stats.BasicEnergy;
        }
        override public void Tick ( ) {
            CheckCollision ( );
            if (bAim && !bUsingDash) {
                if (bUsingEnergy) {
                    energy -= Time.unscaledDeltaTime;
                    if (EnergyChange != null)
                        EnergyChange (props);
                    if (energy <= 0f) {
                        ResetState ( );
                        return;
                    }
                }
                if (timer.IsFinished) {
                    bUsingEnergy = true;
                    timer.Reset (stats.BasicChargeTime);
                    charge = 0f;
                }
                charge += Time.unscaledDeltaTime;
                DashPreparing ( );
            }

        }

        override public void FixedTick ( ) {
            //Start perform Dash
            if (bUsingDash) {
                UsingDash ( );
            }
        }

        void DashPreparing ( ) {
            props.Charge = charge;
            props.EnergyRemain = energy;
            props.Direction = direction;
            props.Pos = Player.Tf.position;
            if (DashPrepare != null)
                DashPrepare (props);
        }

        void DashAimFin ( ) {
            if (DashEnded != null)
                DashEnded ( );
        }

        void UseDash ( ) {
            if (bAim && !bUsingDash) {
                Time.timeScale = normalTimeScale;
                float distance = stats.ChargeMultiplier * charge / maxCharge;
                velocity = distance / stats.AnimTime;
                bUsingDash = true;
                timer.Reset (stats.AnimTime);
                DashAimFin ( );
            }
        }

        void UsingDash ( ) {
            if (timer.IsFinished) {
                ResetState ( );
                bUsingDash = false;
                bCanDash = false;
            }
            #region CHECK_BREAKABLE_OBJECT
            if (Player.RayCastController.IsCollide) {
                List<HitResult> results = Player.RayCastController.Result;
                foreach (HitResult o in results) {
                    if (o.hit2D.collider.tag == "Breakable") {
                        o.hit2D.collider.GetComponent<BreakableItem> ( ).Break ( );
                        ResetState ( );
                        bUsingDash = false;
                        bCanDash = true;
                    }
                }
            }
            #endregion
            Player.Rb.MovePosition (Player.Rb.position + direction * velocity * Time.fixedDeltaTime);
        }
        void ResetState ( ) {
            bAim = false;
            bUsingEnergy = false;
            charge = 0f;
            velocity = 0f;
            direction = Vector2.zero;
            Time.timeScale = normalTimeScale;
            DashAimFin ( );
        }

        void CheckCollision ( ) {
            if (Player.RayCastController.IsCollide)
                bCanDash = true;
        }

        void OnDashStarted (InputAction.CallbackContext ctx) {
            if (!bUsingDash && bCanDash) {
                ResetState ( );
                bAim = true;
                timer.Reset (stats.BasicChargeTime);
                direction = ctx.ReadValue<Vector2> ( ).normalized;
                Time.timeScale = stats.AimTimeScale;
            }
        }
        void OnDashPerformed (InputAction.CallbackContext ctx) {
            if (bAim && !bUsingDash) {
                direction = ctx.ReadValue<Vector2> ( ).normalized;
            }
        }
        void OnDashCanceled (InputAction.CallbackContext ctx) {
            if (!bUsingDash) {
                ResetState ( );
            }
        }
        void OnUsePressed (InputAction.CallbackContext ctx) {
            UseDash ( );
        }
        override public void OnEnable ( ) {
            Control.GamePlay.Use.started += OnUsePressed;
            Control.GamePlay.Dash.started += OnDashStarted;
            Control.GamePlay.Dash.performed += OnDashPerformed;
            Control.GamePlay.Dash.canceled += OnDashCanceled;
        }
        override public void OnDisable ( ) {
            Control.GamePlay.Use.started -= OnUsePressed;
            Control.GamePlay.Dash.started -= OnDashStarted;
            Control.GamePlay.Dash.performed -= OnDashPerformed;
            Control.GamePlay.Dash.canceled -= OnDashCanceled;
        }
    }

    [System.Serializable]
    class DashStats : PlayerStats {
        public float BasicChargeTime = 0f;
        public float BasicEnergy = 10f;
        public float ChargeMultiplier = 0f;
        public float AnimTime = .3f;
        public float AimTimeScale = .01f;
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
