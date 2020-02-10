namespace CJStudio.Dash.MapObject {
    using System.Collections.Generic;
    using P = CJStudio.Dash.Player;
    using UnityEngine;
    class Spring : AMapObject {
        [SerializeField] Vector2 force = Vector2.zero;
        [SerializeField] ESpringDirection direction = ESpringDirection.UP;

        override protected void CollisionEnter (Collision2D other) {
            if (other.gameObject.tag == "Player") {
                List<ContactPoint2D> contacts = new List<ContactPoint2D> ( );
                col.GetContacts (contacts);
                foreach (ContactPoint2D o in contacts) {
                    if (o.collider.gameObject.tag == "Player") {
                        P.Player p = GameManager.Instance.Player;
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
    }
    enum ESpringDirection {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }

}
