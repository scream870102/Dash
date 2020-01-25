namespace Dash.Player {
    using System.Collections.Generic;
    using System;

    using Eccentric.Utils;

    using UnityEngine.InputSystem;
    using UnityEngine;
    [System.Serializable]
    public class Movement : PlayerComponent {
        float originGravity = 0f;
        RayCastController rayCastController = null;
        MovementStats stats = null;
        Vector2 inputValue = Vector2.zero;
        bool bJumpPressed = false;
        bool bCanJump = false;
        bool bWallSliding = false;
        float velocityXSmoothing;
        const float smoothTime = .1f;
        public Movement (Player player, MovementStats stats) : base (player) {
            this.stats = stats;
            rayCastController = player.RayCastController;
            originGravity = player.Rb.gravityScale;
        }
        override public void Tick ( ) {
            CheckCollision ( );
            Move ( );
            Jump ( );
            if (player.Rb.velocity.y <= 0f && !bWallSliding)
                player.Rb.gravityScale = originGravity * stats.FallGravityMultiplier;
            else if (bWallSliding && player.Rb.velocity.y <= 0f)
                player.Rb.gravityScale = originGravity * stats.WallSlidingGravityMultiplier;
            else
                player.Rb.gravityScale = originGravity;

        }
        override public void FixedTick ( ) { }

        void Move ( ) {
            Vector2 nVel = player.Rb.velocity;
            if (rayCastController.Down) {
                nVel.x = inputValue.x * stats.NormalVel * Time.deltaTime;
            }
            else {
                nVel.x += inputValue.x * stats.AirVel * Time.deltaTime;
                nVel.x = Mathf.Clamp (nVel.x, -stats.NormalVel * Time.deltaTime, stats.NormalVel * Time.deltaTime);
                nVel.x = Mathf.SmoothDamp (player.Rb.velocity.x, nVel.x, ref velocityXSmoothing, smoothTime);
            }
            player.Rb.velocity = nVel;
        }

        void Jump ( ) {
            if (bCanJump && bJumpPressed) {
                Vector2 vel = player.Rb.velocity;
                //Wall Jump
                if (bWallSliding && !rayCastController.Down) {
                    EHitDirection wallDirection = rayCastController.Left?EHitDirection.LEFT : EHitDirection.RIGHT;
                    // from left wall to right wall
                    if (inputValue.x > 0f && wallDirection == EHitDirection.LEFT)
                        vel.x = stats.WallJumpVel;
                    // from right wall to left wall
                    else if (inputValue.x < 0f && wallDirection == EHitDirection.RIGHT)
                        vel.x = -stats.WallJumpVel;
                    else
                        return;
                }
                vel.y = stats.JumpVel;
                player.Rb.velocity = vel;
                bCanJump = false;
            }
        }

        void CheckCollision ( ) {
            bCanJump = false;
            bWallSliding = false;
            if ((rayCastController.Left || rayCastController.Right) && !rayCastController.Down) {
                bWallSliding = true;
                bCanJump = true;
            }
            if (rayCastController.Down)
                bCanJump = true;
        }

        override public void OnEnable ( ) {
            control.GamePlay.Move.performed += OnMoveValueChanged;
            control.GamePlay.Move.canceled += OnMoveValueReleased;
            control.GamePlay.Jump.started += OnJumpPressed;
            control.GamePlay.Jump.canceled += OnJumpReleased;
        }
        override public void OnDisable ( ) {
            control.GamePlay.Move.performed -= OnMoveValueChanged;
            control.GamePlay.Move.canceled -= OnMoveValueReleased;
            control.GamePlay.Jump.started -= OnJumpPressed;
            control.GamePlay.Jump.canceled -= OnJumpReleased;
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
    }

    [System.Serializable]
    public class MovementStats : PlayerStats {
        public float NormalVel = 250f;
        public float AirVel = 100f;
        public float JumpVel = 15f;
        public float WallJumpVel = 7.5f;
        public float FallGravityMultiplier = 1.5f;
        public float WallSlidingGravityMultiplier = 0.02f;
    }
}
