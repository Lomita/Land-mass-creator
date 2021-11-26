using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LandMassCreator
{
    /// <summary>
    /// Class for all Menu Items
    /// </summary>
    public class MenuItems
    {
        /// <summary>
        /// Creates a new terrain scene 
        /// </summary>
        [MenuItem("Tools/Land Mass Creator/New Terrain")]
        private static void NewTerrain()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            //New Scene
            UnityEngine.SceneManagement.Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            newScene.name = "New Terrain";

            //lighting
            GameObject directionalLightGO = new GameObject("Directional Light");
            directionalLightGO.transform.rotation = Quaternion.Euler(50.0f, -30.0f, 0.0f);
            Light light = directionalLightGO.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(1.0f, 0.9568627f, 0.8392157f, 1.0f);
            light.shadows = LightShadows.Soft;
            light.shadowStrength = 1.0f;
            light.shadowBias = 0.05f;
            light.shadowNormalBias = 0.4f;
            light.shadowNearPlane = 0.2f;

            //landmass generator
            GameObject LandmassGeneratorGO = new GameObject("Terrain");
            LandmassGeneratorGO.AddComponent<LandmassGenerator>();
            LandmassGeneratorGO.AddComponent<MeshFilter>();
            MeshRenderer renderer = LandmassGeneratorGO.AddComponent<MeshRenderer>();
            renderer.material = new Material(Shader.Find("Custom/VertexColor"));

            //Select terrain gameobject
            Selection.activeGameObject = LandmassGeneratorGO;
        }
    }
}