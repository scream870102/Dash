namespace CJStudio.Dash.MapObject {
    using CJStudio.Dash.Player;

    using Eccentric;

    using UnityEngine;
    class SpaceAreaTrigger : AMapObject {
        protected override void Awake ( ) {
            base.Awake ( );
            col.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer ("Default");
        }
        override protected void TriggerEnter (Collider2D other) {
            if (other.gameObject.tag == "Player")
                DomainEvents.Raise<OnSpaceAreaEnter> (new OnSpaceAreaEnter (true));
        }

        override protected void TriggerExit (Collider2D other) {
            if (other.gameObject.tag == "Player") {
                DomainEvents.Raise<OnSpaceAreaEnter> (new OnSpaceAreaEnter (false));
            }

        }
    }

}
