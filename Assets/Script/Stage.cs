namespace CJStudio.Dash {
    using System.Collections.Generic;
    using P = Player;
    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    public class Stage : MonoBehaviour {
        [SerializeField] GameObject recoverObjectParent = null;
        [SerializeField] bool CanRecoverSeveral = false;
        bool bRecovered = false;
        public GameObject RecoverObjectParent => recoverObjectParent;
        public List<GameObject> stageObjects = new List<GameObject> ( );
        public Vector2 StagePosition => transform.position;
        void Awake ( ) {
            this.gameObject.tag = "Stage";
            GetComponent<Collider2D> ( ).isTrigger = true;
        }
        public void EnableStage ( ) {
            if (CanRecoverSeveral) {
                foreach (GameObject o in stageObjects)
                    o.SetActive (true);
            }
            else if (!bRecovered) {
                foreach (GameObject o in stageObjects)
                    o.SetActive (true);
            }
            bRecovered = true;
        }
        public void DisableStage ( ) {
            foreach (GameObject o in stageObjects)
                o.SetActive (false);
        }

        void OnTriggerEnter2D (Collider2D other) {
            if (other.gameObject.tag == "Player") {
                GameManager.Instance.StageController.SetSavePoint (this, other.GetComponent<P.Player> ( ));
            }
        }
    }

}
