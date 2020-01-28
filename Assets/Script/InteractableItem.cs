namespace CJStudio.Dash {
    using P = Player;
    using UnityEngine;
    [RequireComponent (typeof (Collider2D))]
    class InteractableItem : MonoBehaviour {
        [SerializeField] [Range (0f, 5f)] float energyToPlus = 1f;
        void OnTriggerEnter2D (Collider2D other) {
            if (other.gameObject.tag == "Player") {
                P.Player player = other.GetComponent<P.Player> ( );
                player.AddEnergy (energyToPlus);
                this.gameObject.SetActive (false);
            }
        }
    }
}
