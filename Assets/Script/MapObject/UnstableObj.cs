﻿namespace CJStudio.Dash.MapObject {
    using Eccentric.Utils;

    using UnityEngine;
    class UnstableObj : AMapObject {
        [SerializeField] float stateDuration = 0f;
        [SerializeField] Color disableColor = Color.black;
        ScaledTimer timer = null;
        SpriteRenderer rend = null;
        bool bDisappear = false;
        void Start ( ) {
            timer = new ScaledTimer (stateDuration);
            rend = GetComponent<SpriteRenderer> ( );
        }
        void Update ( ) {
            if (timer.IsFinished) {
                timer.Reset ( );
                bDisappear = !bDisappear;
                if (bDisappear) {
                    col.enabled = false;
                    rend.color = disableColor;
                }
                else {
                    col.enabled = true;
                    rend.color = Color.white;
                }
            }
        }
    }

}