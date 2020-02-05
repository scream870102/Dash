namespace CJStudio.Dash.Player {
    using System.Collections.Generic;
    using System;

    using Eccentric.Utils;
    using Eccentric;

    using UnityEngine.InputSystem.DualShock;
    using UnityEngine.InputSystem;
    using UnityEngine;
    [System.Serializable]
    class Movement : PlayerComponent {
        MovementStats stats = null;
        IMoveStrategy strategy = null;
        BasicMoveStrategy basicMove = null;
        [SerializeField] SpaceMoveStrategy spaceMove = null;
        [SerializeField] AMoveAttr attr = null;
        RayCastController rayCastController = null;

        bool bFaceRight = true;

        public Movement (Player player, MovementStats stats) : base (player) {
            this.stats = stats;
            rayCastController = player.RayCastController;
            attr = new AMoveAttr ( );
            basicMove = new BasicMoveStrategy (ref attr, ref stats, player);
            spaceMove = new SpaceMoveStrategy (ref attr, ref stats, player);
            strategy = basicMove;
            attr.originGravity = player.Rb.gravityScale;
        }

        override public void Tick ( ) {
            CheckCollision ( );
            Move ( );
            Jump ( );
            #region WALL_SLIDE_VFX
            if (attr.bWallSliding)
                Player.Particle.Play ( );
            else {
                Player.Particle.Clear ( );
                Player.Particle.Pause ( );
            }
            #endregion
            #region ANIMATOR_PAPAMETER
            Player.Anim.SetFloat ("velX", Mathf.Abs (attr.inputValue.x));
            Player.Anim.SetFloat ("velY", Player.Rb.velocity.y);
            Player.Anim.SetBool ("wallSlide", attr.bWallSliding);
            #endregion
            #region SPRITE_RENDER_DIRECTION
            if (!Player.IsDashing && Player.Rb.velocity.x != 0f) {
                if (attr.bWallSliding)
                    bFaceRight = rayCastController.Right;
                else
                    bFaceRight = Player.Rb.velocity.x > 0f;
                Render.ChangeDirectionXWithSpriteRender (bFaceRight, Player.Rend, true);
            }
            #endregion
            #region TEST
            Player.deltaText.text = "fps:" + (1f / Time.unscaledDeltaTime).ToString ("0") + "\nVel:" + Player.Rb.velocity.x.ToString ("0.00") + "\nInput:" + attr.inputValue.x.ToString ("0.00");
            #endregion

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
            Control.GamePlay.Move.performed += OnMoveValueChanged;
            Control.GamePlay.Move.canceled += OnMoveValueReleased;
            Control.GamePlay.Jump.started += OnJumpPressed;
            Control.GamePlay.Jump.canceled += OnJumpReleased;
            DomainEvents.Register<OnSpaceAreaEnter> (OnSpaceAreaEnter);
        }
        override public void OnDisable ( ) {
            Control.GamePlay.Move.performed -= OnMoveValueChanged;
            Control.GamePlay.Move.canceled -= OnMoveValueReleased;
            Control.GamePlay.Jump.started -= OnJumpPressed;
            Control.GamePlay.Jump.canceled -= OnJumpReleased;
            DomainEvents.UnRegister<OnSpaceAreaEnter> (OnSpaceAreaEnter);
        }

        void OnMoveValueChanged (InputAction.CallbackContext ctx) {
            attr.inputValue = ctx.ReadValue<Vector2> ( );
        }
        void OnMoveValueReleased (InputAction.CallbackContext ctx) {
            attr.inputValue = Vector2.zero;
        }

        void OnJumpPressed (InputAction.CallbackContext ctx) {
            attr.bJumpPressed = true;
        }
        void OnJumpReleased (InputAction.CallbackContext ctx) {
            attr.bJumpPressed = false;
        }

        public void AddHoriVelocity (float vel) {
            attr.externalHoriVel += vel;
            attr.bExternalVelPositive = attr.externalHoriVel > 0f;
        }

        public void AddVertVelocity (float vel) {
            Vector2 nVel = Player.Rb.velocity;
            nVel.y += vel;
            Player.Rb.velocity = nVel;
        }

        void OnSpaceAreaEnter (OnSpaceAreaEnter e) {
            if (e.IsEnter) {
                strategy = spaceMove;
                strategy.Init ( );
                Player.Rb.gravityScale = 0f;
                Player.Anim.SetBool ("space", true);
            }
            else {
                strategy = basicMove;
                strategy.Init ( );
                Player.Rb.gravityScale = attr.originGravity;
                Player.Anim.SetBool ("space", false);
            }
        }

        override public void SetSaveData (SaveData data) {
            base.SetSaveData (data);
            attr.Init ( );
        }
    }

    abstract class IMoveStrategy {
        protected MovementStats stats = null;
        protected AMoveAttr attr;
        protected Player player = null;
        protected RayCastController rayCastController;
        public IMoveStrategy (ref AMoveAttr attr, ref MovementStats stats, Player player) {
            this.attr = attr;
            this.player = player;
            this.rayCastController = player.RayCastController;
            this.stats = stats;
        }

        abstract public void Move ( );
        abstract public void Jump ( );
        abstract public void CheckCollision ( );
        virtual public void Init ( ) { }
    }
    class BasicMoveStrategy : IMoveStrategy {
        public BasicMoveStrategy (ref AMoveAttr attr, ref MovementStats stats, Player player) : base (ref attr, ref stats, player) { }
        override public void Move ( ) {
            Vector2 nVel = player.Rb.velocity;
            if (attr.bExternalVel && rayCastController.Down) {
                nVel.x = attr.inputValue.x * stats.NormalVel + attr.externalHoriVel;
                if (attr.externalHoriVel > 0f) {
                    attr.externalHoriVel -= attr.frictionAccumulation * Time.deltaTime;
                }
                else if (attr.externalHoriVel < 0f) {
                    attr.externalHoriVel += attr.frictionAccumulation * Time.deltaTime;
                }
                if ((attr.externalHoriVel >= 0f && !attr.bExternalVelPositive) || (attr.externalHoriVel <= 0f && attr.bExternalVelPositive)) {
                    attr.bExternalVel = false;
                    attr.externalHoriVel = 0f;
                }
            }
            else if (attr.bExternalVel) {
                nVel.x = attr.inputValue.x * stats.AirVel + attr.externalHoriVel;
                if (attr.externalHoriVel > 0f) {
                    attr.externalHoriVel -= stats.AirFriction * Time.deltaTime;
                }
                else if (attr.externalHoriVel < 0f) {
                    attr.externalHoriVel += stats.AirFriction * Time.deltaTime;
                }
                if ((attr.externalHoriVel >= 0f && !attr.bExternalVelPositive) || (attr.externalHoriVel <= 0f && attr.bExternalVelPositive)) {
                    attr.bExternalVel = false;
                    attr.externalHoriVel = 0f;
                }
            }
            else if (rayCastController.Down) {
                nVel.x = attr.inputValue.x * stats.NormalVel;
            }
            else {
                nVel.x += attr.inputValue.x * stats.AirVel;
                nVel.x = Mathf.Clamp (nVel.x, -stats.NormalVel, stats.NormalVel);
                nVel.x = Mathf.SmoothDamp (player.Rb.velocity.x, nVel.x, ref attr.velocityXSmoothing, AMoveAttr.smoothTime);
            }
            player.Rb.velocity = nVel;
        }

        override public void Jump ( ) {
            if (attr.bCanJump && attr.bJumpPressed) {
                Vector2 vel = player.Rb.velocity;
                //Wall Jump
                if (attr.bWallSliding && !rayCastController.Down) {
                    // EHitDirection wallDirection = rayCastController.Left?EHitDirection.LEFT : EHitDirection.RIGHT;
                    // vel.x = wallDirection == EHitDirection.LEFT?stats.WallJumpVel: -stats.WallJumpVel;
                    EHitDirection wallDirection = rayCastController.Left?EHitDirection.LEFT : EHitDirection.RIGHT;
                    // from left wall to right wall
                    if (attr.inputValue.x >= 0f && wallDirection == EHitDirection.LEFT)
                        vel.x = stats.WallJumpVel;
                    // from right wall to left wall
                    else if (attr.inputValue.x <= 0f && wallDirection == EHitDirection.RIGHT)
                        vel.x = -stats.WallJumpVel;
                    else
                        return;
                }
                vel.y = stats.JumpVel;
                player.Rb.velocity = vel;
                attr.bCanJump = false;
                player.Anim.SetTrigger ("jump");
            }
            #region CHANGE_GRAVITY_SCALE
            if (player.Rb.velocity.y <= 0f && !attr.bWallSliding)
                player.Rb.gravityScale = attr.originGravity * stats.FallGravityMultiplier;
            else if (attr.bWallSliding && player.Rb.velocity.y <= 0f)
                player.Rb.gravityScale = attr.originGravity * stats.WallSlidingGravityMultiplier;
            else
                player.Rb.gravityScale = attr.originGravity;
            #endregion
        }

        override public void CheckCollision ( ) {
            bool preWallSlide = attr.bWallSliding;
            attr.bCanJump = false;
            attr.bWallSliding = false;
            attr.bExternalVel = false;

            //if there is external Vel exist calculate the friction accumulation
            //and in only will affect by one collider
            if (attr.externalHoriVel != 0f) {
                attr.bExternalVel = true;
                attr.frictionAccumulation = 0f;
                foreach (HitResult o in rayCastController.Result) {
                    if (o.direction == EHitDirection.DOWN) {
                        attr.frictionAccumulation = o.hit2D.collider.friction;
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
                        attr.bWallSliding = true;
                        attr.bCanJump = true;
                        break;
                    }
                }
            }

            //eliminate horizontal velocity when it toucheds the wall
            if (attr.externalHoriVel > 0f && rayCastController.Right) {
                attr.externalHoriVel = 0f;
                attr.bExternalVel = false;
            }
            else if (attr.externalHoriVel < 0f && rayCastController.Left) {
                attr.externalHoriVel = 0f;
                attr.bExternalVel = false;
            }

            if (rayCastController.Down) {
                attr.bCanJump = true;
            }
        }
    }

    [System.Serializable]
    class SpaceMoveStrategy : IMoveStrategy {
        [SerializeField] ScaledTimer timer = null;
        float jumpFadeVel = 0f;
        bool bJumping = false;
        Vector2 direction = Vector2.zero;
        float jumpVel = 0f;
        public SpaceMoveStrategy (ref AMoveAttr attr, ref MovementStats stats, Player player) : base (ref attr, ref stats, player) {
            timer = new ScaledTimer (stats.SpaceJumpDuration);
            jumpFadeVel = stats.SpaceJumpVel / stats.SpaceJumpDuration;
        }
        override public void Move ( ) {
            Vector2 nVel = player.Rb.velocity;
            nVel.x = attr.inputValue.x * stats.SpaceVel;
            nVel.y = attr.inputValue.y * stats.SpaceVel;
            player.Rb.velocity = nVel;
        }
        override public void Jump ( ) {
            if (attr.bCanJump && attr.bJumpPressed) {
                timer.Reset (stats.SpaceJumpDuration);
                bJumping = true;
                attr.bCanJump = false;
                direction = attr.inputValue;
                jumpVel = stats.SpaceJumpVel;
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
                attr.bCanJump = true;
            }
        }

        override public void CheckCollision ( ) {
            if (rayCastController.IsCollide)
                jumpVel = 0f;

        }

        override public void Init ( ) {
            player.Rb.velocity = Vector2.zero;
            attr.bCanJump = true;
            bJumping = false;
            jumpVel = 0f;
        }
    }

    [System.Serializable]
    class AMoveAttr {
        public Vector2 inputValue = Vector2.zero;
        public float externalHoriVel = 0f;
        public float frictionAccumulation = 0f;
        public bool bJumpPressed = false;
        public bool bCanJump = false;
        public bool bWallSliding = false;
        public bool bExternalVel = false;
        public bool bExternalVelPositive = false;
        public float originGravity = 0f;
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
    class MovementStats : PlayerStats {
        public float NormalVel = 7.5f;
        public float AirVel = 5.25f;
        public float JumpVel = 15f;
        public float WallJumpVel = 5f;
        public float FallGravityMultiplier = 1.5f;
        public float WallSlidingGravityMultiplier = 0.02f;
        public float AirFriction = 50f;
        #region  SPACE
        public float SpaceVel = 3f;
        public float SpaceJumpVel = 10f;
        public float SpaceJumpDuration = 10f;
        #endregion
    }
}
