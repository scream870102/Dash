namespace CJStudio.Dash.Player {
    using System.Collections.Generic;

    using Eccentric;

    using UnityEngine.InputSystem;
    using UnityEngine.UI;
    using UnityEngine;
    class Player : MonoBehaviour {
        #region TEST
        public Text deltaText = null;
        #endregion
        List<PlayerComponent> components = new List<PlayerComponent> ( );
        Rigidbody2D rb = null;
        Transform tf = null;
        Animator anim = null;
        SpriteRenderer rend = null;
        TrailRenderer trail = null;
        ParticleSystem particle = null;
        GameController gameController = null;
        [SerializeField] RayCastController rayCastController = null;
        #region STATS
        [SerializeField] MovementStats movementStats = null;
        [SerializeField] DashStats dashStats = null;
        #endregion
        public RayCastController RayCastController => rayCastController;
        public Rigidbody2D Rb => rb;
        public Transform Tf => tf;
        public Animator Anim => anim;
        public SpriteRenderer Rend => rend;
        public TrailRenderer Trail => trail;
        public ParticleSystem Particle => particle;
        public Dash Dash => components [1] as Dash;
        public Movement Movement => components [0] as Movement;
        public PlayerControl Control => GameManager.Instance.Control;
        public bool IsDashing => Dash.IsDashing;
        public GameController GameController {
            get {
                if (gameController)return gameController;
                return gameController = GameObject.FindObjectOfType<GameController> ( )as GameController;
            }
        }
        void Awake ( ) {
            rb = GetComponent<Rigidbody2D> ( );
            anim = GetComponent<Animator> ( );
            rend = GetComponent<SpriteRenderer> ( );
            trail = GetComponentInChildren<TrailRenderer> ( );
            particle = GetComponentInChildren<ParticleSystem> ( );
            particle.Clear ( );
            particle.Stop ( );
            tf = this.transform;
            components.Add (new Movement (this, movementStats));
            components.Add (new Dash (this, dashStats));
        }

        void OnEnable ( ) {
            Control.UI.Confirm.performed += OnConfirmPressed;
            Control.UI.Cancel.performed += OnCancelPressed;
            Control.Disable ( );
            Control.GamePlay.Enable ( );
            foreach (PlayerComponent o in components)
                o.OnEnable ( );
        }

        void OnDisable ( ) {
            Control.UI.Confirm.performed -= OnConfirmPressed;
            Control.UI.Cancel.performed -= OnCancelPressed;
            Control.Disable ( );
            foreach (PlayerComponent o in components)
                o.OnDisable ( );
        }

        void Start ( ) {
            rayCastController.Init (GetComponent<BoxCollider2D> ( ));

        }

        void Update ( ) {
            rayCastController.Tick ( );
            foreach (PlayerComponent o in components)
                o.Tick ( );
        }

        void FixedUpdate ( ) {
            foreach (PlayerComponent o in components)
                o.FixedTick ( );
        }

        public void AddEnergy (float supplement) {
            Dash.AddEnergy (supplement);
        }
        void OnTriggerEnter2D (Collider2D other) {
            if (other.gameObject.layer == (int)LayerMask.NameToLayer ("DeadZone")) {
                Anim.SetTrigger ("die");
                Control.Disable ( );
            }
        }

        public void OnDieAnimFined ( ) {
            DomainEvents.Raise (new OnPlayerDead ( ));
            Control.Disable ( );
            Control.UI.Enable ( );
        }

        public void SetSaveData (SaveData data) {
            Movement.SetSaveData (data);
            Dash.SetSaveData (data);
            rb.velocity = Vector2.zero;
        }
        void OnConfirmPressed (InputAction.CallbackContext ctx) {
            Debug.Log ("Confirm");
            Control.Disable ( );
            Control.GamePlay.Enable ( );
            DomainEvents.Raise (new OnStageReset ( ));
        }
        void OnCancelPressed (InputAction.CallbackContext ctx) {
            Control.Disable ( );
            GameManager.Instance.LoadScene ("TitleScene");
        }

    }
    class OnPlayerDead : IDomainEvent {
        public OnPlayerDead ( ) { }
    }

}
