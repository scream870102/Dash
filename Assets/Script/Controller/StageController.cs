namespace CJStudio.Dash {
    using Eccentric;
    using P = Player;
    using System.Collections.Generic;
    using UnityEngine;
    class StageController {
        GameController gameController;
        Stage currentStage = null;
        SaveData playerData = null;
        P.Player player = null;
        public StageController (Stage initStage, GameController gameController) {
            this.gameController = gameController;
            DomainEvents.Register<OnStageReset> (OnStageReset);
            DomainEvents.Register<P.OnPlayerDead> (OnPlayerDead);
            currentStage = initStage;
            player = GameManager.Instance.Player;
            playerData = new SaveData ( );
        }

        ~StageController ( ) {
            DomainEvents.UnRegister<OnStageReset> (OnStageReset);
            DomainEvents.UnRegister<P.OnPlayerDead> (OnPlayerDead);
        }

        void OnStageReset (OnStageReset e) {
            LoadSavePoint ( );
        }
        void OnPlayerDead (P.OnPlayerDead e) {
            SetSavePoint (currentStage, player);
        }
        public void SetSavePoint (Stage stage, P.Player player) {
            DomainEvents.Raise<OnStageChange> (new OnStageChange (stage, currentStage));
            currentStage = stage;
            //get player data
            this.player = player;
            playerData = new SaveData (player.Dash.Energy, player.Dash.CanDash, stage, gameController.ElapsedTime);
            SLController.Save (playerData);
        }

        public void SetStage (Vector3 pos, float energy, bool canUseDash, float elapsedTime) {
            player.Tf.position = pos;
            Stage[ ] stageList = GameObject.FindObjectsOfType<Stage> ( );
            foreach (Stage o in stageList) {
                if (o.transform.position == pos) {
                    currentStage = o;
                    break;
                }
            }
            gameController.ElapsedTime = elapsedTime;
            playerData = new SaveData (energy, canUseDash, currentStage, gameController.ElapsedTime);
            DomainEvents.Raise<OnStageReset> (new OnStageReset ( ));
        }

        void LoadSavePoint ( ) {
            if (currentStage) {
                currentStage.EnableStage ( );
                player.SetSaveData (playerData);
                player.Tf.position = currentStage.StagePosition;
            }
        }
    }

    [System.Serializable]
    class SaveData {
        public Stage Stage;
        public ELevel Level;
        public float EnergyRemain;
        public bool CanUseDash;
        public Vector2 Pos;
        public float ElapsedTime;
        public SaveData (float energyRemain = 0f, bool canUseDash = false, Stage stage = null, float elapsedTime = 0f) {
            this.EnergyRemain = energyRemain;
            this.CanUseDash = canUseDash;
            this.Stage = stage;
            if (stage != null) {
                this.Level = stage.Level;
                this.Pos = stage.transform.position;
                this.ElapsedTime = elapsedTime;
            }
        }
    }

    class OnStageReset : IDomainEvent {
        public OnStageReset ( ) { }
    }
    class OnStageChange : IDomainEvent {
        Stage activeStage;
        Stage prevStage;
        public Stage ActiveStage => activeStage;
        public Stage PrevStage => prevStage;
        public OnStageChange (Stage stage, Stage prevStage) {
            this.activeStage = stage;
            this.prevStage = prevStage;
        }
    }
}