namespace CJStudio.Dash.Player {
    using System.Collections.Generic;
    using System;

    using Eccentric.Utils;

    using UnityEngine.InputSystem;
    using UnityEngine;
    [System.Serializable]
    class Movement : PlayerComponent {
        MovementStats stats = null;
        EPlayerState state = EPlayerState.NORMAL;
        float originGravity = 0f;
        RayCastController rayCastController = null;
        Vector2 inputValue = Vector2.zero;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        float externalHoriVel = 0f;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        float frictionAccumulation = 0f;
        bool bJumpPressed = false;
        bool bCanJump = false;
        bool bWallSliding = false;
        bool bFaceRight = true;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif

        bool bExternalVel = false;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif

        bool bExternalVelPositive = false;
        #region LERP
        const float smoothTime = .1f;
        float velocityXSmoothing;
        #endregion
        public Movement (Player player, MovementStats stats) : base (player) {
            this.stats = stats;
            rayCastController = player.RayCastController;
            originGravity = player.Rb.gravityScale;
        }

        override public void Tick ( ) {
            CheckCollision ( );
            Move ( );
            Jump ( );
            #region WALL_SLIDE_VFX
            if (bWallSliding)
                Player.Particle.Play ( );
            else {
                Player.Particle.Clear ( );
                Player.Particle.Pause ( );
            }
            #endregion
            #region CHANGE_GRAVITY_SCALE
            if (Player.Rb.velocity.y <= 0f && !bWallSliding)
                Player.Rb.gravityScale = originGravity * stats.FallGravityMultiplier;
            else if (bWallSliding && Player.Rb.velocity.y <= 0f)
                Player.Rb.gravityScale = originGravity * stats.WallSlidingGravityMultiplier;
            else
                Player.Rb.gravityScale = originGravity;
            #endregion
            #region ANIMATOR_PAPAMETER
            Player.Anim.SetFloat ("velX", Mathf.Abs (inputValue.x));
            Player.Anim.SetFloat ("velY", Player.Rb.velocity.y);
            Player.Anim.SetBool ("wallSlide", bWallSliding);
            #endregion
            #region SPRITE_RENDER_DIRECTION
            if (!Player.IsDashing && Player.Rb.velocity.x != 0f) {
                if (bWallSliding)
                    bFaceRight = rayCastController.Right;
                else
                    bFaceRight = Player.Rb.velocity.x > 0f;
                Render.ChangeDirectionXWithSpriteRender (bFaceRight, Player.Rend, true);
            }
            #endregion
            #region TEST
            Player.deltaText.text = "fps:" + (1f / Time.unscaledDeltaTime).ToString ("0") + "\nVel:" + Player.Rb.velocity.x.ToString ("0.00") + "\nInput:" + inputValue.x.ToString ("0.00");
            #endregion

        }

        override public void FixedTick ( ) { }

        void Move ( ) {
            Vector2 nVel = Player.Rb.velocity;
            if (bExternalVel && rayCastController.Down) {
                nVel.x = inputValue.x * stats.NormalVel + externalHoriVel;
                if (externalHoriVel > 0f) {
                    externalHoriVel -= frictionAccumulation * Time.deltaTime;
                }
                else if (externalHoriVel < 0f) {
                    externalHoriVel += frictionAccumulation * Time.deltaTime;
                }
                if ((externalHoriVel >= 0f && !bExternalVelPositive) || (externalHoriVel <= 0f && bExternalVelPositive)) {
                    bExternalVel = false;
                    externalHoriVel = 0f;
                }
                // if (rayCastController.Left || rayCastController.Right) {
                //     bExternalVel = false;
                //     externalHoriVel = 0f;
                // }
            }
            else if (bExternalVel) {
                nVel.x = inputValue.x * stats.AirVel + externalHoriVel;
                if (externalHoriVel > 0f) {
                    externalHoriVel -= stats.AirFriction * Time.deltaTime;
                }
                else if (externalHoriVel < 0f) {
                    externalHoriVel += stats.AirFriction * Time.deltaTime;
                }
                if ((externalHoriVel >= 0f && !bExternalVelPositive) || (externalHoriVel <= 0f && bExternalVelPositive)) {
                    bExternalVel = false;
                    externalHoriVel = 0f;
                }
            }
            else if (rayCastController.Down) {
                nVel.x = inputValue.x * stats.NormalVel;
            }
            else {
                nVel.x += inputValue.x * stats.AirVel;
                nVel.x = Mathf.Clamp (nVel.x, -stats.NormalVel, stats.NormalVel);
                nVel.x = Mathf.SmoothDamp (Player.Rb.velocity.x, nVel.x, ref velocityXSmoothing, smoothTime);
            }
            Player.Rb.velocity = nVel;

        }

