namespace CJStudio.Dash
{
    using System.Collections.Generic;

    using MapObject;
    using P = Player;
    using Eccentric;

    using UnityEngine;
    [RequireComponent(typeof(Collider2D))]
    public class Stage : MonoBehaviour
    {
        [SerializeField] GameObject recoverObjectParent = null;
        [SerializeField] bool CanRecoverSeveral = false;
        [SerializeField] Sprite activeSprite = null;
        Sprite deactiveSprite = null;
        SpriteRenderer rend = null;
        public GameObject RecoverObjectParent => recoverObjectParent;
        public List<AMapObject> stageObjects = new List<AMapObject>();
        public Vector2 StagePosition => transform.position;
        void Awake()
        {
            this.gameObject.tag = "Stage";
            GetComponent<Collider2D>().isTrigger = true;
            rend = GetComponent<SpriteRenderer>();
            deactiveSprite = rend.sprite;
        }
        void OnEnable()
        {
            DomainEvents.Register<OnStageChange>(OnStageChange);
        }
        void OnDisable()
        {
            DomainEvents.UnRegister<OnStageChange>(OnStageChange);
        }

        void OnStageChange(OnStageChange e)
        {
            if (e.ActiveStage == this)
                rend.sprite = activeSprite;
            else
                rend.sprite = deactiveSprite;
        }
        public void EnableStage()
        {
            if (CanRecoverSeveral)
            {
                foreach (AMapObject o in stageObjects)
                    o.Init();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                GameManager.Instance.Player.GameController.StageController.SetSavePoint(this, other.GetComponent<P.Player>());
            }
        }
    }

}
