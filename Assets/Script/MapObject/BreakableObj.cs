namespace CJStudio.Dash.MapObject {
    using Eccentric;

    using UnityEngine;
    class BreakableObj : AMapObject {
        Animator anim = null;
        override protected void Awake ( ) {
            base.Awake ( );
            gameObject.tag = "Breakable";
            anim = GetComponent<Animator> ( );
        }
        public void Break ( ) {
            DomainEvents.Raise (new OnExplosionVFX (transform.position));
            anim.SetTrigger ("Break");
        }

        public void DisableObj ( ) {
            SetActive (false);
        }

    }

}
