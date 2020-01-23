namespace Dash.Player {
    using System.Collections.Generic;

    using Eccentric.Utils;

    using UnityEngine;
    public class Player : MonoBehaviour {
#if UNITY_EDITOR
        // this is for monitor value change
        [ReadOnly, SerializeField] Movement tmp = null;
#endif
        Rigidbody2D rb = null;
        PlayerControl playerControl = null;
        [SerializeField] RayCastController rayCastController = null;
        List<PlayerComponent> components = new List<PlayerComponent> ( );
        [SerializeField] MovementStats movementStats = null;
        public PlayerControl PlayerControl => playerControl;
        public RayCastController RayCastController => rayCastController;
        public Rigidbody2D Rb => rb;
        void Awake ( ) {
            playerControl = new PlayerControl ( );
            rb = GetComponent<Rigidbody2D> ( );
            components.Add (new Movement (this, movementStats));
#if UNITY_EDITOR
            tmp = components [0] as Movement;
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

    }

}
