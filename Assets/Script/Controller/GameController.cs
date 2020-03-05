namespace CJStudio.Dash {
    using System;
    using Camera;
    using Eccentric.Utils;
    using Eccentric;
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
        EGameState state = EGameState.PLAYING;
        PlayerControl Control => GameManager.Instance == null?null : GameManager.Instance.Control;
        ELevel nextLevel = ELevel.TITLE;
        void Awake ( ) {
            stageController = new StageController (initStage, this);
            DomainEvents.Raise (new OnGameStarted ( ));
            state = EGameState.PLAYING;
        }
        void Update ( ) {
            elapsedTime += Time.deltaTime;
            if (ElapsedTimeChange != null)
                ElapsedTimeChange (elapsedTime);
        }
        void OnEnable ( ) {
            DomainEvents.Register<OnGoalReached> (OnGoalReached);
            DomainEvents.Register<OnPlayerDead> (OnPlayerDead);
            Control.UI.Confirm.started += OnConfirmPressed;
            Control.UI.Cancel.started += OnCancelPressed;
        }
        void OnDisable ( ) {
            DomainEvents.UnRegister<OnGoalReached> (OnGoalReached);
            DomainEvents.UnRegister<OnPlayerDead> (OnPlayerDead);
            if (Control != null) {
                Control.UI.Confirm.started -= OnConfirmPressed;
                Control.UI.Cancel.started -= OnCancelPressed;
                Control.Disable ( );
            }
        }

        void OnGoalReached (OnGoalReached e) {
            if (GoalReached != null)
                GoalReached (elapsedTime);
            state = EGameState.WAITING;
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
            if (state == EGameState.WAITING) {
                GameManager.Instance.LoadLevel (nextLevel);
            }
            else if (state == EGameState.DIE) {
                DomainEvents.Raise<OnStageReset> (new OnStageReset ( ));
                Control.Disable ( );
                Control.GamePlay.Enable ( );
                state = EGameState.PLAYING;
            }
        }

        void OnCancelPressed (InputAction.CallbackContext ctx) {
            if (state == EGameState.WAITING) {
                if (GameEnded != null)
                    GameEnded ( );
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

    [System.Serializable]
    enum EGameState {
        PLAYING,
        DIE,
        WAITING,
        ENDED,

    }
}