namespace CJStudio.Dash {
    using Eccentric;
    using P = Player;
    class StageController {
        Stage currentStage = null;
        SaveData playerData = null;
        P.Player player = null;
        public StageController (Stage initStage) {
            DomainEvents.Register<OnStageReset> (OnStageReset);
            currentStage = initStage;
            player = GameManager.Instance.Player;
            playerData = new SaveData (GameManager.Instance.Player.Dash.Energy);
        }

        ~StageController ( ) {
            DomainEvents.UnRegister<OnStageReset> (OnStageReset);
        }

        void OnStageReset (OnStageReset e) {
            LoadSavePoint ( );
        }

        public void SetSavePoint (Stage stage, P.Player player) {
            currentStage = stage;
            //get player data
            this.player = player;
            playerData = new SaveData (player.Dash.Energy, player.Dash.CanDash);

        }
        void LoadSavePoint ( ) {
            if (currentStage) {
                currentStage.EnableStage ( );
                //set player data
                player.SetSaveData (playerData);
                player.Tf.position = currentStage.StagePosition;
            }
        }
    }

    class SaveData {
        public float EnergyRemain { get; private set; }
        public bool CanUseDash { get; private set; }
        public SaveData (float energyRemain = 0f, bool canUseDash = false) {
            this.EnergyRemain = energyRemain;
            this.CanUseDash = canUseDash;
        }
    }

    class OnStageReset : IDomainEvent {
        public OnStageReset ( ) { }
    }
}
