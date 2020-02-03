namespace CJStudio.Dash.MapObject {
    using Eccentric.Utils;

    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    class Platform : AMapObject {
        [SerializeField] Vector2 moveRange = Vector2.zero;
        [SerializeField] float time = 0f;
        [SerializeField] LayerMask passengerLayer = 0;
        Vector2 initPos = Vector2.zero;
        Collider2D col = null;
        Vector2 velocity = Vector2.zero;
        ScaledTimer timer = null;
        bool bPlus = true;
        Transform passenger = null;
        void Awake ( ) {
            col = GetComponent<Collider2D> ( );
            col.isTrigger = false;
            initPos = transform.position;
            velocity = moveRange / time;
            gameObject.layer = LayerMask.NameToLayer ("Ground");
        }

        void Start ( ) {
            timer = new ScaledTimer (time);
            Init ( );
        }

        void Update ( ) {
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
        void OnCollisionEnter2D (Collision2D other) {
            if ((1 << other.gameObject.layer) == passengerLayer.value) {
                passenger = other.transform;
                passenger.SetParent (transform);
            }
        }
        void OnCollisionExit2D (Collision2D other) {
            if (other.transform == passenger) {
                passenger.SetParent (null);
                passenger = null;
            }

        }
        override public void Init ( ) {
            this.transform.position = initPos;
            bPlus = true;
            timer.Reset ( );
        }
    }
}
