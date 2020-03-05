namespace CJStudio.Dash {
    using Eccentric;

    using P = Player;
    using Player;
    using UnityEngine.UI;
    using UnityEngine;

    class UIController : MonoBehaviour {
        P.Player player = null;
        [SerializeField] SpriteRenderer targetSprite = null;
        [SerializeField] SpriteRenderer arrowSprite = null;
        [SerializeField] GameObject dashUI = null;
        [SerializeField] RectTransform energyBar = null;
        [SerializeField] GameObject noobImage = null;
        [SerializeField] GameObject endObject = null;
        [SerializeField] Text elapsedTimeText = null;
        [SerializeField] Text timeText = null;
        [SerializeField] GameObject pauseObj = null;
        float energyBarMaxHeight = 0f;
        void Awake ( ) {
            if (energyBar)
                energyBarMaxHeight = energyBar.sizeDelta.y;
            player = GameManager.Instance.Player;
            noobImage.SetActive (false);
            endObject.SetActive (false);
            pauseObj.SetActive (false);

        }

        void OnDashPrepare (DashProps e) {
            if (!dashUI.activeSelf) {
                dashUI.SetActive (true);
            }
            dashUI.transform.position = e.Pos;
            float degree = Math.GetDegree (e.Direction);
            targetSprite.size = new Vector2 ((e.Distance + .99f), 1f);
            arrowSprite.size = new Vector2 (e.MaxDistance + 1f, 1f);
            dashUI.transform.rotation = Quaternion.Euler (0f, 0f, degree);
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

        void OnElapsedTimeChange (float elapsedTime) {
            elapsedTimeText.text = elapsedTime.ToString ("0.00");
        }

        void OnPlayerDead (OnPlayerDead e) {
            noobImage.SetActive (true);
        }

        void OnStageReset (OnStageReset e) {
            noobImage.SetActive (false);
        }

        void OnGamePaused (OnGamePaused e) {
            pauseObj.SetActive (true);
        }

        void OnGameResumed (OnGameResumed e) {
            pauseObj.SetActive (false);
        }

        void OnGoalReached (float time) {
            endObject.SetActive (true);
            timeText.text = time.ToString ("0.00");
        }

        void OnEnable ( ) {
            player.Dash.Aim += OnDashPrepare;
            player.Dash.AimEnded += OnDashEnded;
            player.Dash.EnergyChange += OnEnergyChange;
            DomainEvents.Register<OnPlayerDead> (OnPlayerDead);
            DomainEvents.Register<OnStageReset> (OnStageReset);
            DomainEvents.Register<OnGamePaused> (OnGamePaused);
            DomainEvents.Register<OnGameResumed> (OnGameResumed);
            GameController gameController = GameObject.FindObjectOfType<GameController> ( );
            if (gameController) {
                gameController.GoalReached += OnGoalReached;
                gameController.ElapsedTimeChange += OnElapsedTimeChange;
            }
        }

        void OnDisable ( ) {
            player.Dash.Aim -= OnDashPrepare;
            player.Dash.AimEnded -= OnDashEnded;
            player.Dash.EnergyChange -= OnEnergyChange;
            DomainEvents.UnRegister<OnPlayerDead> (OnPlayerDead);
            DomainEvents.UnRegister<OnStageReset> (OnStageReset);
            DomainEvents.UnRegister<OnGamePaused> (OnGamePaused);
            DomainEvents.UnRegister<OnGameResumed> (OnGameResumed);
            GameController gameController = GameObject.FindObjectOfType<GameController> ( );
            if (gameController) {
                gameController.GoalReached -= OnGoalReached;
                gameController.ElapsedTimeChange -= OnElapsedTimeChange;
            }
        }
    }
}