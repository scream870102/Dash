namespace CJStudio.Dash.MapObject {
    using System.Collections.Generic;
    using P = CJStudio.Dash.Player;
    using Eccentric;

    using UnityEngine;
    class Spring : AMapObject {
        [SerializeField] Vector2 force = Vector2.zero;
        [SerializeField] ESpringDirection direction = ESpringDirection.UP;
        Animator animator = null;
        override protected void Start ( ) {
            base.Start ( );
            animator = GetComponent<Animator> ( );
        }
        override protected void TriggerEnter (Collider2D other) {
            if (other.gameObject.tag == "Player") {
                P.Player p = GameManager.Instance.Player;
                if (p.IsDashing)p.ForceStopDash ( );
                if (direction == ESpringDirection.UP)
                    p.AddVertVelocity (force.y, true);
                else if (direction == ESpringDirection.DOWN)
                    p.AddVertVelocity (force.y, true);
                else if (direction == ESpringDirection.LEFT)
                    p.AddHoriVelocity (force.x);
                else if (direction == ESpringDirection.RIGHT)
                    p.AddHoriVelocity (force.x);
                animator.SetTrigger ("Pop");
                DomainEvents.Raise<OnRingVFX> (new OnRingVFX (transform.position, direction));
            }
        }
    }
    enum ESpringDirection {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }

}
