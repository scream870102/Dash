namespace CJStudio.Dash {
    using Eccentric.Utils;

    using UnityEngine;
    class GameManager : TSingletonMonoBehavior<GameManager> {
        StageController stageController = null;
        public StageController StageController => stageController;
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
            stageController = new StageController ( );
        }

    }

}
