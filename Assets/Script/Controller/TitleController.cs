namespace CJStudio.Dash {
    using System.Collections.Generic;
    using System.Collections;
    using Eccentric.Utils;
    using UnityEngine.InputSystem;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using UnityEngine;
    class TitleController : MonoBehaviour {
        [SerializeField] ConsoleButton buttons;
        [SerializeField] Color activeColor;
        [SerializeField] Color deactiveColor;
        [SerializeField] GameObject tutorialObject = null;
        [SerializeField] Text continueButton = null;
        PlayerControl Control => GameManager.Instance == null?null : GameManager.Instance.Control;
        void Awake ( ) { }
        void Start ( ) {
            if (SLController.Load ( ) == null)
                continueButton.gameObject.SetActive (false);
        }
        void OnEnable ( ) {
            buttons.ActiveOption += OnActiveOption;
            buttons.DeactiveOption += OnDeactiveOption;
            buttons.CertainAction += OnActionCertain;
            buttons.Init ( );
            if (Control != null) {
                Control.UI.Choose.started += OnAxisValueChanged;
                Control.UI.Confirm.started += OnConfirmPressed;
                Control.UI.Cancel.started += OnCancelPressed;
                Control.Disable ( );
                Control.UI.Enable ( );
            }
            tutorialObject.SetActive (false);
        }

        void OnDisable ( ) {
            buttons.ActiveOption -= OnActiveOption;
            buttons.DeactiveOption -= OnDeactiveOption;
            buttons.CertainAction -= OnActionCertain;
            if (Control != null) {
                Control.UI.Choose.started -= OnAxisValueChanged;
                Control.UI.Confirm.started -= OnConfirmPressed;
                Control.UI.Cancel.started -= OnCancelPressed;
                Control.Disable ( );
            }
        }

        public void OnNewGamePressed ( ) {
            GameManager.Instance.LoadLevel (ELevel.LEVEL1);
        }
        public void OnContinuePressed ( ) {
            SaveData data = SLController.Load ( );
            GameManager.Instance.LoadLevel (data.Level, data);
        }
        public void OnTutorialPressed ( ) {
            tutorialObject.SetActive (true);
        }

        public void OnExitPressed ( ) {
            Application.Quit ( );
        }

        void OnActiveOption (Text text) {
            text.color = activeColor;
        }

        void OnDeactiveOption (Text text) {
            text.color = deactiveColor;
        }
        void OnActionCertain ( ) { }

        void OnAxisValueChanged (InputAction.CallbackContext ctx) {
            float value = ctx.ReadValue<System.Single> ( );
            if (value < 0)
                buttons.PlusIndex ( );
            else if (value > 0)
                buttons.MinusIndex ( );
        }

        void OnConfirmPressed (InputAction.CallbackContext ctx) {
            if (!tutorialObject.activeSelf)
                buttons.Invoke ( );
        }
        void OnCancelPressed (InputAction.CallbackContext ctx) {
            if (tutorialObject.activeSelf)
                tutorialObject.SetActive (false);
        }
    }

}