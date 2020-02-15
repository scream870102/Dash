namespace CJStudio.Dash.MapObject {
    using UnityEngine;
    class BreakableObj : AMapObject {
        Animator anim = null;
        override protected void Awake ( ) {
            base.Awake ( );
            gameObject.tag = "Breakable";
            anim = GetComponent<Animator> ( );
        }
        public void Break ( ) {
            anim.SetTrigger ("Break");
        }

        public void DisableObj ( ) {
            SetActive (false);
        }

    }

}
