namespace CJStudio.Dash.MapObject {
    using UnityEngine;
    public class AMapObject : MonoBehaviour {
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
