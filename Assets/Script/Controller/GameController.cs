namespace CJStudio.Dash {
    using System;

    using Camera;

    using Eccentric.Utils;
    using Eccentric;

    using UnityEngine.InputSystem;
    using UnityEngine;

    class GameController : MonoBehaviour {
        public StageController StageController => stageController;
        public CameraController CameraController => cameraController;
        public event Action GameEnded = null;
        public event Action<float> GoalReached = null;
        public event Action<float> ElapsedTimeChange = null;
        StageController stageController = null;
        [SerializeField] CameraController cameraController = null;
        float elapsedTime = 0f;
        [SerializeField] Stage initStage = null;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        EGameState state = EGameState.PLAYING;
        PlayerControl Control => GameManager.Instance == null?null : GameManager.Instance.Control;
        void Awake ( ) {
            stageController = new StageController (initStage);
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
            Control.UI.Confirm.started += OnConfirmPressed;
            Control.UI.Cancel.started += OnCancelPressed;
        }
        void OnDisable ( ) {
            DomainEvents.UnRegister<OnGoalReached> (OnGoalReached);
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
            Control.Disable ( );
            Control.UI.Enable ( );
        }
        void OnConfirmPressed (InputAction.CallbackContext ctx) {
            if (state == EGameState.WAITING)
                GameManager.Instance.LoadScene ("GameScene");
        }

        void OnCancelPressed (InputAction.CallbackContext ctx) {
            if (state == EGameState.WAITING) {
                if (GameEnded != null)
                    GameEnded ( );
            }
        }
    }
    class OnGoalReached : IDomainEvent {
        public OnGoalReached ( ) { }
    }
    class OnGameStarted : IDomainEvent {
        public OnGameStarted ( ) { }
    }

    [System.Serializable]
    enum EGameState {
        PLAYING,
        WAITING,
        ENDED,

    }
}
