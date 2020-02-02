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
        PlayerControl Control => GameManager.Instance.Control;
        void Awake ( ) { }
        void OnEnable ( ) {
            buttons.ActiveOption += OnActiveOption;
            buttons.DeactiveOption += OnDeactiveOption;
            buttons.CertainAction += OnActionCertain;
            buttons.Init ( );
            Control.UI.Choose.started += OnAxisValueChanged;
            Control.UI.Confirm.started += OnConfirmPressed;
            Control.Disable ( );
            Control.UI.Enable ( );
            tutorialObject.SetActive (false);
        }

        void OnDisable ( ) {
            buttons.ActiveOption -= OnActiveOption;
            buttons.DeactiveOption -= OnDeactiveOption;
            buttons.CertainAction -= OnActionCertain;
            Control.UI.Choose.started -= OnAxisValueChanged;
            Control.UI.Confirm.started -= OnConfirmPressed;
            Control.Disable ( );
        }

        public void OnStartPressed ( ) {
            SceneManager.LoadScene ("GameScene");
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
            if (!tutorialObject.activeInHierarchy)
                buttons.Invoke ( );
            else
                tutorialObject.SetActive (false);
        }
    }

}
