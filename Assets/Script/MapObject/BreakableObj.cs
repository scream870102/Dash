namespace CJStudio.Dash.MapObject {
    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    class BreakableObj : AMapObject {
        override protected void Awake ( ) {
            base.Awake ( );
            gameObject.tag = "Breakable";
        }
        public void Break ( ) {
            SetActive (false);
        }
    }

}
