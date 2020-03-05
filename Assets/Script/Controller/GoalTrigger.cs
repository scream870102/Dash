namespace CJStudio.Dash {
    using Eccentric;
    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    class GoalTrigger : MonoBehaviour {
        [SerializeField] ELevel nextLevel = ELevel.TITLE;
        void Awake ( ) {
            GetComponent<Collider2D> ( ).isTrigger = true;
        }
        void OnTriggerEnter2D (Collider2D other) {
            if (other.gameObject.tag == "Player")
                DomainEvents.Raise (new OnGoalReached (nextLevel));
        }
    }

}