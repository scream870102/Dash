namespace CJStudio.Dash {
    using System.Threading.Tasks;
    using Eccentric.Utils;
    using Eccentric;
    using Firebase.Firestore;
    using UnityEngine.SceneManagement;
    using UnityEngine;

    class GameManager : TSingletonMonoBehavior<GameManager> {
        ELevel currentLevel = ELevel.TITLE;
        public ELevel CurrentLevel => currentLevel;
        PlayerControl control = null;
        public PlayerControl Control => control;
        Player.Player player = null;
        public Player.Player Player {
            get {
                if (player == null)
                    player = GameObject.FindObjectOfType (typeof (Player.Player)) as Player.Player;
                return player;
            }
            private set { player = value; }
        }
        FirebaseFirestore db = null;
        public FirebaseFirestore Db => db;
        override protected void Awake ( ) {
            base.Awake ( );
            if (Instance == this) {
                control = new PlayerControl ( );
                SLController.WriteLog ("Start check firebase instance");
                var task = Firebase.FirebaseApp.CheckAndFixDependenciesAsync ( );
                task.Wait ( );
                Firebase.DependencyStatus obj = task.Result;
                if (obj == Firebase.DependencyStatus.Available) {
                    db = FirebaseFirestore.DefaultInstance;
                    SLController.WriteLog ("Finish check firebase instance");
                }
                else {
                    SLController.WriteLog (System.String.Format ("Could not resolve all Firebase dependencies: {0}", obj));
#if UNITY_EDITOR
                    Debug.LogError (System.String.Format ("Could not resolve all Firebase dependencies: {0}", obj));
#endif
                }
            }

        }
        void OnEnable ( ) {
            DomainEvents.Register<OnGameStarted> (OnGameStarted);
        }
        void OnDisable ( ) {
            DomainEvents.UnRegister<OnGameStarted> (OnGameStarted);
        }
        void OnGameStarted (OnGameStarted e) {
            GameObject.FindObjectOfType<GameController> ( ).GameEnded += OnGameEnded;
        }

        void OnGameEnded ( ) {
            GameObject.FindObjectOfType<GameController> ( ).GameEnded -= OnGameEnded;
            SceneManager.LoadScene ("TitleScene");
        }
        public async void LoadLevel (ELevel level, SaveData data = null) {
            currentLevel = level;
            AsyncOperation operation = SceneManager.LoadSceneAsync ((int) level, LoadSceneMode.Single);
            operation.allowSceneActivation = false;
            while (!operation.isDone) {
                if (operation.progress == .9f)
                    break;
                await Task.Delay (10);
            }
            operation.allowSceneActivation = true;
            while (!operation.isDone)
                await Task.Delay (10);
            if (data == null) return;
            GameController obj = GameObject.Find ("GameController").GetComponent<GameController> ( );
            if (obj != null)
                obj.StageController.SetStage (data.Pos, data.EnergyRemain, data.CanUseDash, data.ElapsedTime);
        }

    }

}