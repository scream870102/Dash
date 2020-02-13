namespace CJStudio.Dash.MapObject {
    using Eccentric.Utils;

    using UnityEngine;
    class UnstableObj : AMapObject {
        [SerializeField] float stateDuration = 0f;
        [SerializeField] bool IsDisappearAtFirst = false;
        [SerializeField] AnimationClip InAnim = null;
        [SerializeField] AnimationClip OutAnim = null;
        Animation anim = null;
        ScaledTimer timer = null;
        SpriteRenderer rend = null;
        bool bDisappear = false;
        override protected void Start ( ) {
            base.Start ( );
            anim = GetComponent<Animation> ( );
            timer = new ScaledTimer (stateDuration, false);
            rend = GetComponent<SpriteRenderer> ( );
            bDisappear = IsDisappearAtFirst;
            rend.color = bDisappear?new Color (1f, 1f, 1f, .25f) : Color.white;
            col.enabled = !bDisappear;
        }
        override protected void Tick ( ) {
            if (Mathf.Abs (timer.Remain - anim.clip.length) < 0.01f) {
                anim.Play (bDisappear?InAnim.name : OutAnim.name);
            }
            if (timer.IsFinished) {
                timer.Reset ( );
                bDisappear = !bDisappear;
                col.enabled = !bDisappear;
            }
        }
        override public void Init ( ) {
            base.Init ( );
            anim.Stop ( );
            bDisappear = IsDisappearAtFirst;
            rend.color = bDisappear?new Color (1f, 1f, 1f, .25f) : Color.white;
            timer.Reset ( );
            col.enabled = !bDisappear;
        }

    }

}
