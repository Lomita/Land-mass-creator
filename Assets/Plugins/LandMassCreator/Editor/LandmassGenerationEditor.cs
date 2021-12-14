using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Diagnostics;
using System;
using SFB;

namespace LandMassCreator
{
    /// <summary>
    /// Custom Editor of type WorldGenerator
    /// </summary>
    [CustomEditor(typeof(LandmassGenerator))]
    public class LandmassGeneratorEditor : Editor
    {
        /// <summary>
        /// The target of this custom editor LandmassGenerator
        /// </summary>
        private LandmassGenerator m_lmg = null;

        /// <summary>
        /// GUI style skin
        /// </summary>
        private GUISkin m_skin = null;

        /// <summary>
        /// Export path selection callback
        /// </summary>
        private static Action<string> OnExportPathSelected;
        
        /// <summary>
        /// Import path selection callback
        /// </summary>
        private Action<string[]> OnImportFileSelected;

        /// <summary>
        /// This function is called when the object becomes enabled and active
        /// </summary>
        private void OnEnable()
        {
            m_lmg = (LandmassGenerator)target;
            m_skin = (GUISkin)Resources.Load("LMC_GUISkin");

            OnExportPathSelected = Export;
            OnImportFileSelected = Import;
        }

        /// <summary>
        /// OnInspectorGUI Override 
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawGUI();

            if (GUI.changed)
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                
            if (m_lmg.AutoUpdate)
                m_lmg.GenerateTerrain();
        }

