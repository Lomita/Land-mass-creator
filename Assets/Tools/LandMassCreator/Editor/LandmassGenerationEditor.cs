using UnityEngine;
using UnityEditor;

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
        /// Serialized LandmassGenerator
        /// </summary>
        private SerializedObject m_serializedLgm = null;

        /// <summary>
        /// Holds the serialized tree prefabs property 
        /// </summary>
        private SerializedProperty m_treePrefabsProperty = null;

        /// <summary>
        /// Holds the serialized plant prefabs property 
        /// </summary>
        private SerializedProperty m_plantPrefabsProperty = null;

        /// <summary>
        /// Holds the serialized other prefabs property 
        /// </summary>
        private SerializedProperty m_otherPrefabsProperty = null;

        /// <summary>
        /// GUI style skin
        /// </summary>
        private GUISkin m_skin = null;

        /// <summary>
        /// This function is called when the object becomes enabled and active
        /// </summary>
        private void OnEnable()
        {
            m_lmg = (LandmassGenerator)target;
            m_skin = (GUISkin)Resources.Load("LMC_GUISkin");

            //get serialized properties from landmasgenerator
            m_serializedLgm = new SerializedObject(target);
            m_treePrefabsProperty = m_serializedLgm.FindProperty("m_treePrefabs");
            m_plantPrefabsProperty = m_serializedLgm.FindProperty("m_plantPrefabs");
            m_otherPrefabsProperty = m_serializedLgm.FindProperty("m_otherPrefabs");

            m_lmg.GenerateTerrain();
        }

        /// <summary>
        /// OnInspectorGUI Override 
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawGUI();
            if (m_lmg.AutoUpdate)
                m_lmg.GenerateTerrain();
        }

        /// <summary>
        /// Function to draw inspector GUI
        /// </summary>
        private void DrawGUI()
        {
            if (m_lmg.Settings == null)
                return;

            //Draw terrain settings GUI
            EditorGUILayout.LabelField("Terrain Settings", m_skin.label);
            EditorGUILayout.BeginVertical("box");           
            m_lmg.Settings.MapHeight = EditorGUILayout.IntField("Length", m_lmg.Settings.MapHeight.Clamp(2, 1000));
            m_lmg.Settings.MapWidth = EditorGUILayout.IntField("Width", m_lmg.Settings.MapWidth.Clamp(2, 1000));      
            EditorGUILayout.EndVertical();

            //Draw noise Settings
            EditorGUILayout.LabelField("Noise Settings", m_skin.label);
            EditorGUILayout.BeginVertical("box");
            m_lmg.Settings.Seed = EditorGUILayout.IntField("Seed", m_lmg.Settings.Seed);
            m_lmg.Settings.Scale = EditorGUILayout.FloatField("Scale", m_lmg.Settings.Scale);

            Vector2 offset = EditorGUILayout.Vector2Field("Scale Offset", new Vector2(m_lmg.Settings.ScaleOffsetX, m_lmg.Settings.ScaleOffsetY));
            m_lmg.Settings.ScaleOffsetX = offset.x;
            m_lmg.Settings.ScaleOffsetY = offset.y;

            m_lmg.Settings.Octaves = EditorGUILayout.IntField("Octaves", m_lmg.Settings.Octaves.Clamp(1, 15));
            m_lmg.Settings.Persistance = EditorGUILayout.Slider("Persistance", m_lmg.Settings.Persistance, 0.0f, 5.0f);
            m_lmg.Settings.Density = EditorGUILayout.Slider("Density", m_lmg.Settings.Density, 0.0f, 1.0f);
            m_lmg.Settings.OceanLevel = EditorGUILayout.FloatField("Ocean Level", m_lmg.Settings.OceanLevel);
            m_lmg.Settings.CapMountainHeight = EditorGUILayout.FloatField("Max Mountain Height", m_lmg.Settings.CapMountainHeight);
            EditorGUILayout.EndVertical();

            //Draw color Settings
            EditorGUILayout.LabelField("Color Settings", m_skin.label);
            EditorGUILayout.BeginVertical("box");
            m_lmg.ColorPalette = EditorGUILayout.GradientField("Vertex Colors", m_lmg.ColorPalette);
            EditorGUILayout.EndVertical();

            //Draw Fill Terrain Settings
            EditorGUILayout.LabelField("Fill Terrain Settings", m_skin.label);
            EditorGUILayout.BeginVertical("box");
               
            EditorGUILayout.PropertyField(m_treePrefabsProperty, new GUIContent("Tree Prefabs"), true);
            EditorGUILayout.PropertyField(m_plantPrefabsProperty, new GUIContent("Plant Prefabs"), true);
            EditorGUILayout.PropertyField(m_otherPrefabsProperty, new GUIContent("Object Prefabs"), true);
            
            EditorGUILayout.EndVertical();

            //Draw Generate Terrain Settings
            EditorGUILayout.LabelField("Generate Terrain", m_skin.label);
            EditorGUILayout.BeginVertical("box");
            m_lmg.DrawGizmosVertices = EditorGUILayout.Toggle("Show Vertices", m_lmg.DrawGizmosVertices);
            m_lmg.AutoUpdate = EditorGUILayout.Toggle("Auto Update", m_lmg.AutoUpdate);
            
            if (GUILayout.Button("Generate")) 
                m_lmg.GenerateTerrain();
            
            EditorGUILayout.EndVertical();

            //Draw Import Export Settings
            EditorGUILayout.LabelField("Export / Import", m_skin.label);
            EditorGUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Export"))
            {
                PortState portState = EditorUtils.ExportTerrainSettings(m_lmg);
                switch (portState.Status)
                {
                    case Status.SUCCESS:
                        break;

                    case Status.CANCEL:
                        break;

                    case Status.FAILED:
                        break;

                    case Status.UNKNOWN:
                        break;
                }
            }

            if (GUILayout.Button("Import"))
            {
                EditorUtils.ImportTerrainSettings(m_lmg);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}