using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]
public class MapEditor : Editor {
    MapGenerator map = null; 

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(map==null)
            map = target as MapGenerator;

        //map.GenerateMap();

        if (GUILayout.Button("Regenerate"))
        {
            map.GenerateMap();
        }
    }
}
