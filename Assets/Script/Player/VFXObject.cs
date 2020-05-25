namespace CJStudio.Dash.Player {
    using UnityEngine;

    [CreateAssetMenu (fileName = "New VFX", menuName = "FX/VFX")]
    class VFXObject : ScriptableObject {
        public EVFXAction action = EVFXAction.CHARGE;
        public EVFXType type = EVFXType.DUST;
        public GameObject root = null;

    }
}