using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor: Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager myScript = (GameManager)target;
        if (GUILayout.Button("ResetPlayerPrefs"))
        {
            Debug.Log("clicked");
            myScript.ResetLevelInfo();
        }
    }
}
#endif