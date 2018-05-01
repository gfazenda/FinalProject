using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(EnemyCoordinator))]
public class EnemyCoordEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyCoordinator myScript = (EnemyCoordinator)target;
        if (GUILayout.Button("GetEnemies"))
        {
            Debug.Log("clicked");
            myScript.RereadEnemies();
        }
    }
}
#endif