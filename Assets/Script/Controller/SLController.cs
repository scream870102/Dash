namespace CJStudio.Dash {
    using System.IO;
    using System;
    using UnityEngine;

    static class SLController {
        public static void Save (SaveData data) {
            string path = Application.persistentDataPath + "/data.json";
            FileStream fs = new FileStream (path, FileMode.Create);
            string fileContext = JsonUtility.ToJson (data);
            StreamWriter file = new StreamWriter (fs);
            file.Write (fileContext);
            file.Close ( );
        }

        public static SaveData Load ( ) {
            SaveData data = null;
            string path = Application.persistentDataPath + "/data.json";
            if (!File.Exists (path)) return null;
            return data = JsonUtility.FromJson<SaveData> (File.ReadAllText (path));
        }

        public static void SetUserName (string name) {
            string path = Application.persistentDataPath + "/user.txt";
            FileStream fs = new FileStream (path, FileMode.Create);
            string fileContext = name;
            StreamWriter file = new StreamWriter (fs);
            file.Write (fileContext);
            file.Close ( );
            SLController.WriteLog ("Set User Name to " + name);
        }

        public static string GetUserName ( ) {
            string path = Application.persistentDataPath + "/user.txt";
            if (!File.Exists (path)) {
                SetUserName ("Momoko kawaii");
                return "Momoko kawaii";
            }
            return File.ReadAllText (path);
        }

        public static void WriteLog (string content) {
            string path = Application.persistentDataPath + "/log.txt";
            FileStream fs = new FileStream (path, FileMode.Append);

            string fileContext = System.DateTime.Now.ToShortDateString ( ) + " " + System.DateTime.Now.ToShortTimeString ( ) + "   " + content + "\n";
            StreamWriter file = new StreamWriter (fs);
            file.Write (fileContext);
            file.Close ( );
        }
    }

    public enum ELevel {
        TITLE = 0,
        LEVEL1 = 1,
        LEVEL2 = 2,
    }
}