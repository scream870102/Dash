using CJStudio.Dash;

using UnityEditor;

using UnityEngine;

public class Example {
    [MenuItem ("CONTEXT/Stage/Set List")]
    static void SetList (MenuCommand command) {
        Stage stage = command.context as Stage;
        if (stage.RecoverObjectParent != null) {
            stage.stageObjects.Clear ( );
            for (int i = 0; i < stage.RecoverObjectParent.transform.childCount; i++) {
                stage.stageObjects.Add (stage.RecoverObjectParent.transform.GetChild (i).gameObject);

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
}
