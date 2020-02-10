namespace CJStudio.Dash.MapObject {
    using CJStudio.Dash.Player;

    using Eccentric;

    using UnityEngine;
    class SlipAreaTrigger : AMapObject {

        override protected void CollisionEnter (Collision2D other) {
            if (other.gameObject.tag == "Player")
                DomainEvents.Raise<OnSlideAreaEnter> (new OnSlideAreaEnter (true));
        }
        override protected void CollisionExit (Collision2D other) {
            if (other.gameObject.tag == "Player") {
                DomainEvents.Raise<OnSlideAreaEnter> (new OnSlideAreaEnter (false));
            }
        }
    }
}
