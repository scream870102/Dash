namespace CJStudio.Dash.MapObject {
    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    class BreakableObj : AMapObject {
        void Awake ( ) {
            gameObject.layer = LayerMask.NameToLayer ("Ground");
            gameObject.tag = "Breakable";
        }
        public void Break ( ) {
            SetActive (false);
        }
    }

}
