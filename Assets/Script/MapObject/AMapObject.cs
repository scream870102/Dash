namespace CJStudio.Dash.MapObject {
    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    public class AMapObject : MonoBehaviour {
        protected Collider2D col = null;
        virtual protected void Awake ( ) {
            col = GetComponent<Collider2D> ( );
            gameObject.layer = LayerMask.NameToLayer ("Ground");
        }
        public virtual void Init ( ) {
            SetActive (true);
        }
        protected virtual void SetActive (bool value) {
            if (!value) {
                for (int i = 0; i < transform.childCount; i++) {
                    transform.GetChild (i).SetParent (null);
                }
            }
            gameObject.SetActive (value);
        }
    }
}
