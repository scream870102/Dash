namespace CJStudio.Dash.Player {
    using System.Collections.Generic;

    using Eccentric;

    using UnityEngine.InputSystem;
    using UnityEngine;
    class Player : MonoBehaviour {
        List<PlayerComponent> components = new List<PlayerComponent> ( );
        Rigidbody2D rb = null;
        Transform tf = null;
        Animator anim = null;
        SpriteRenderer rend = null;
        PlayerControl playerControl = null;
        [SerializeField] RayCastController rayCastController = null;
        #region STATS
        [SerializeField] MovementStats movementStats = null;
        [SerializeField] DashStats dashStats = null;
        #endregion
        public PlayerControl PlayerControl => playerControl;
        public RayCastController RayCastController => rayCastController;
        public Rigidbody2D Rb => rb;
        public Transform Tf => tf;
        public Animator Anim => anim;
        public SpriteRenderer Rend => rend;
        public Dash Dash => components [1] as Dash;
        public Movement Movement => components [0] as Movement;
        public bool IsDashing => Dash.IsDashing;
        void Awake ( ) {
            playerControl = new PlayerControl ( );
            rb = GetComponent<Rigidbody2D> ( );
            anim = GetComponent<Animator> ( );
            rend = GetComponent<SpriteRenderer> ( );
            tf = this.transform;
            components.Add (new Movement (this, movementStats));
            components.Add (new Dash (this, dashStats));
        }

        void OnEnable ( ) {
            PlayerControl.UI.Certain.performed += OnCertainPressed;
            playerControl.UI.Disable ( );
            playerControl.GamePlay.Enable ( );
            foreach (PlayerComponent o in components)
                o.OnEnable ( );
        }

        void OnDisable ( ) {
            PlayerControl.UI.Certain.performed -= OnCertainPressed;
            playerControl.GamePlay.Disable ( );
            playerControl.UI.Disable ( );
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
            }
        }

        public void OnDieAnimFined ( ) {
            DomainEvents.Raise (new OnPlayerDead ( ));
            playerControl.GamePlay.Disable ( );
            playerControl.UI.Enable ( );
        }

        public void SetSaveData (SaveData data) {
            Movement.SetSaveData (data);
            Dash.SetSaveData (data);
        }
        void OnCertainPressed (InputAction.CallbackContext ctx) {
            playerControl.UI.Disable ( );
            playerControl.GamePlay.Enable ( );
            DomainEvents.Raise (new OnStageReset ( ));
        }

    }
    class OnPlayerDead : IDomainEvent {
        public OnPlayerDead ( ) { }
    }

}
