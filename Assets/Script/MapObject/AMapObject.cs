namespace CJStudio.Dash.MapObject {
    using UnityEngine;
    public class AMapObject : MonoBehaviour {
        public virtual void Init ( ) {
            this.gameObject.SetActive (true);
        }
    }
}
