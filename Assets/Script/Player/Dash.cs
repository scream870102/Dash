namespace Dash.Player {
    using Eccentric.Utils;

    using UnityEngine.InputSystem;
    using UnityEngine;
    [System.Serializable]
    public class Dash : PlayerComponent {
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
        }
        override public void Tick ( ) {
            CheckCollision ( );
            if (bAim && !bUsingDash) {
                if (bUsingEnergy) {
                    energy -= Time.unscaledDeltaTime;
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
            }

        }

        override public void FixedTick ( ) {
            //Start perform Dash
            if (bUsingDash) {
                UsingDash ( );
            }
        }

        void UseDash ( ) {
            if (bAim && !bUsingDash) {
                Time.timeScale = normalTimeScale;
                float distance = stats.ChargeMultiplier * charge / maxCharge;
                velocity = distance / stats.AnimTime;
                bUsingDash = true;
                timer.Reset (stats.AnimTime);
            }
        }

        void UsingDash ( ) {
            if (timer.IsFinished) {
                ResetState ( );
                bUsingDash = false;
                bCanDash = false;
            }
            player.Rb.MovePosition (player.Rb.position + direction * velocity * Time.fixedDeltaTime);
        }
        void ResetState ( ) {
            bAim = false;
            bUsingEnergy = false;
            charge = 0f;
            velocity = 0f;
            direction = Vector2.zero;
            Time.timeScale = normalTimeScale;
        }

        void CheckCollision ( ) {
            if (player.RayCastController.IsCollide)
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
            control.GamePlay.Use.started += OnUsePressed;
            control.GamePlay.Dash.started += OnDashStarted;
            control.GamePlay.Dash.performed += OnDashPerformed;
            control.GamePlay.Dash.canceled += OnDashCanceled;
        }
        override public void OnDisable ( ) {
            control.GamePlay.Use.started -= OnUsePressed;
            control.GamePlay.Dash.started -= OnDashStarted;
            control.GamePlay.Dash.performed -= OnDashPerformed;
            control.GamePlay.Dash.canceled -= OnDashCanceled;
        }
    }

    [System.Serializable]
    public class DashStats : PlayerStats {
        public float BasicChargeTime = 0f;
        public float BasicEnergy = 10f;
        public float ChargeMultiplier = 0f;
        public float AnimTime = .3f;
        public float AimTimeScale = .01f;
    }
}
