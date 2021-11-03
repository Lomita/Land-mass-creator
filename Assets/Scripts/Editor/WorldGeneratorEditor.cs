using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WorldGenerator worldGen = (WorldGenerator)target;

        if (DrawDefaultInspector() && worldGen.AutoUpdate)
            worldGen.GenerateWorld();

        if (GUILayout.Button("Generate"))
            worldGen.GenerateWorld();
    }
}
