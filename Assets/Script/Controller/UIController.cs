namespace CJStudio.Dash {
    using System.Collections.Generic;

    using Eccentric.Utils;
    using Eccentric;

    using P = Player;
    using Player;

    using UnityEngine.UI;
    using UnityEngine;
    class UIController : TSingletonMonoBehavior<UIController> {
        P.Player player = null;
        [SerializeField] SpriteRenderer arrowSprite = null;
        [SerializeField] GameObject dashUI = null;
        [SerializeField] RectTransform energyBar = null;
        float energyBarMaxHeight = 0f;
        override protected void Awake ( ) {
            base.Awake ( );
            if (energyBar)
                energyBarMaxHeight = energyBar.sizeDelta.y;
            player = GameManager.Instance.Player;
        }

        void OnDashPrepare (DashProps e) {
            if (!dashUI.activeSelf) {
                dashUI.SetActive (true);
            }
            dashUI.transform.position = e.Pos;
            arrowSprite.size = new Vector2 (e.Charge / e.MaxCharge, 1f);
            dashUI.transform.rotation = Quaternion.Euler (0f, 0f, Math.GetDegree (e.Direction));
        }
        void OnDashEnded ( ) {
            if (dashUI.activeSelf)
                dashUI.SetActive (false);
        }

        void OnEnergyChange (DashProps e) {
            float ratio = e.EnergyRemain < 0f?0f : e.EnergyRemain;
            ratio = energyBarMaxHeight * ratio / e.MaxEnergy;
            energyBar.sizeDelta = new Vector2 (energyBar.sizeDelta.x, ratio);
        }

        void OnEnable ( ) {
            if (arrowSprite != null && player != null) {
                player.Dash.DashPrepare += OnDashPrepare;
                player.Dash.DashEnded += OnDashEnded;
                player.Dash.EnergyChange += OnEnergyChange;
            }
        }

        void OnDisable ( ) {
            if (arrowSprite != null && player != null) {
                player.Dash.DashPrepare -= OnDashPrepare;
                player.Dash.DashEnded -= OnDashEnded;
                player.Dash.EnergyChange -= OnEnergyChange;
            }
        }
    }
}
