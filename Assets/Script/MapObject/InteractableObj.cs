namespace CJStudio.Dash.MapObject {
    using P = Player;
    using UnityEngine;
    class InteractableObj : MonoBehaviour {
        [SerializeField] [Range (0f, 5f)] float energyToPlus = 1f;
        void OnTriggerEnter2D (Collider2D other) {
            if (other.gameObject.tag == "Player") {
                P.Player player = GameManager.Instance.Player;
                player.AddEnergy (energyToPlus);
                this.gameObject.SetActive (false);
            }
        }
    }
}
