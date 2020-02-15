namespace CJStudio.Dash {
    using System.Collections.Generic;

    using MapObject;

    using UnityEditor;

    using UnityEngine;
    class Example {
        [MenuItem ("CONTEXT/Stage/Set List")]
        static void SetList (MenuCommand command) {
            Stage stage = command.context as Stage;
            if (stage.RecoverObjectParent != null) {
                stage.stageObjects.Clear ( );
                for (int i = 0; i < stage.RecoverObjectParent.transform.childCount; i++) {
                    stage.stageObjects.AddRange (stage.RecoverObjectParent.transform.GetChild (i).GetComponents<AMapObject> ( ));

                }
            }
        }

        [MenuItem ("GameObject/Stage/Add New Stage")]
        static void NewStage ( ) {
            // GameObject parent = new GameObject ("Stage");
            // GameObject recoverObjectParent = new GameObject ("RecoverObjectParent");
            // recoverObjectParent.transform.parent = parent.transform;
            // GameObject savePoint = GameObject.Instantiate (PrefabUtility.LoadPrefabContents ("Assets/Prefab/SavePoint.prefab"));
            // savePoint.name = "SavePoint";
            // savePoint.transform.parent = parent.transform;
            GameObject stage = GameObject.Instantiate (PrefabUtility.LoadPrefabContents ("Assets/Prefab/Stage.prefab"));
            stage.name = "Stage";
        }

        [MenuItem ("GameObject/Stage/Set all Stage List")]
        static void SetAllStageList ( ) {
            int count = 0;
            List<Stage> stages = new List<Stage> (GameObject.FindObjectsOfType<Stage> ( ));
            foreach (Stage stage in stages) {
                if (stage.RecoverObjectParent != null) {
                    stage.stageObjects.Clear ( );
                    count++;
                    for (int i = 0; i < stage.RecoverObjectParent.transform.childCount; i++) {
                        stage.stageObjects.AddRange (stage.RecoverObjectParent.transform.GetChild (i).GetComponents<AMapObject> ( ));

                    }
                }
            }
            Debug.Log ("Find " + count + " Stages and Finish set list");
        }
    }
}
