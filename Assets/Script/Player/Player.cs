﻿namespace CJStudio.Dash.Player {
    using System.Collections.Generic;

    using Eccentric;

    using UnityEngine.InputSystem;
    using UnityEngine.UI;
    using UnityEngine;
    class Player : MonoBehaviour {
        #region TEST
        public Text deltaText = null;
        [SerializeField] Movement movement = null;
        #endregion
        List<PlayerComponent> components = new List<PlayerComponent> ( );
        bool bDead = false;
        Rigidbody2D rb = null;
        Transform tf = null;
        Animator anim = null;
        SpriteRenderer rend = null;
        TrailRenderer trail = null;
        BoxCollider2D col = null;
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
        public BoxCollider2D Col => col;
        public ParticleSystem Particle => particle;
        public Dash Dash => components [1] as Dash;
        public Movement Movement => components [0] as Movement;
        public PlayerControl Control => GameManager.Instance == null?null : GameManager.Instance.Control;
        public bool IsDashing => Dash.IsDashing;
        public Collider2D PushObj { get; private set; }
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
            col = GetComponent<BoxCollider2D> ( );
            particle.Clear ( );
            particle.Stop ( );
            tf = this.transform;
            components.Add (new Movement (this, movementStats));
            components.Add (new Dash (this, dashStats));
#if UNITY_EDITOR
            movement = components [0] as Movement;
#endif
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
            if (Control != null) {
                Control.UI.Confirm.performed -= OnConfirmPressed;
                Control.UI.Cancel.performed -= OnCancelPressed;
                Control.Disable ( );
            }
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
            bDead = true;
            Anim.ResetTrigger ("die");
        }

        public void SetSaveData (SaveData data) {
            Movement.SetSaveData (data);
            Dash.SetSaveData (data);
            rb.velocity = Vector2.zero;

        }
        void OnConfirmPressed (InputAction.CallbackContext ctx) {
            if (bDead) {
                Control.Disable ( );
                Control.GamePlay.Enable ( );
                DomainEvents.Raise (new OnStageReset ( ));
                bDead = false;
            }
        }
        void OnCancelPressed (InputAction.CallbackContext ctx) {
            Control.Disable ( );
            GameManager.Instance.LoadScene ("TitleScene");
        }

        public void AddHoriVelocity (float vel) {
            Movement.AddHoriVelocity (vel);
        }

        public void AddVertVelocity (float vel) {
            Movement.AddVertVelocity (vel);
        }

        public void SetPushObj (Collider2D obj = null) {
            this.PushObj = obj;
            Anim.SetBool ("push", obj != null);
        }

    }
    class OnPlayerDead : IDomainEvent {
        public OnPlayerDead ( ) { }
    }

    class OnSpaceAreaEnter : IDomainEvent {
        bool bEnter;
        public bool IsEnter => bEnter;
        public OnSpaceAreaEnter (bool IsEnter) {
            this.bEnter = IsEnter;
        }
    }
    class OnSlideAreaEnter : IDomainEvent {
        bool bEnter;
        public bool IsEnter => bEnter;
        public OnSlideAreaEnter (bool IsEnter) {
            this.bEnter = IsEnter;
        }
    }

}
