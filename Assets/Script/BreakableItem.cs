namespace CJStudio.Dash {
    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    class BreakableItem : MonoBehaviour {
        void Awake ( ) {
            if (gameObject.tag != "Breakable")
                gameObject.tag = "Breakable";
        }
        public void Break ( ) {
            this.gameObject.SetActive (false);
        }
    }

}
