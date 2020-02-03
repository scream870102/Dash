namespace CJStudio.Dash {
    using System.Collections.Generic;
    using P = Player;
    using MapObject;

    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    public class Stage : MonoBehaviour {
        [SerializeField] GameObject recoverObjectParent = null;
        [SerializeField] bool CanRecoverSeveral = false;
        bool bRecovered = false;
        public GameObject RecoverObjectParent => recoverObjectParent;
        public List<AMapObject> stageObjects = new List<AMapObject> ( );
        public Vector2 StagePosition => transform.position;
        void Awake ( ) {
            this.gameObject.tag = "Stage";
            GetComponent<Collider2D> ( ).isTrigger = true;
        }
        public void EnableStage ( ) {
            if (CanRecoverSeveral) {
                foreach (AMapObject o in stageObjects)
                    o.Init ( );
            }
            else if (!bRecovered) {
                foreach (AMapObject o in stageObjects)
                    o.Init ( );
            }
            bRecovered = true;
        }


        void OnTriggerEnter2D (Collider2D other) {
            if (other.gameObject.tag == "Player") {
                GameManager.Instance.Player.GameController.StageController.SetSavePoint (this, other.GetComponent<P.Player> ( ));
            }
        }
    }

}
