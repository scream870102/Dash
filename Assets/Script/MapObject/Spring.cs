namespace CJStudio.Dash.MapObject {
    using System.Collections.Generic;
    using P = CJStudio.Dash.Player;
    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    class Spring : AMapObject {
        [SerializeField] Vector2 force = Vector2.zero;
        [SerializeField] ESpringDirection direction = ESpringDirection.UP;
        override protected void Awake ( ) {
            base.Awake ( );
        }

        void OnCollisionEnter2D (Collision2D other) {
            if (other.gameObject.tag == "Player") {
                List<ContactPoint2D> contacts = new List<ContactPoint2D> ( );
                col.GetContacts (contacts);
                foreach (ContactPoint2D o in contacts) {
                    P.Player p = o.collider.GetComponent<P.Player> ( );
                    if (p != null) {
                        if (direction == ESpringDirection.UP && o.normal.y == -1)
                            p.AddVertVelocity (force.y);
                        else if (direction == ESpringDirection.DOWN && o.normal.y == 1)
                            p.AddVertVelocity (force.y);
                        else if (direction == ESpringDirection.LEFT && o.normal.x == 1)
                            p.AddHoriVelocity (force.x);
                        else if (direction == ESpringDirection.RIGHT && o.normal.x == -1)
                            p.AddHoriVelocity (force.x);
                        break;
                    }

                }
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
