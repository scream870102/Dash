namespace CJStudio.Dash.MapObject {
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UnityEngine;
    class OneTouchObj : AMapObject {
        [SerializeField] float remainTime = .5f;
        [SerializeField] ETouchDirection direction = ETouchDirection.UP;
        Animation anim = null;
        SpriteRenderer rend = null;
        float animLength = 0f;
        override protected void Start ( ) {
            base.Start ( );
            anim = GetComponent<Animation> ( );
            rend = GetComponent<SpriteRenderer> ( );
            animLength = anim.clip.length;
        }
        override protected void CollisionEnter (Collision2D other) {
            if (other.gameObject.tag == "Player") {
                if (direction == ETouchDirection.ALL) {
                    DisableObj ( );
                    return;
                }
                List<ContactPoint2D> contacts = new List<ContactPoint2D> ( );
                col.GetContacts (contacts);
                foreach (ContactPoint2D o in contacts) {
                    if (direction == ETouchDirection.UP && o.normal.y == -1f && o.collider.gameObject.tag == "Player") {
                        DisableObj ( );
                        return;
                    }
                    else if (direction == ETouchDirection.DOWN && o.normal.y == 1f && o.collider.gameObject.tag == "Player") {
                        DisableObj ( );
                        return;
                    }
                    else if (direction == ETouchDirection.LEFT && o.normal.x == 1f && o.collider.gameObject.tag == "Player") {
                        DisableObj ( );
                        return;
                    }
                    else if (direction == ETouchDirection.RIGHT && o.normal.x == -1f && o.collider.gameObject.tag == "Player") {
                        DisableObj ( );
                        return;
                    }
                }
            }
        }

        async void DisableObj ( ) {
            AnimPlay ( );
            await Task.Delay ((int)(remainTime * 1000));
            SetActive (false);
        }

        async void AnimPlay ( ) {
            await Task.Delay ((int)(remainTime - animLength) * 1000);
            anim.Play ( );
        }

        override public void Init ( ) {
            base.Init ( );
            rend.color = Color.white;
        }
        enum ETouchDirection {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            ALL,
        }
    }

}