        void Jump ( ) {
            if (bCanJump && bJumpPressed) {
                Vector2 vel = Player.Rb.velocity;
                //Wall Jump
                if (bWallSliding && !rayCastController.Down) {
                    // EHitDirection wallDirection = rayCastController.Left?EHitDirection.LEFT : EHitDirection.RIGHT;
                    // vel.x = wallDirection == EHitDirection.LEFT?stats.WallJumpVel: -stats.WallJumpVel;
                    EHitDirection wallDirection = rayCastController.Left?EHitDirection.LEFT : EHitDirection.RIGHT;
                    // from left wall to right wall
                    if (inputValue.x >= 0f && wallDirection == EHitDirection.LEFT)
                        vel.x = stats.WallJumpVel;
                    // from right wall to left wall
                    else if (inputValue.x <= 0f && wallDirection == EHitDirection.RIGHT)
                        vel.x = -stats.WallJumpVel;
                    else
                        return;
                }
                vel.y = stats.JumpVel;
                Player.Rb.velocity = vel;
                bCanJump = false;
                Player.Anim.SetTrigger ("jump");
            }
        }

        void CheckCollision ( ) {
            bool preWallSlide = bWallSliding;
            bCanJump = false;
            bWallSliding = false;
            bExternalVel = false;

            //if there is external Vel exist calculate the friction accumulation
            //and in only will affect by one collider
            if (externalHoriVel != 0f) {
                bExternalVel = true;
                frictionAccumulation = 0f;
                foreach (HitResult o in rayCastController.Result) {
                    if (o.direction == EHitDirection.DOWN) {
                        frictionAccumulation = o.hit2D.collider.friction;
                        break;
                    }
                }
            }

            //if touches the wall and not on the ground set wall climbing
            if ((rayCastController.Left || rayCastController.Right) && !rayCastController.Down) {
                foreach (HitResult o in rayCastController.Result) {
                    if (o.detailPos.y == 0) {
                        //Check if this first time to grab the wall
                        if (!preWallSlide)
                            Player.Rb.velocity = new Vector2 (Player.Rb.velocity.x, 0f);
                        bWallSliding = true;
                        bCanJump = true;
                        break;
                    }
                }
            }

            //eliminate horizontal velocity when it toucheds the wall
            if (externalHoriVel > 0f && rayCastController.Right) {
                externalHoriVel = 0f;
                bExternalVel = false;
            }
            else if (externalHoriVel < 0f && rayCastController.Left) {
                externalHoriVel = 0f;
                bExternalVel = false;
            }

            if (rayCastController.Down) {
                bCanJump = true;
            }
        }

        override public void OnEnable ( ) {
            Control.GamePlay.Move.performed += OnMoveValueChanged;
            Control.GamePlay.Move.canceled += OnMoveValueReleased;
            Control.GamePlay.Jump.started += OnJumpPressed;
            Control.GamePlay.Jump.canceled += OnJumpReleased;
        }
        override public void OnDisable ( ) {
            Control.GamePlay.Move.performed -= OnMoveValueChanged;
            Control.GamePlay.Move.canceled -= OnMoveValueReleased;
            Control.GamePlay.Jump.started -= OnJumpPressed;
            Control.GamePlay.Jump.canceled -= OnJumpReleased;
        }

        void OnMoveValueChanged (InputAction.CallbackContext ctx) {
            inputValue = ctx.ReadValue<Vector2> ( );
        }
        void OnMoveValueReleased (InputAction.CallbackContext ctx) {
            inputValue = Vector2.zero;
        }

        void OnJumpPressed (InputAction.CallbackContext ctx) {
            bJumpPressed = true;
        }
        void OnJumpReleased (InputAction.CallbackContext ctx) {
            bJumpPressed = false;
        }

        public void AddHoriVelocity (float vel) {
            externalHoriVel += vel;
            bExternalVelPositive = externalHoriVel > 0f;
        }

        public void AddVertVelocity (float vel) {
            Vector2 nVel = Player.Rb.velocity;
            nVel.y += vel;
            Player.Rb.velocity = nVel;
        }
    }

    [System.Serializable]
    class MovementStats : PlayerStats {
        public float NormalVel = 250f;
        public float AirVel = 100f;
        public float JumpVel = 15f;
        public float WallJumpVel = 7.5f;
        public float FallGravityMultiplier = 1.5f;
        public float WallSlidingGravityMultiplier = 0.02f;
        public float AirFriction = 50f;
    }
    enum EPlayerState {
        NORMAL,
        WALL_SLIDE,
        SPRING,
        SPACE,
        SLIDE,
    }
}
