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
    }

    public enum ELevel {
        TITLE = 0,
        LEVEL1 = 1,
        LEVEL2 = 2,
    }
}