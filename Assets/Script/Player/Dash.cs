namespace CJStudio.Dash.Player {
    using CJStudio.Dash.Camera;
    using CJStudio.Dash.MapObject;

    using Eccentric.Input;
    using Eccentric.Utils;
    using Eccentric;

    using UnityEngine.InputSystem;
    using UnityEngine;
    [System.Serializable]
    class Dash : PlayerComponent {
        const float DEG_FOR_CHECK = 5f;
        public event System.Action<DashProps> Aim = null;
        public event System.Action AimEnded = null;
        public event System.Action<DashProps> EnergyChange = null;
        public bool IsDashing => bUsingDash;
        public float Energy => energy;
        public bool CanDash => bCanDash;
        DashProps props = null;
        UnscaledTimer timer = null;
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
        float chargeJudge = 0f;
        Vector2 oriColSize = Vector2.zero;
        public Dash (Player player, DashStats stats) : base (player) {
            this.stats = stats;
            energy = stats.BasicEnergy;
            timer = new UnscaledTimer ( );
            normalTimeScale = Time.timeScale;
            props = new DashProps ( );
            EnergyChanging ( );
            props.MaxCharge = stats.MaxCharge;
            props.MaxEnergy = stats.BasicEnergy;
            chargeJudge = 0.5f * (stats.MinCharge + stats.MaxCharge) / stats.MaxCharge;
            oriColSize = Player.Col.size;
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
            float chargeRatio = charge / stats.MaxCharge;
            float distance = chargeRatio * (chargeRatio >= chargeJudge?stats.MaxChargeMultiplier : stats.MinChargeMultiplier);
            props.Distance = distance;
            if (Aim != null)
                Aim (props);
        }

        void AimAnimEnded ( ) {
            if (AimEnded != null)
                AimEnded ( );
        }

        //Call this method when player press use button to reset related value
        void UseDash ( ) {
            if (bAim && !bUsingDash && bCanDash) {
                Time.timeScale = normalTimeScale;
                velocity = props.Distance / stats.AnimTime;
                bUsingDash = true;
                bCanDash = false;
                timer.Reset (stats.AnimTime);
                AimAnimEnded ( );
                Player.Anim.SetBool ("dash", true);
                Player.GameController.CameraController.ShakeCamera (stats.DashShakeProps);
                Player.Rb.velocity = Vector2.zero;
                Player.FX.PlayVFX (EVFXType.TRAIL);
                GamepadController.VibrateController (EVibrateDuration.NORMAL, EVibrateStrength.STRONG);
                Player.Col.size = oriColSize * stats.DashColSizeMultiplier;
            }
        }

        void UsingDash ( ) {
            if (timer.IsFinished) {
                ResetState ( );
                bUsingDash = false;
                Player.Anim.SetBool ("dash", false);
            }
            // if touch breakable object reset the state and can using dash again
            #region CHECK_BREAKABLE_OBJECT
            RaycastHit2D result = Physics2D.Raycast (Player.Tf.position, direction, stats.RayForBreakableItem, stats.BreakableItemLayer);
            if (result.collider == null)result = Physics2D.Raycast (Player.Tf.position, Math.GetDirectionFromDeg (Math.GetDegree (direction) + DEG_FOR_CHECK), stats.RayForBreakableItem, stats.BreakableItemLayer);
            if (result.collider == null)result = Physics2D.Raycast (Player.Tf.position, Math.GetDirectionFromDeg (Math.GetDegree (direction) - DEG_FOR_CHECK), stats.RayForBreakableItem, stats.BreakableItemLayer);
            if (result.collider != null && result.collider.tag == "Breakable") {
                result.collider.GetComponent<BreakableObj> ( ).Break ( );
                bCanDash = true;
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
            Player.GameController.CameraController.DisableCameraShake ( );
            Player.Col.size = oriColSize;
            Time.timeScale = normalTimeScale;
            AimAnimEnded ( );
            Player.FX.StopVFX (EVFXType.TRAIL);
        }

        //Check if player bomb into any collider which can reset its dash state
        void CheckCollision ( ) {
            if (!bUsingDash && !bCanDash)
                bCanDash = Player.RayCastController.IsCollide;
        }

        override public void SetSaveData (SaveData data) {
            this.bCanDash = data.CanUseDash;
            this.energy = data.EnergyRemain;
            EnergyChanging ( );
        }

        void OnAimBtnStarted (InputAction.CallbackContext ctx) {
            if (!bUsingDash && bCanDash) {
                ResetState ( );
                bAim = true;
                Player.GameController.CameraController.SetCameraShake (stats.AimShakeProps);
                Player.Anim.SetBool ("aim", true);
                timer.Reset (stats.BasicChargeTime);
                direction = ctx.ReadValue<Vector2> ( ).normalized;
                Time.timeScale = stats.AimTimeScale;
            }
        }
        void OnAimBtnPerformed (InputAction.CallbackContext ctx) {
            if (bAim && !bUsingDash) {
                direction = ctx.ReadValue<Vector2> ( ).normalized;
            }
        }
        void OnAimBtnCanceled (InputAction.CallbackContext ctx) {
            if (!bUsingDash) {
                ResetState ( );
            }
        }
        void OnDashBtnPressed (InputAction.CallbackContext ctx) {
            UseDash ( );
        }
        override public void OnEnable ( ) {
            Control.GamePlay.Dash.started += OnDashBtnPressed;
            Control.GamePlay.Aim.started += OnAimBtnStarted;
            Control.GamePlay.Aim.performed += OnAimBtnPerformed;
            Control.GamePlay.Aim.canceled += OnAimBtnCanceled;
        }

        override public void OnDisable ( ) {
            Control.GamePlay.Dash.started -= OnDashBtnPressed;
            Control.GamePlay.Aim.started -= OnAimBtnStarted;
            Control.GamePlay.Aim.performed -= OnAimBtnPerformed;
            Control.GamePlay.Aim.canceled -= OnAimBtnCanceled;
        }
    }

    [System.Serializable]
    class DashStats : PlayerStats {
        public float BasicChargeTime => MaxCharge - MinCharge;
        public float MinCharge = 0f;
        public float MaxCharge = .6f;
        public float BasicEnergy = 3f;
        public float MaxChargeMultiplier = 6f;
        public float MinChargeMultiplier = 6f;
        public float AnimTime = .15f;
        public float AimTimeScale = .05f;
        public LayerMask BreakableItemLayer = 0;
        public float RayForBreakableItem = 1.25f;
        public CameraShakeProps DashShakeProps = null;
        public CameraShakeProps AimShakeProps = null;
        public float DashColSizeMultiplier = 0.5f;
    }
    class DashProps {
        public float Charge { get; set; }
        public float EnergyRemain { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Pos { get; set; }
        public float MaxCharge { get; set; }
        public float MaxEnergy { get; set; }
        public float Distance { get; set; }

    }

}
