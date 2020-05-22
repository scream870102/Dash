namespace CJStudio.Dash {
    using System.Collections.Generic;
    using System;
    using Camera;
    using Eccentric.Utils;
    using Eccentric;
    using Firebase.Firestore;
    using Player;
    using UnityEngine.InputSystem;
    using UnityEngine;

    class GameController : MonoBehaviour {
        public StageController StageController => stageController;
        public CameraController CameraController => cameraController;
        public event Action GameEnded = null;
        public event Action<float> GoalReached = null;
        public event Action<float> ElapsedTimeChange = null;
        public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
        StageController stageController = null;
        [SerializeField] CameraController cameraController = null;
        float elapsedTime = 0f;
        [SerializeField] Stage initStage = null;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        EGameState state = EGameState.PLAY;
        PlayerControl Control => GameManager.Instance == null?null : GameManager.Instance.Control;
        ELevel nextLevel = ELevel.TITLE;
        void Awake ( ) {
            stageController = new StageController (initStage, this);
            DomainEvents.Raise (new OnGameStarted ( ));
            state = EGameState.PLAY;

        }

        void Update ( ) {
            if (state == EGameState.PLAY)
                elapsedTime += Time.deltaTime;
            if (ElapsedTimeChange != null)
                ElapsedTimeChange (elapsedTime);
        }

        public void MinusElapsedTime (float time) {
            elapsedTime -= time;
            if (elapsedTime < 0f) elapsedTime = 0f;
        }

        void OnEnable ( ) {
            if (GameManager.Instance.GameController != this)
                GameManager.Instance.GameController = this;
            DomainEvents.Register<OnGoalReached> (OnGoalReached);
            DomainEvents.Register<OnPlayerDead> (OnPlayerDead);
            Control.UI.Confirm.started += OnConfirmPressed;
            Control.UI.Cancel.started += OnCancelPressed;
            Control.UI.Option.started += OnOptionPressed;
            Control.GamePlay.Option.started += OnOptionPressed;
        }
        void OnDisable ( ) {
            DomainEvents.UnRegister<OnGoalReached> (OnGoalReached);
            DomainEvents.UnRegister<OnPlayerDead> (OnPlayerDead);
            if (Control != null) {
                Control.UI.Confirm.started -= OnConfirmPressed;
                Control.UI.Cancel.started -= OnCancelPressed;
                Control.UI.Option.started -= OnOptionPressed;
                Control.GamePlay.Option.started -= OnOptionPressed;
                Control.Disable ( );
            }
        }

        void OnGoalReached (OnGoalReached e) {
            if (GoalReached != null)
                GoalReached (elapsedTime);
            //抵達Level終點
#region  SCORE
            CollectionReference colRef = GameManager.Instance.Db.Collection ("ScoreBoard");
            Dictionary<string, object> user = new Dictionary<string, object> { { "Time", System.DateTime.Now.ToShortDateString ( ) + " " + System.DateTime.Now.ToShortTimeString ( ) },
                { "Score", elapsedTime },
                { "User", SLController.GetUserName ( ) },
                { "Level", "Level " + (int) (GameManager.Instance.CurrentLevel) }
            };
            colRef.Document ( ).SetAsync (user).ContinueWith (t => {
                Debug.Log ("Finish add data to firebase");
            });
#endregion
            state = EGameState.COUNT;
            nextLevel = e.NextLevel;
            Control.Disable ( );
            Control.UI.Enable ( );
        }

        void OnPlayerDead (OnPlayerDead e) {
            Control.Disable ( );
            Control.UI.Enable ( );
            state = EGameState.DIE;
        }
        void OnConfirmPressed (InputAction.CallbackContext ctx) {
            if (state == EGameState.COUNT) {
                GameManager.Instance.LoadLevel (nextLevel);
            }
            else if (state == EGameState.DIE) {
                DomainEvents.Raise<OnStageReset> (new OnStageReset ( ));
                Control.Disable ( );
                Control.GamePlay.Enable ( );
                state = EGameState.PLAY;
            }
            else if (state == EGameState.PAUSE) {
                DomainEvents.Raise<OnGameResumed> (new OnGameResumed ( ));
                Time.timeScale = 1f;
                state = EGameState.PLAY;
                DomainEvents.Raise<OnGameResumed> (new OnGameResumed ( ));
                Control.Disable ( );
                Control.GamePlay.Enable ( );
            }
        }

        void OnCancelPressed (InputAction.CallbackContext ctx) {
            if (state == EGameState.COUNT) {
                if (GameEnded != null)
                    GameEnded ( );
            }
        }

        void OnOptionPressed (InputAction.CallbackContext ctx) {
            if (state == EGameState.PLAY) {
                DomainEvents.Raise<OnGamePaused> (new OnGamePaused ( ));
                Time.timeScale = 0f;
                state = EGameState.PAUSE;
                Control.Disable ( );
                Control.UI.Enable ( );
            }
            else if (state == EGameState.DIE) {
                GameManager.Instance.LoadLevel (ELevel.TITLE);
            }
        }
    }
    class OnGoalReached : IDomainEvent {
        ELevel nextLevel = ELevel.TITLE;
        public ELevel NextLevel => nextLevel;
        public OnGoalReached (ELevel nextLevel = ELevel.TITLE) {
            this.nextLevel = nextLevel;
        }
    }
    class OnGameStarted : IDomainEvent {
        public OnGameStarted ( ) { }
    }

    class OnGamePaused : IDomainEvent {
        public OnGamePaused ( ) { }
    }
    class OnGameResumed : IDomainEvent {
        public OnGameResumed ( ) { }
    }

    [System.Serializable]
    enum EGameState {
        PLAY,
        DIE,
        COUNT,
        PAUSE,

    }
}