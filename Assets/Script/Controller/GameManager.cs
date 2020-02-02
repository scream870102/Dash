namespace CJStudio.Dash {
    using System;

    using Eccentric.Utils;
    using Eccentric;

    using UnityEngine.SceneManagement;
    using UnityEngine;
    class GameManager : TSingletonMonoBehavior<GameManager> {
        PlayerControl control = null;
        public PlayerControl Control => control;
        Player.Player player = null;
        public Player.Player Player {
            get {
                if (player == null)
                    player = GameObject.FindObjectOfType (typeof (Player.Player))as Player.Player;
                return player;
            }
            private set { player = value; }
        }
        override protected void Awake ( ) {
            base.Awake ( );
            control = new PlayerControl ( );
        }
        void OnEnable ( ) {
            DomainEvents.Register<OnGameStarted> (OnGameStarted);
        }
        void OnDisable ( ) {
            DomainEvents.UnRegister<OnGameStarted> (OnGameStarted);
        }
        void OnGameStarted (OnGameStarted e) {
            GameObject.FindObjectOfType<GameController> ( ).GameEnded += OnGameEnded;
        }

        void OnGameEnded ( ) {
            GameObject.FindObjectOfType<GameController> ( ).GameEnded -= OnGameEnded;
            SceneManager.LoadScene ("TitleScene");
        }
        public void LoadScene (string name) {
            SceneManager.LoadScene (name);
        }

    }

}
