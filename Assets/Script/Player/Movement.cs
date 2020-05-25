namespace CJStudio.Dash.Player {
    using System.Collections.Generic;
    using System;
    using Eccentric.Utils;
    using Eccentric;
    using UnityEngine.InputSystem;
    using UnityEngine;
    [System.Serializable]
    class Movement : PlayerComponent {
        MovementAttr attr = null;
        IMoveStrategy strategy = null;
        BasicMoveStrategy basicMove = null;
        SpaceMoveStrategy spaceMove = null;
        SlideMoveStrategy slideMove = null;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        MovementProps props = null;
        RayCastController rayCastController = null;
        public bool IsFacingRight => props.bFaceRight;
        public Movement (Player player, MovementAttr stats) : base (player) {
            this.attr = stats;
            rayCastController = player.RayCastController;
            props = new MovementProps ( );
            basicMove = new BasicMoveStrategy (ref props, ref stats, player);
            spaceMove = new SpaceMoveStrategy (ref props, ref stats, player);
            slideMove = new SlideMoveStrategy (ref props, ref stats, player);
            strategy = basicMove;
            props.originGravity = player.Rb.gravityScale;
        }

        override public void Tick ( ) {
            CheckCollision ( );
            Move ( );
            Jump ( );
#region WALL_SLIDE_VFX
            if (props.bWallSliding) Player.FX.PlayVFX (Player.VFXAction[EVFXAction.GRAB], props.bFaceRight);
            else Player.FX.StopVFX (Player.VFXAction[EVFXAction.GRAB]);
#endregion
#region ANIMATOR_PAPAMETER
            Player.Anim.SetFloat ("velX", Mathf.Abs (props.inputValue.x));
            Player.Anim.SetFloat ("velY", Player.Rb.velocity.y);
            Player.Anim.SetBool ("wallSlide", props.bWallSliding);
#endregion
#region SPRITE_RENDER_DIRECTION
            if (!Player.IsDashing && Player.Rb.velocity.x != 0f) {
                if (props.bWallSliding)
                    props.bFaceRight = rayCastController.Right;
                else
                    props.bFaceRight = Player.Rb.velocity.x > 0f;
                Render.ChangeDirectionXWithSpriteRender (props.bFaceRight, Player.Rend, Player.IsRendInvert);
            }
#endregion
#if UNITY_EDITOR
#region TEST
            Player.deltaText.text = "fps:" + (1f / Time.unscaledDeltaTime).ToString ("0") + "\nVel:" + Player.Rb.velocity.x.ToString ("0.00");
#endregion
#endif
        }

        override public void FixedTick ( ) { }

        void Move ( ) {
            strategy.Move ( );
        }

        void Jump ( ) {
            strategy.Jump ( );
        }

        void CheckCollision ( ) {
            strategy.CheckCollision ( );
        }

        override public void OnEnable ( ) {
            Control.GamePlay.Move.performed += OnMoveBtnPerformed;
            Control.GamePlay.Move.canceled += OnMoveBtnCanceled;
            Control.GamePlay.Jump.started += OnJumpBtnStarted;
            Control.GamePlay.Jump.canceled += OnJumpBtnCanceled;
            DomainEvents.Register<OnSpaceAreaEnter> (OnSpaceAreaEnter);
            DomainEvents.Register<OnSlideAreaEnter> (OnSlideAreaEnter);
        }
        override public void OnDisable ( ) {
            Control.GamePlay.Move.performed -= OnMoveBtnPerformed;
            Control.GamePlay.Move.canceled -= OnMoveBtnCanceled;
            Control.GamePlay.Jump.started -= OnJumpBtnStarted;
            Control.GamePlay.Jump.canceled -= OnJumpBtnCanceled;
            DomainEvents.UnRegister<OnSpaceAreaEnter> (OnSpaceAreaEnter);
            DomainEvents.UnRegister<OnSlideAreaEnter> (OnSlideAreaEnter);
        }

        void OnMoveBtnPerformed (InputAction.CallbackContext ctx) {
            props.inputValue = ctx.ReadValue<Vector2> ( );
        }
        void OnMoveBtnCanceled (InputAction.CallbackContext ctx) {
            props.inputValue = Vector2.zero;
        }

        void OnJumpBtnStarted (InputAction.CallbackContext ctx) {
            props.bJumpPressed = true;
        }
        void OnJumpBtnCanceled (InputAction.CallbackContext ctx) {
            props.bJumpPressed = false;
        }

        public void AddHoriVelocity (float vel, bool IsResetVel = false) {
            Vector2 nVel = Player.Rb.velocity;
            if (IsResetVel) nVel.y = 0f;
            Player.Rb.velocity = nVel;
            props.externalHoriVel += vel;
            props.bExternalVelPositive = props.externalHoriVel > 0f;
        }

        public void AddVertVelocity (float vel, bool IsResetVel = false) {
            Vector2 nVel = Player.Rb.velocity;
            if (IsResetVel) nVel.y = 0f;
            nVel.y += vel;
            Player.Rb.velocity = nVel;
        }

        void OnSpaceAreaEnter (OnSpaceAreaEnter e) {
            if (e.IsEnter) {
                strategy = spaceMove;
                strategy.Init ( );
                Player.Anim.SetBool ("space", true);
            }
            else if (strategy == spaceMove) {
                strategy = basicMove;
                strategy.Init ( );
                Player.Anim.SetBool ("space", false);
            }
        }

        void OnSlideAreaEnter (OnSlideAreaEnter e) {
            if (e.IsEnter) {
                strategy = slideMove;
                strategy.Init ( );
            }
            else if (strategy == slideMove) {
                strategy = basicMove;
                strategy.Init ( );
            }
        }

        override public void SetSaveData (SaveData data) {
            base.SetSaveData (data);
            props.Init ( );
        }
    }

    abstract class IMoveStrategy {
        protected MovementAttr attr = null;
        protected MovementProps props;
        protected Player player = null;
        protected RayCastController rayCastController;
        public IMoveStrategy (ref MovementProps props, ref MovementAttr attr, Player player) {
            this.props = props;
            this.player = player;
            this.rayCastController = player.RayCastController;
            this.attr = attr;
        }

        abstract public void Move ( );
        abstract public void Jump ( );
        abstract public void CheckCollision ( );
        virtual public void Init ( ) { }
    }
    class BasicMoveStrategy : IMoveStrategy {
        public BasicMoveStrategy (ref MovementProps props, ref MovementAttr attr, Player player) : base (ref props, ref attr, player) { }
        override public void Init ( ) {
            player.Rb.gravityScale = props.originGravity;
        }
        override public void Move ( ) {
            Vector2 nVel = player.Rb.velocity;
            if (props.bExternalVel && rayCastController.Down) {
                nVel.x = props.inputValue.x * attr.NormalVel + props.externalHoriVel;
                if (props.externalHoriVel > 0f) {
                    props.externalHoriVel -= props.frictionAccumulation * Time.deltaTime;
                }
                else if (props.externalHoriVel < 0f) {
                    props.externalHoriVel += props.frictionAccumulation * Time.deltaTime;
                }
                if ((props.externalHoriVel >= 0f && !props.bExternalVelPositive) || (props.externalHoriVel <= 0f && props.bExternalVelPositive)) {
                    props.bExternalVel = false;
                    props.externalHoriVel = 0f;
                }
            }
            else if (props.bExternalVel) {
                nVel.x = props.inputValue.x * attr.AirVel + props.externalHoriVel;
                if (props.externalHoriVel > 0f) {
                    props.externalHoriVel -= attr.AirFriction * Time.deltaTime;
                }
                else if (props.externalHoriVel < 0f) {
                    props.externalHoriVel += attr.AirFriction * Time.deltaTime;
                }
                if ((props.externalHoriVel >= 0f && !props.bExternalVelPositive) || (props.externalHoriVel <= 0f && props.bExternalVelPositive)) {
                    props.bExternalVel = false;
                    props.externalHoriVel = 0f;
                }
            }
            else if (rayCastController.Down) {
                nVel.x = props.inputValue.x * attr.NormalVel;
            }
            else {
                nVel.x += props.inputValue.x * attr.AirVel;
                nVel.x = Mathf.Clamp (nVel.x, -attr.NormalVel, attr.NormalVel);
                nVel.x = Mathf.SmoothDamp (player.Rb.velocity.x, nVel.x, ref props.velocityXSmoothing, MovementProps.smoothTime);
            }
            player.Rb.velocity = nVel;
        }

        override public void Jump ( ) {
            //如果角色在Dash的途中仍然沒有放開Jump Button 將其判定改為沒有按住
            if (props.bJumpPressed && player.IsDashing) {
                props.bJumpPressed = false;
            }
            if (props.bCanJump && props.bJumpPressed && !player.IsDashing) {
                Vector2 vel = player.Rb.velocity;
                //Wall Jump
                if (props.bWallSliding && !rayCastController.Down) {
                    EHitDirection wallDirection = rayCastController.Left?EHitDirection.LEFT : EHitDirection.RIGHT;
                    // from left wall to right wall
                    if (wallDirection == EHitDirection.LEFT)
                        vel.x = attr.WallJumpVel;
                    else
                        vel.x = -attr.WallJumpVel;
                }
                vel.y = attr.JumpVel;
                player.Rb.velocity = vel;
                props.bCanJump = false;
                props.bJumpPressed = false;
                player.Anim.SetTrigger ("jump");
                player.FX.PlayVFX (player.VFXAction[EVFXAction.JUMP], props.bFaceRight);
                player.FX.PlaySFX (ESFXType.JUMP);
            }
#region WALL_SLIDE_DOWN_VEL
            if (props.bWallSliding) {
                Vector2 vel = player.Rb.velocity;
                float input = props.inputValue.y > 0f?0f : props.inputValue.y;
                vel.y += input * attr.WallSlideVel;
                player.Rb.velocity = vel;
            }
#endregion
#region CHANGE_GRAVITY_SCALE
            if (player.Rb.velocity.y <= 0f && !props.bWallSliding)
                player.Rb.gravityScale = props.originGravity * attr.FallGravityMultiplier;
            else if (props.bWallSliding && player.Rb.velocity.y <= 0f)
                player.Rb.gravityScale = props.originGravity * attr.WallSlidingGravityMultiplier;
            else
                player.Rb.gravityScale = props.originGravity;
#endregion
        }

        override public void CheckCollision ( ) {
            bool preWallSlide = props.bWallSliding;
            props.bCanJump = false;
            props.bWallSliding = false;
            props.bExternalVel = false;

            //if there is external Vel exist calculate the friction accumulation
            //and in only will affect by one collider
            if (props.externalHoriVel != 0f) {
                props.bExternalVel = true;
                props.frictionAccumulation = 0f;
                foreach (HitResult o in rayCastController.Result) {
                    if (o.direction == EHitDirection.DOWN) {
                        props.frictionAccumulation = o.hit2D.collider.friction;
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
                            player.Rb.velocity = new Vector2 (player.Rb.velocity.x, 0f);
                        props.bWallSliding = true;
                        props.bCanJump = true;
                        break;
                    }
                }
            }

            //eliminate horizontal velocity when it toucheds the wall
            if (props.externalHoriVel > 0f && rayCastController.Right) {
                props.externalHoriVel = 0f;
                props.bExternalVel = false;
            }
            else if (props.externalHoriVel < 0f && rayCastController.Left) {
                props.externalHoriVel = 0f;
                props.bExternalVel = false;
            }

            if (rayCastController.Down) {
                props.bCanJump = true;
            }
        }
    }

    class SpaceMoveStrategy : IMoveStrategy {
        [SerializeField] ScaledTimer timer = null;
        float jumpFadeVel = 0f;
        bool bJumping = false;
        Vector2 direction = Vector2.zero;
        float jumpVel = 0f;
        public SpaceMoveStrategy (ref MovementProps props, ref MovementAttr attr, Player player) : base (ref props, ref attr, player) {
            timer = new ScaledTimer (attr.SpaceJumpDuration);
            jumpFadeVel = attr.SpaceJumpVel / attr.SpaceJumpDuration;
        }
        override public void Move ( ) {
            Vector2 nVel = player.Rb.velocity;
            nVel.x = props.inputValue.x * attr.SpaceVel;
            nVel.y = props.inputValue.y * attr.SpaceVel;
            player.Rb.velocity = nVel;
        }
        override public void Jump ( ) {
            if (props.bCanJump && props.bJumpPressed) {
                timer.Reset (attr.SpaceJumpDuration);
                bJumping = true;
                props.bCanJump = false;
                direction = props.inputValue;
                jumpVel = attr.SpaceJumpVel;
                player.Anim.SetTrigger ("spaceJump");
            }
            if (bJumping && !timer.IsFinished && jumpVel > 0f) {
                Vector2 nVel = player.Rb.velocity;
                nVel += direction * jumpVel;
                jumpVel -= jumpFadeVel * Time.deltaTime;
                player.Rb.velocity = nVel;
            }
            else if (bJumping && timer.IsFinished) {
                bJumping = false;
                props.bCanJump = true;
            }
        }

        override public void CheckCollision ( ) {
            if (rayCastController.IsCollide)
                jumpVel = 0f;
        }

        override public void Init ( ) {
            player.Rb.velocity = Vector2.zero;
            props.bCanJump = true;
            player.Rb.gravityScale = 0f;
            props.externalHoriVel = 0f;
            bJumping = false;
            jumpVel = 0f;
        }
    }

    class SlideMoveStrategy : BasicMoveStrategy {
        Vector2 direction = Vector2.zero;
        bool bTouchedGround = false;
        public SlideMoveStrategy (ref MovementProps props, ref MovementAttr attr, Player player) : base (ref props, ref attr, player) { }
        override public void Move ( ) {
            if (direction == Vector2.zero) {
                direction = props.inputValue;
                direction.x = direction.x > 0f?1f : direction.x < 0f? - 1f : 0f;
            }
            if (bTouchedGround) {
                direction = props.inputValue;
                direction.x = direction.x > 0f?1f : direction.x < 0f? - 1f : 0f;
                bTouchedGround = false;
            }
            Vector2 nVel = player.Rb.velocity;
            nVel.x = direction.x * attr.NormalVel;
            player.Rb.velocity = nVel;
        }
        override public void CheckCollision ( ) {
            props.bWallSliding = false;
            props.bCanJump = rayCastController.Down;
            bTouchedGround = (rayCastController.Left || rayCastController.Right);
        }

        override public void Init ( ) {
            player.Rb.velocity = Vector2.zero;
            props.externalHoriVel = 0f;
            bTouchedGround = true;
        }
    }

    [System.Serializable]
    class MovementProps {
        public Vector2 inputValue = Vector2.zero;
        public float externalHoriVel = 0f;
        public float frictionAccumulation = 0f;
        public bool bJumpPressed = false;
        public bool bCanJump = false;
        public bool bWallSliding = false;
        public bool bExternalVel = false;
        public bool bExternalVelPositive = false;
        public float originGravity = 0f;
        public bool bFaceRight = false;
#region LERP
        public const float smoothTime = .1f;
        public float velocityXSmoothing;
#endregion

        public void Init ( ) {
            inputValue = Vector2.zero;
            externalHoriVel = 0f;
            frictionAccumulation = 0f;
            bJumpPressed = false;
            bCanJump = false;
            bWallSliding = false;
            bExternalVel = false;
            bExternalVelPositive = false;
        }
    }

    [System.Serializable]
    class MovementAttr : PlayerAttr {
        public float NormalVel = 7.5f;
        public float AirVel = 5.25f;
        public float JumpVel = 15f;
        public float WallJumpVel = 5f;
        public float WallSlideVel = .3f;
        public float FallGravityMultiplier = 1.5f;
        public float WallSlidingGravityMultiplier = 0.02f;
        public float AirFriction = 50f;
#region  SPACE
        public float SpaceVel = 3f;
        public float SpaceJumpVel = 10f;
        public float SpaceJumpDuration = 2f;
#endregion
    }
}