namespace CJStudio.Dash.MapObject {
    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    class BreakableObj : AMapObject {
        LayerMask layer = 0;
        void Awake ( ) {
            layer = LayerMask.NameToLayer ("BreakableItem");
            gameObject.layer = (int)layer;
            if (gameObject.tag != "Breakable")
                gameObject.tag = "Breakable";
        }
        public void Break ( ) {
            this.gameObject.SetActive (false);
        }
    }

}
