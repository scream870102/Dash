namespace CJStudio.Dash {
    using System.Collections.Generic;
    using Eccentric.Utils;
    using Firebase.Firestore;
    using UnityEngine.InputSystem;
    using UnityEngine.UI;
    using UnityEngine;
    class TitleController : MonoBehaviour {
        [SerializeField] ConsoleButton buttons;
        [SerializeField] Color activeColor;
        [SerializeField] Color deactiveColor;
        [SerializeField] GameObject tutorialObject = null;
        [SerializeField] GameObject scoreBoardObject = null;
        [SerializeField] Text continueButton = null;
        [SerializeField] Text loadText = null;
        [SerializeField] Text userText = null;
        [SerializeField] InputField inputField = null;
        [SerializeField] ELevel initLevel = ELevel.STORY;
#region SCORE_BOARD
        [SerializeField] List<Transform> scoreObjs = null;
        List<Text> scoreTexts = new List<Text> ( );
        List<Text> userTexts = new List<Text> ( );
        List<Text> timeTexts = new List<Text> ( );
        List<Text> levelTexts = new List<Text> ( );
        List<ScoreData> datas = new List<ScoreData> ( );
        bool bSocreGet = false;
#endregion
        PlayerControl Control => GameManager.Instance == null?null : GameManager.Instance.Control;
        void Awake ( ) {
            foreach (Transform o in scoreObjs) {
                scoreTexts.Add (o.Find ("Score").GetComponent<Text> ( ));
                userTexts.Add (o.Find ("User").GetComponent<Text> ( ));
                timeTexts.Add (o.Find ("Time").GetComponent<Text> ( ));
                levelTexts.Add (o.Find ("Level").GetComponent<Text> ( ));
                o.gameObject.SetActive (false);
            }
            inputField.text = SLController.GetUserName ( );
        }

        void Start ( ) {
            if (SLController.Load ( ) == null)
                continueButton.gameObject.SetActive (false);
            GetScore ( );
        }

        void Update ( ) {
            if (bSocreGet) {
                for (int i = 0; i < datas.Count; i++) {
                    scoreObjs[i].gameObject.SetActive (true);
                    scoreTexts[i].text = datas[i].score;
                    userTexts[i].text = datas[i].user;
                    timeTexts[i].text = datas[i].time;
                    levelTexts[i].text = datas[i].level;

                }
                bSocreGet = false;
                loadText.enabled = false;
            }
            if (Input.GetKeyDown (KeyCode.F2)) {
                initLevel = ELevel.LEVEL1_OTAKU;
            }
        }

        void OnEnable ( ) {
            buttons.ActiveOption += OnActiveOption;
            buttons.DeactiveOption += OnDeactiveOption;
            buttons.CertainAction += OnActionCertain;
            buttons.Init ( );
            if (Control != null) {
                Control.UI.Choose.started += OnAxisValueChanged;
                Control.UI.Confirm.started += OnConfirmPressed;
                Control.UI.Cancel.started += OnCancelPressed;
                Control.Disable ( );
                Control.UI.Enable ( );
            }
            tutorialObject.SetActive (false);
        }

        void OnDisable ( ) {
            buttons.ActiveOption -= OnActiveOption;
            buttons.DeactiveOption -= OnDeactiveOption;
            buttons.CertainAction -= OnActionCertain;
            if (Control != null) {
                Control.UI.Choose.started -= OnAxisValueChanged;
                Control.UI.Confirm.started -= OnConfirmPressed;
                Control.UI.Cancel.started -= OnCancelPressed;
                Control.Disable ( );
            }
        }

        public void OnNewGamePressed ( ) {
            GameManager.Instance.LoadLevel (initLevel);
        }
        public void OnContinuePressed ( ) {
            SaveData data = SLController.Load ( );
            GameManager.Instance.LoadLevel (data.Level, data);
        }

        public void OnScorePressed ( ) {
            scoreBoardObject.SetActive (true);
        }

        public void OnTutorialPressed ( ) {
            tutorialObject.SetActive (true);
        }

        public void OnExitPressed ( ) {
            Application.Quit ( );
        }

        public void OnSetNamePressed ( ) {
            SLController.SetUserName (userText.text);
            Debug.Log (userText.text);
        }

        void OnActiveOption (Text text) {
            text.color = activeColor;
        }

        void OnDeactiveOption (Text text) {
            text.color = deactiveColor;
        }
        void OnActionCertain ( ) { }

        void OnAxisValueChanged (InputAction.CallbackContext ctx) {
            float value = ctx.ReadValue<System.Single> ( );
            if (value < 0)
                buttons.PlusIndex ( );
            else if (value > 0)
                buttons.MinusIndex ( );
        }

        void OnConfirmPressed (InputAction.CallbackContext ctx) {
            if (!tutorialObject.activeSelf && !scoreBoardObject.activeSelf)
                buttons.Invoke ( );
        }
        void OnCancelPressed (InputAction.CallbackContext ctx) {
            if (tutorialObject.activeSelf)
                tutorialObject.SetActive (false);
            else if (scoreBoardObject.activeSelf)
                scoreBoardObject.SetActive (false);
        }

        void GetScore ( ) {
            SLController.WriteLog ("Try to get score");
            CollectionReference scoreboardRef = GameManager.Instance.Db.Collection ("ScoreBoard");
            Query query = scoreboardRef.OrderBy ("Score").Limit (10);
            query.GetSnapshotAsync ( ).ContinueWith (task => {
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>> ( );
                foreach (DocumentSnapshot o in task.Result.Documents) {
                    data.Add (o.ToDictionary ( ));
                }
                for (int i = 0; i < task.Result.Count; i++) {
                    this.datas.Add (new ScoreData (data[i]["User"].ToString ( ), float.Parse (data[i]["Score"].ToString ( )).ToString ("F2"), data[i]["Time"].ToString ( ), data[i]["Level"].ToString ( )));
                }
                bSocreGet = true;
                SLController.WriteLog ("Alreay get the score");
            });
        }
    }

    [System.Serializable]
    class ScoreData {
        public ScoreData (string user, string score, string time, string level) {
            this.user = user;
            this.score = score;
            this.time = time;
            this.level = level;
        }
        public string user;
        public string score;
        public string time;
        public string level;
    }

}