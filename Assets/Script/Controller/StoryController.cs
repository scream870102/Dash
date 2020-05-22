namespace CJStudio.Dash {
    using UnityEngine.InputSystem;
    using UnityEngine.UI;
    using UnityEngine;

    class StoryController : MonoBehaviour {
        [SerializeField] Scrollbar scrollbar = null;
        [SerializeField] float scrollSpeed = .25f;
        PlayerControl Control => GameManager.Instance == null?null : GameManager.Instance.Control;
        bool bScrolling = false;
        float direction = 0f;

        //Use Update to update the scroll problem
        void Update ( ) {
            if (bScrolling) {
                scrollbar.value = Mathf.Clamp01 (scrollbar.value + direction * Time.deltaTime);
            }
        }

        void OnEnable ( ) {
            if (Control != null) {
                Control.UI.Choose.started += OnAxisValueStarted;
                Control.UI.Choose.canceled += OnAxisValueCanceled;
                Control.UI.Confirm.started += OnConfirmPressed;
                Control.Disable ( );
                Control.UI.Enable ( );
            }
        }

        void OnDisable ( ) {
            if (Control != null) {
                Control.UI.Choose.started -= OnAxisValueStarted;
                Control.UI.Choose.canceled -= OnAxisValueCanceled;
                Control.UI.Confirm.started -= OnConfirmPressed;
            }

        }

        void OnAxisValueStarted (InputAction.CallbackContext ctx) {
            bScrolling = true;
            direction = (ctx.ReadValue<System.Single> ( ) > 0f?1f: -1f) * scrollSpeed;
        }

        void OnAxisValueCanceled (InputAction.CallbackContext ctx) {
            bScrolling = false;
        }

        void OnConfirmPressed (InputAction.CallbackContext ctx) {
            GameManager.Instance.LoadLevel (ELevel.LEVEL1);
        }
    }

}