        /// <summary>
        /// Function to draw inspector GUI
        /// </summary>
        private void DrawGUI()
        {
            serializedObject.Update();

            DrawGUINoiseSettings();
            DrawGUIColorSettings();
            DrawGUIGenerateSettings();
            DrawGUIExportImportSettings();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draw GUI noise settings
        /// </summary>
        private void DrawGUINoiseSettings()
        {
            SerializedProperty heightMapProperty = serializedObject.FindProperty("m_settings");

            //Draw terrain settings GUI
            EditorGUILayout.LabelField("Terrain Settings", m_skin.label);
            EditorGUILayout.BeginVertical("box");

            heightMapProperty.FindPropertyRelative("m_mapHeight").intValue = EditorGUILayout.IntField("Length", m_lmg.Settings.MapHeight).Clamp(2, 200);
            heightMapProperty.FindPropertyRelative("m_mapWidth").intValue = EditorGUILayout.IntField("Width", m_lmg.Settings.MapWidth).Clamp(2, 200);

            EditorGUILayout.EndVertical();

            //Draw noise Settings
            EditorGUILayout.LabelField("Noise Settings", m_skin.label);
            EditorGUILayout.BeginVertical("box");
            heightMapProperty.FindPropertyRelative("m_seed").intValue = EditorGUILayout.IntField("Seed", m_lmg.Settings.Seed);
            heightMapProperty.FindPropertyRelative("m_scale").floatValue = EditorGUILayout.FloatField("Scale", m_lmg.Settings.Scale);

            Vector2 offset = EditorGUILayout.Vector2Field("Scale Offset", new Vector2(m_lmg.Settings.ScaleOffsetX, m_lmg.Settings.ScaleOffsetY));
            heightMapProperty.FindPropertyRelative("m_scaleOffsetX").floatValue = offset.x;
            heightMapProperty.FindPropertyRelative("m_scaleOffsetY").floatValue = offset.y;

            heightMapProperty.FindPropertyRelative("m_octaves").intValue = EditorGUILayout.IntField("Octaves", m_lmg.Settings.Octaves).Clamp(1, 15); ;
            heightMapProperty.FindPropertyRelative("m_persistence").floatValue = EditorGUILayout.Slider("Persistence", m_lmg.Settings.Persistence, 0.0f, 5.0f).Clamp(0.0f, 5.0f);
            heightMapProperty.FindPropertyRelative("m_density").floatValue = EditorGUILayout.Slider("Density", m_lmg.Settings.Density, 0.0f, 1.0f).Clamp(0.0f, 1.0f);
            heightMapProperty.FindPropertyRelative("m_oceanLevel").floatValue = EditorGUILayout.FloatField("Ocean Level", m_lmg.Settings.OceanLevel);
            heightMapProperty.FindPropertyRelative("m_CapMountainHeight").floatValue = EditorGUILayout.FloatField("Max Mountain Height", m_lmg.Settings.CapMountainHeight);
            
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Draw GUI color settings
        /// </summary>
        private void DrawGUIColorSettings()
        {
            EditorGUILayout.LabelField("Color Settings", m_skin.label);
            EditorGUILayout.BeginVertical("box");
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_colorPalette"), new GUIContent("Vertex Colors"));
            
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Draw GUI generate settings
        /// </summary>
        private void DrawGUIGenerateSettings()
        {
            EditorGUILayout.LabelField("Generate Terrain", m_skin.label);
            EditorGUILayout.BeginVertical("box");

            serializedObject.FindProperty("m_drawGizmosVertices").boolValue = EditorGUILayout.Toggle("Show Vertices", m_lmg.DrawGizmosVertices);
            serializedObject.FindProperty("m_autoUpdate").boolValue = EditorGUILayout.Toggle("Auto Update", m_lmg.AutoUpdate);

            if (GUILayout.Button("Generate"))
                m_lmg.GenerateTerrain();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Draw GUI export import settings
        /// </summary>
        private void DrawGUIExportImportSettings()
        {
            EditorGUILayout.LabelField("Export / Import", m_skin.label);
            EditorGUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Export"))
            {
                StandaloneFileBrowser.SaveFilePanelAsync("Select export directory", 
                    EditorUtils.StandardPortPath, "terrain", "lmg", OnExportPathSelected);
            }

            if (GUILayout.Button("Import"))
            {
                StandaloneFileBrowser.OpenFilePanelAsync("Select terrain settings for import", 
                    EditorUtils.StandardPortPath, "lmg", false, OnImportFileSelected);
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Called by SaveFilePanelAsync OnExportPathSelected
        /// Exports the terrain settings and handles errors
        /// </summary>
        /// <param name="path">The selected export file path</param>
        private void Export(string path)
        {
            PortState portState = EditorUtils.ExportTerrainSettings(m_lmg, path);

            switch (portState.Status)
            {
                case Status.SUCCESS:
                    {
                        if (EditorUtility.DisplayDialog("Export Success",
                            "Would you like to open the export folder?", "Yes", "No"))
                        {
#if UNITY_STANDALONE_OSX
                            Process.Start("open", portState.Path);
#elif UNITY_STANDALONE_WIN
                            Process.Start("explorer.exe", portState.Path);
#elif UNITY_STANDALONE_LINUX
                            Process.Start("xdg-open", portState.Path);
#endif
                        }
                        break;
                    }

                case Status.CANCEL:
                    break;

                case Status.FAILED:
                case Status.UNKNOWN:
                    {
                        EditorUtility.DisplayDialog("Export Failed",
                            "Error: " + portState.Msg, "Ok");
                        break;
                    }
            }
        }

        /// <summary>
        /// Called by OpenFilePanelAsync OnImportFileSelected
        /// Imports the terrain settings and handles errors
        /// </summary>
        /// <param name="path">The selected import file path</param>
        private void Import(string[] path)
        {
            PortState portState = new PortState(Status.CANCEL);

            if (!path.Equals(null) && !path.Length.Equals(0))
                portState = EditorUtils.ImportTerrainSettings(m_lmg, path[0]);

            switch (portState.Status)
            {
                case Status.SUCCESS:
                    {
                        //repaint not working for gradient fields =C
                        Repaint();

                        if (!m_lmg.AutoUpdate)
                            m_lmg.GenerateTerrain();

                        break;
                    }

                case Status.CANCEL:
                    break;

                case Status.FAILED:
                case Status.UNKNOWN:
                    {
                        EditorUtility.DisplayDialog("Import Failed",
                            "Error: " + portState.Msg, "Ok");
                        break;
                    }
            }
        }
    }
}