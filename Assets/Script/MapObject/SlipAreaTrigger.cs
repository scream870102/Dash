namespace CJStudio.Dash.MapObject {
    using CJStudio.Dash.Player;

    using Eccentric;

    using UnityEngine;
    class SlipAreaTrigger : AMapObject {

        void OnCollisionEnter2D (Collision2D other) {
            if (other.gameObject.tag == "Player")
                DomainEvents.Raise<OnSlideAreaEnter> (new OnSlideAreaEnter (true));
        }
        void OnCollisionExit2D (Collision2D other) {
            if (other.gameObject.tag == "Player") {
                DomainEvents.Raise<OnSlideAreaEnter> (new OnSlideAreaEnter (false));
            }
        }
    }
}
