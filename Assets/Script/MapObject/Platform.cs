namespace CJStudio.Dash.MapObject {
    using Eccentric.Utils;

    using UnityEngine;
    class Platform : AMapObject {
        [SerializeField] Vector2 moveRange = Vector2.zero;
        [SerializeField] float time = 0f;
        [SerializeField] LayerMask passengerLayer = 0;
        Vector2 initPos = Vector2.zero;
        Vector2 velocity = Vector2.zero;
        ScaledTimer timer = null;
        bool bPlus = true;
        Transform passenger = null;
        override protected void Awake ( ) {
            base.Awake ( );
            col.isTrigger = false;
            initPos = transform.position;
            velocity = moveRange / time;
        }

        override protected void Start ( ) {
            timer = new ScaledTimer (time);
        }

        override protected void Tick ( ) {
            if (timer.IsFinished) {
                timer.Reset ( );
                bPlus = !bPlus;
            }
            if (bPlus) {
                transform.position += (Vector3)velocity * Time.deltaTime;
            }
            else {
                transform.position -= (Vector3)velocity * Time.deltaTime;
            }
        }

        override protected void CollisionEnter (Collision2D other) {
            if ((1 << other.gameObject.layer) == passengerLayer.value) {
                passenger = other.transform;
                passenger.SetParent (transform);
            }
        }
        override protected void CollisionExit (Collision2D other) {
            if (other.transform == passenger) {
                passenger.SetParent (null);
                passenger = null;
            }
        }
        void OnDisable ( ) {
            if (passenger && passenger.parent == this.transform) {
                passenger.SetParent (null);
            }
            passenger = null;
        }

        override public void Init ( ) {
            base.Init ( );
            this.transform.position = initPos;
            bPlus = true;
            timer.Reset ( );
        }
    }
}
