namespace CJStudio.Dash.Player {
    using System.Collections.Generic;

    using Eccentric.Utils;

    using UnityEngine;
    class Player : MonoBehaviour {
#if UNITY_EDITOR
        // this is for monitor value change
        [ReadOnly, SerializeField] Dash tmp = null;
#endif
        Rigidbody2D rb = null;
        Transform tf = null;
        PlayerControl playerControl = null;
        [SerializeField] RayCastController rayCastController = null;
        List<PlayerComponent> components = new List<PlayerComponent> ( );
        [SerializeField] MovementStats movementStats = null;
        [SerializeField] DashStats dashStats = null;
        public PlayerControl PlayerControl => playerControl;
        public RayCastController RayCastController => rayCastController;
        public Rigidbody2D Rb => rb;
        public Transform Tf => tf;
        public Dash Dash => components [1] as Dash;
        void Awake ( ) {
            playerControl = new PlayerControl ( );
            rb = GetComponent<Rigidbody2D> ( );
            tf = this.transform;
            components.Add (new Movement (this, movementStats));
            components.Add (new Dash (this, dashStats));
#if UNITY_EDITOR
            tmp = components [1] as Dash;
#endif

        }
        void OnEnable ( ) {
            playerControl.GamePlay.Enable ( );
            foreach (PlayerComponent o in components)
                o.OnEnable ( );
        }

        void OnDisable ( ) {
            playerControl.GamePlay.Disable ( );
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

    }

}
