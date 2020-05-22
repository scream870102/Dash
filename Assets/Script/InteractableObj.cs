namespace CJStudio.Dash {
    using P = Player;
    using UnityEngine;
    class InteractableObj : MonoBehaviour {
        [SerializeField][Range (0f, 5f)] float energyToPlus = 1f;
        void OnTriggerEnter2D (Collider2D other) {
            if (other.gameObject.tag == "Player") {
                if (GameManager.Instance.GameController != null)
                    GameManager.Instance.GameController.MinusElapsedTime (energyToPlus);
                P.Player player = GameManager.Instance.Player;
                player.AddEnergy (0f);
                this.gameObject.SetActive (false);
            }
        }
    }
}