namespace CJStudio.Dash.MapObject {
    using Eccentric.Utils;

    using UnityEngine.Events;
    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    class Launcher : AMapObject {
        bool bActive = false;
        public UnityEvent LaunchEvent = new UnityEvent ( );
        override protected void TriggerEnter (Collider2D other) {
            if (!bActive && other.gameObject.tag == "Player") {
                bActive = true;
                LaunchEvent.Invoke ( );
            }
        }

        override protected void CollisionEnter (Collision2D other) {
            if (!bActive && other.gameObject.tag == "Player") {
                bActive = true;
                LaunchEvent.Invoke ( );
            }
        }
        override public void Init ( ) {
            base.Init ( );
            bActive = false;
        }

    }

}
