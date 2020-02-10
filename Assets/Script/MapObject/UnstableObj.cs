namespace CJStudio.Dash.MapObject {
    using Eccentric.Utils;

    using UnityEngine;
    class UnstableObj : AMapObject {
        [SerializeField] float stateDuration = 0f;
        [SerializeField] Color disableColor = Color.black;
        [SerializeField] bool IsDisappearAtFirst = false;
        ScaledTimer timer = null;
        SpriteRenderer rend = null;
        bool bDisappear = false;
        override protected void Start ( ) {
            base.Start ( );
            timer = new ScaledTimer (stateDuration);
            rend = GetComponent<SpriteRenderer> ( );
            bDisappear = IsDisappearAtFirst;
        }
        override protected void Tick ( ) {
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
        override public void Init ( ) {
            base.Init ( );
            bDisappear = IsDisappearAtFirst;
            timer.Reset ( );
        }
    }

}
