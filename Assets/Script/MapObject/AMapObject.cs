namespace CJStudio.Dash.MapObject {
    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    public class AMapObject : MonoBehaviour {
        bool bEnableAtFirst = false;
        protected Collider2D col = null;
        virtual protected void Awake ( ) {
            gameObject.layer = LayerMask.NameToLayer ("Ground");
            bEnableAtFirst = this.enabled;
            col = GetComponent<Collider2D> ( );
        }
        virtual protected void Start ( ) {
            col = GetComponent<Collider2D> ( );
        }

        public virtual void Init ( ) {
            this.enabled = bEnableAtFirst;
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
        void Update ( ) {
            if (this.enabled)
                Tick ( );
        }

        void OnCollisionEnter2D (Collision2D other) {
            if (this.enabled)
                CollisionEnter (other);

        }
        void OnCollisionStay2D (Collision2D other) {
            if (this.enabled)
                CollisionStay (other);
        }
        void OnCollisionExit2D (Collision2D other) {
            if (this.enabled)
                CollisionExit (other);
        }

        void OnTriggerEnter2D (Collider2D other) {
            if (this.enabled)
                TriggerEnter (other);
        }

        void OnTriggerStay2D (Collider2D other) {
            if (this.enabled)
                TriggerStay (other);
        }

        void OnTriggerExit2D (Collider2D other) {
            if (this.enabled)
                TriggerExit (other);
        }

        protected virtual void CollisionEnter (Collision2D other) { }
        protected virtual void CollisionStay (Collision2D other) { }
        protected virtual void CollisionExit (Collision2D other) { }
        protected virtual void TriggerEnter (Collider2D other) { }
        protected virtual void TriggerStay (Collider2D other) { }
        protected virtual void TriggerExit (Collider2D other) { }
        protected virtual void Tick ( ) { }
    }
}
