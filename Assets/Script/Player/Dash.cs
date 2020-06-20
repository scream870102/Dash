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
        DashAttr attr = null;
        bool bUsingDash = false;
        bool bCanDash = false;
        bool bAim = false;
        bool bUsingEnergy = false;
        bool bInSpaceArea = false;
        float energy = 0f;
        float charge = 0f;
        float velocity = 0f;
        Vector2 direction = Vector2.zero;
        float normalTimeScale = 0f;
        float chargeJudge = 0f;
        Vector2 oriColSize = Vector2.zero;
        public Dash (Player player, DashAttr attr) : base (player) {
            this.attr = attr;
            energy = attr.BasicEnergy;
            timer = new UnscaledTimer ( );
            normalTimeScale = Time.timeScale;
            props = new DashProps ( );
            EnergyChanging ( );
            props.MaxCharge = attr.MaxCharge;
            props.MaxEnergy = attr.BasicEnergy;
            chargeJudge = 0.5f * (attr.MinCharge + attr.MaxCharge) / attr.MaxCharge;
            oriColSize = Player.Col.size;
        }

        override public void Tick ( ) {
            energy += attr.RecoverRate * Time.unscaledDeltaTime;
            if (energy > attr.BasicEnergy)
                energy = attr.BasicEnergy;
            EnergyChanging ( );
            if (bCanDash)
                Player.FX.PlayVFX (Player.VFXAction[EVFXAction.DASH_READY]);
            else
                Player.FX.StopVFX (Player.VFXAction[EVFXAction.DASH_READY]);
            CheckCollision ( );
            if (bAim && !bUsingDash) {
#if UNITY_EDITOR
                Debug.DrawRay (Player.Tf.position, direction * attr.RayForBreakableItem, Color.blue);
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
                    timer.Reset (attr.BasicChargeTime);
                    charge = attr.MinCharge;
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
            energy = Mathf.Clamp (newEnergy, 0f, attr.BasicEnergy);
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
            float chargeRatio = charge / attr.MaxCharge;
            float distance = chargeRatio * (chargeRatio >= chargeJudge?attr.MaxChargeMultiplier : attr.MinChargeMultiplier);
            props.Distance = distance;
            props.MaxDistance = attr.MaxChargeMultiplier;
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
                velocity = props.Distance / attr.AnimTime;
                bUsingDash = true;
                bCanDash = bInSpaceArea;
                timer.Reset (attr.AnimTime);
                AimAnimEnded ( );
                Player.Anim.SetBool ("dash", true);
                Player.GameController.CameraController.ShakeCamera (attr.DashShakeProps);
                Player.Rb.velocity = Vector2.zero;
                Player.FX.PlayVFX (Player.VFXAction[EVFXAction.DASH], Player.IsFacingRight, Math.GetDegree (direction), direction);
                Player.FX.PlaySFX (ESFXType.DASH);
                GamepadController.VibrateController (EVibrateDuration.NORMAL, EVibrateStrength.STRONG);
                Player.Col.size = oriColSize * attr.DashColSizeMultiplier;
            }
        }

        void UsingDash ( ) {
            if (timer.IsFinished) {
                ForceStopDash ( );
            }
            // if touch breakable object reset the state and can using dash again
#region CHECK_BREAKABLE_OBJECT
            RaycastHit2D result = Physics2D.Raycast (Player.Tf.position, direction, attr.RayForBreakableItem, attr.BreakableItemLayer);
            if (result.collider == null) result = Physics2D.Raycast (Player.Tf.position, Math.GetDirectionFromDeg (Math.GetDegree (direction) + DEG_FOR_CHECK), attr.RayForBreakableItem, attr.BreakableItemLayer);
            if (result.collider == null) result = Physics2D.Raycast (Player.Tf.position, Math.GetDirectionFromDeg (Math.GetDegree (direction) - DEG_FOR_CHECK), attr.RayForBreakableItem, attr.BreakableItemLayer);
            if (result.collider != null && result.collider.tag == "Breakable") {
                result.collider.GetComponent<BreakableObj> ( ).Break ( );
                bCanDash = true;
                Player.FX.PlaySFX (ESFXType.RESET_DASH);
            }
#endregion
            if (direction.x != 0f) {
                bool bFaceRight = direction.x > 0f;
                Render.ChangeDirectionXWithSpriteRender (bFaceRight, Player.Rend, Player.IsRendInvert);
            }
            Player.Rb.MovePosition (Player.Rb.position + direction * velocity * Time.fixedDeltaTime);
        }

        //Call this method to reset all vars before a new Dash
        void ResetState ( ) {
            Player.FX.StopVFX (Player.VFXAction[EVFXAction.CHARGE]);
            Player.FX.StopLoopSFX ( );
            bAim = false;
            Player.Anim.SetBool ("aim", false);
            bUsingEnergy = false;
            charge = attr.MinCharge;
            velocity = 0f;
            direction = Vector2.zero;
            Player.GameController.CameraController.DisableCameraShake ( );
            Player.Col.size = oriColSize;
            Time.timeScale = normalTimeScale;
            DomainEvents.Raise<OnAiming> (new OnAiming (false));
            AimAnimEnded ( );
            Player.FX.StopVFX (Player.VFXAction[EVFXAction.DASH]);
        }

        //Check if player bomb into any collider which can reset its dash state
        void CheckCollision ( ) {
            if (!bUsingDash && !bCanDash) {
                bCanDash = Player.RayCastController.IsCollide;
                if (bCanDash)
                    Player.FX.PlaySFX (ESFXType.RESET_DASH);
            }
        }

        override public void SetSaveData (SaveData data) {
            this.bCanDash = data.CanUseDash;
            this.energy = data.EnergyRemain;
            EnergyChanging ( );
        }

        void OnAimBtnStarted (InputAction.CallbackContext ctx) {
            if (bAim && !bUsingDash) {
                if (ctx.control.device.displayName == "Mouse") {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint (ctx.ReadValue<Vector2> ( ));
                    direction = (mousePos - (Vector2) Player.Tf.position).normalized;
                }
                else {
                    direction = ctx.ReadValue<Vector2> ( ).normalized;
                }
            }
        }
        void OnAimBtnPerformed (InputAction.CallbackContext ctx) {
            if (bAim && !bUsingDash) {
                if (ctx.control.device.displayName == "Mouse") {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint (ctx.ReadValue<Vector2> ( ));
                    direction = (mousePos - (Vector2) Player.Tf.position).normalized;
                }
                else {
                    direction = ctx.ReadValue<Vector2> ( ).normalized;
                }
            }
        }
        void OnJumpBtnStarted (InputAction.CallbackContext ctx) {
            UseDash ( );
        }
        void OnDashBtnStarted (InputAction.CallbackContext ctx) {
            if (!bUsingDash && bCanDash) {
                ResetState ( );
                bAim = true;
                Player.GameController.CameraController.SetCameraShake (attr.AimShakeProps);
                Player.Anim.SetBool ("aim", true);
                timer.Reset (attr.BasicChargeTime);
                Vector2 tmp = Control.GamePlay.Aim.ReadValue<Vector2> ( ).normalized;
                direction = tmp == Vector2.zero?Vector2.right : tmp;
                Time.timeScale = attr.AimTimeScale;
                Player.FX.PlayVFX (Player.VFXAction[EVFXAction.CHARGE]);
                Player.FX.PlayLoopSFX (ESFXType.CHARGE);
                DomainEvents.Raise<OnAiming> (new OnAiming (true));
            }
        }

        void OnDashBtnCanceled (InputAction.CallbackContext ctx) {
            if (!bUsingDash) {
                ResetState ( );
            }
        }

        void OnSpaceAreaEnter (OnSpaceAreaEnter e) {
            if (e.IsEnter) {
                bInSpaceArea = true;
                bCanDash = true;
            }
            else {
                bInSpaceArea = false;
            }

        }

        override public void OnEnable ( ) {
            Control.GamePlay.Dash.started += OnDashBtnStarted;
            Control.GamePlay.Dash.canceled += OnDashBtnCanceled;
            Control.GamePlay.Jump.started += OnJumpBtnStarted;
            Control.GamePlay.Aim.started += OnAimBtnStarted;
            Control.GamePlay.Aim.performed += OnAimBtnPerformed;
            DomainEvents.Register<OnSpaceAreaEnter> (OnSpaceAreaEnter);
        }

        override public void OnDisable ( ) {
            Control.GamePlay.Dash.started -= OnDashBtnStarted;
            Control.GamePlay.Dash.canceled -= OnDashBtnCanceled;
            Control.GamePlay.Jump.started -= OnJumpBtnStarted;
            Control.GamePlay.Aim.started -= OnAimBtnStarted;
            Control.GamePlay.Aim.performed -= OnAimBtnPerformed;
            DomainEvents.UnRegister<OnSpaceAreaEnter> (OnSpaceAreaEnter);
        }

        public void ForceStopDash (bool IsResetDash = false) {
            ResetState ( );
            bUsingDash = false;
            Player.Anim.SetBool ("dash", false);
            if (IsResetDash) {
                bCanDash = true;
                Player.FX.PlaySFX (ESFXType.RESET_DASH);
            }
        }
    }

    [System.Serializable]
    class DashAttr : PlayerAttr {
        public float BasicChargeTime => MaxCharge - MinCharge;
        public float MinCharge = .3f;
        public float MaxCharge = .8f;
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
        public float RecoverRate = .1f;
    }
    class DashProps {
        public float Charge { get; set; }
        public float EnergyRemain { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Pos { get; set; }
        public float MaxCharge { get; set; }
        public float MaxEnergy { get; set; }
        public float Distance { get; set; }
        public float MaxDistance { get; set; }

    }

}