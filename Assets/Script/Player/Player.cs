﻿namespace CJStudio.Dash.Player {
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
        //bool bDead = false;
        Rigidbody2D rb = null;
        Transform tf = null;
        Animator anim = null;
        SpriteRenderer rend = null;
        BoxCollider2D col = null;
        GameController gameController = null;
        [SerializeField] RayCastController rayCastController = null;
#region ATTR
        [Header ("Set attribution for all action")]
        [SerializeField] bool bRendInvert = false;
        [SerializeField] MovementAttr movementAttr = null;
        [SerializeField] DashAttr dashAttr = null;
        [SerializeField] FXRef fxRefs = null;
#endregion
        public Dictionary<EVFXAction, EVFXType> VFXAction { get; private set; }
        public RayCastController RayCastController => rayCastController;
        public Rigidbody2D Rb => rb;
        public Transform Tf => tf;
        public Animator Anim => anim;
        public SpriteRenderer Rend => rend;
        public BoxCollider2D Col => col;
        public Movement Movement => components[0] as Movement;
        public Dash Dash => components[1] as Dash;
        public FX FX => components[2] as FX;
        public PlayerControl Control => GameManager.Instance == null?null : GameManager.Instance.Control;
        public bool IsDashing => Dash.IsDashing;
        public bool IsFacingRight => Movement.IsFacingRight;
        public bool IsRendInvert => bRendInvert;
        public Collider2D PushObj { get; private set; }
        public GameController GameController {
            get {
                if (gameController) return gameController;
                return gameController = GameObject.FindObjectOfType<GameController> ( ) as GameController;
            }
        }
        void Awake ( ) {
            rb = GetComponent<Rigidbody2D> ( );
            anim = GetComponent<Animator> ( );
            rend = GetComponent<SpriteRenderer> ( );
            col = GetComponent<BoxCollider2D> ( );
            tf = this.transform;
            components.Add (new Movement (this, movementAttr));
            components.Add (new Dash (this, dashAttr));
            components.Add (new FX (this, fxRefs));
            VFXAction = new Dictionary<EVFXAction, EVFXType> ( );
            foreach (VFXObject a in fxRefs.VFXAction) {
                GameObject root = Instantiate (a.root, Tf);
                FX.VFXS[a.type].Init (root);
                VFXAction.Add (a.action, a.type);
            }

        }

        void OnEnable ( ) {
            Control.Disable ( );
            Control.GamePlay.Enable ( );
            foreach (PlayerComponent o in components)
                o.OnEnable ( );
        }

        void OnDisable ( ) {
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
            FX.PlayVFX (VFXAction[EVFXAction.HEAL]);
            FX.PlaySFX (ESFXType.HEAL);
        }
        void OnTriggerEnter2D (Collider2D other) {
            if (other.gameObject.layer == (int) LayerMask.NameToLayer ("DeadZone")) {
                Anim.SetTrigger ("die");
                Control.Disable ( );
            }
        }

        public void OnDieAnimFined ( ) {
            DomainEvents.Raise (new OnPlayerDead ( ));
            Anim.ResetTrigger ("die");
        }

        public void SetSaveData (SaveData data) {
            Movement.SetSaveData (data);
            Dash.SetSaveData (data);
            rb.velocity = Vector2.zero;

        }

        public void AddHoriVelocity (float vel, bool IsResetVel = false) {
            Movement.AddHoriVelocity (vel, IsResetVel);
        }

        public void AddVertVelocity (float vel, bool IsResetVel = false) {
            Movement.AddVertVelocity (vel, IsResetVel);
        }

        public void SetPushObj (Collider2D obj = null) {
            this.PushObj = obj;
            Anim.SetBool ("push", obj != null);
        }

        public void ForceStopDash (bool IsResetDash = false) {
            Dash.ForceStopDash (IsResetDash);
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

    class OnAiming : IDomainEvent {
        bool bStart;
        public bool IsStart => bStart;
        public OnAiming (bool IsStart) {
            this.bStart = IsStart;
        }
    }

}