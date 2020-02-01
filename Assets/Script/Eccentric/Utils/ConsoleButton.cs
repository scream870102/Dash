namespace Eccentric.Utils {
    using System.Collections.Generic;
    using System;

    using UnityEngine.Events;
    using UnityEngine.UI;
    using UnityEngine;
    [System.Serializable]
    class ConsoleButton {
        [SerializeField] List<Button> buttons = new List<Button> ( );
        public List<Button> Buttons => buttons;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        int currentIndex;
#if UNITY_EDITOR
        [ReadOnly, SerializeField]
#endif
        int maxIndex;
        public int CurrentIndex => currentIndex;
        public event Action<Text> ActiveOption = null;
        public event Action<Text> DeactiveOption = null;
        public event Action CertainAction = null;
        public ConsoleButton (int initIndex = 0) {
            currentIndex = initIndex;
            maxIndex = buttons.Count - 1;
            CheckRange ( );
            InvokeEvent ( );
        }

        public void Init (int initIndex = 0) {
            currentIndex = initIndex;
            maxIndex = buttons.Count - 1;
            CheckRange ( );
            InvokeEvent ( );
        }
        public void Invoke (int index) {
            buttons [index].Action.Invoke ( );
            ActionPressed ( );
        }
        public void Invoke ( ) {
            buttons [CurrentIndex].Action.Invoke ( );
            ActionPressed ( );
        }
        public void PlusIndex ( ) {
            currentIndex++;
            CheckRange ( );
            InvokeEvent ( );
        }
        public void MinusIndex ( ) {
            currentIndex--;
            CheckRange ( );
            InvokeEvent ( );
        }

        void CheckRange ( ) {
            if (currentIndex > maxIndex)
                currentIndex = 0;
            else if (currentIndex < 0)
                currentIndex = maxIndex;
        }

        void InvokeEvent ( ) {
            for (int i = 0; i < buttons.Count; i++) {
                if (i == currentIndex) {
                    if (ActiveOption != null) {
                        ActiveOption (buttons [i].Text);
                    }
                }
                else {
                    if (DeactiveOption != null)
                        DeactiveOption (buttons [i].Text);
                }
            }
        }
        void ActionPressed ( ) {
            if (CertainAction != null)
                CertainAction ( );
        }
    }

    [System.Serializable]
    class Button {
        public Text Text;
        public UnityEvent Action;
    }
}
