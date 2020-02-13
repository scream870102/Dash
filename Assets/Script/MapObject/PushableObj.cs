namespace CJStudio.Dash.MapObject {
    using System.Collections.Generic;
    using UnityEngine;
    using P = CJStudio.Dash.Player;
    class PushableObj : AMapObject {
        Vector2 initPos;
        P.Player player = null;
        Rigidbody2D rb = null;

        override protected void Awake ( ) {
            base.Awake ( );
            initPos = this.transform.position;
            rb = GetComponent<Rigidbody2D> ( );
        }

        override public void Init ( ) {
            base.Init ( );
            this.transform.position = initPos;
            rb.constraints = (RigidbodyConstraints2D.FreezeRotation);
            if (player) {
                player.SetPushObj (null);
                player = null;
            }
        }

        override protected void CollisionEnter (Collision2D other) {
            if (other.gameObject.tag == "Player") {
                List<ContactPoint2D> contacts = new List<ContactPoint2D> ( );
                col.GetContacts (contacts);
                foreach (var o in contacts) {
                    if (o.normal.y == 0) {
                        player = GameManager.Instance.Player;
                        player.SetPushObj (this.col);
                    }
                }
            }

        }

        override protected void CollisionStay (Collision2D other) {
            if (other.gameObject.tag == "Player") {
                List<ContactPoint2D> contacts = new List<ContactPoint2D> ( );
                col.GetContacts (contacts);
                foreach (var o in contacts) {
                    if (o.normal.y == -1)
                        rb.constraints = (RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation);
                }
            }
        }

        override protected void CollisionExit (Collision2D other) {
            rb.constraints = (RigidbodyConstraints2D.FreezeRotation);
            if (player) {
                player.SetPushObj (null);
                player = null;
            }
        }
    }

}
