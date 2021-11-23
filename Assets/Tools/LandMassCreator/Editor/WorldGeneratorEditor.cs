using UnityEngine;
using UnityEditor;

namespace LandMassCreator
{
    /// <summary>
    /// Custom Editor of type WorldGenerator
    /// </summary>
    [CustomEditor(typeof(WorldGenerator))]
    public class WorldGeneratorEditor : Editor
    {
        /// <summary>
        /// OnInspectorGUI Override 
        /// </summary>
        public override void OnInspectorGUI()
        {
            WorldGenerator worldGen = (WorldGenerator)target;

            if (DrawDefaultInspector() && worldGen.AutoUpdate)
                worldGen.GenerateWorld();

            if (GUILayout.Button("Generate"))
                worldGen.GenerateWorld();
        }
    }
}