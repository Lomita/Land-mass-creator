using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace LandMassCreator
{
    public class LandmassGenerator : MonoBehaviour
    {
        /// <summary>
        /// Height map settings
        /// </summary>
        private HeightMapSettings m_settings = new HeightMapSettings();
        
        /// <summary>
        /// Generated height map
        /// </summary>
        private HeightMap m_map = null;

        /// <summary>
        /// gradiant for height colors
        /// </summary>
        private Gradient m_colorPalette = new Gradient();

        /// <summary>
        /// Auto updates all values if checked
        /// </summary>
        private bool m_autoUpdate = true;

        /// <summary>
        /// Draw all vertices
        /// </summary>
        private bool m_drawGizmosVertices = false;

        /// <summary>
        /// Array of tree prefabs
        /// </summary>
        [SF] private GameObject[] m_treePrefabs = null;

        /// <summary>
        /// Array of plantprefabs
        /// </summary>
        [SF] private GameObject[] m_plantPrefabs = null;

        /// <summary>
        /// Array of Otherprefabs
        /// </summary>
        [SF] private GameObject[] m_otherPrefabs = null;

        private Mesh World;                                                 //World mesh
        private Vector3[] Vertices;                                         //mesh vertices
        private Color[] Colors;                                             //world colors
        private int[] Triangles;                                            //mesh triangles

        /// <summary>
        /// Gets and sets height map settings
        /// </summary>
        public HeightMapSettings Settings { get => m_settings; set => m_settings = value; }
        
        /// <summary>
        /// Gets and sets the height color palette
        /// </summary>
        public Gradient ColorPalette { get => m_colorPalette; set => m_colorPalette = value; }

        /// <summary>
        /// Gets and sets whether vertices are to be drawn
        /// </summary>
        public bool DrawGizmosVertices { get => m_drawGizmosVertices; set => m_drawGizmosVertices = value; }

        /// <summary>
        /// Gets and sets if the values should be updated automatically
        /// </summary>
        public bool AutoUpdate { get => m_autoUpdate; set => m_autoUpdate = value; }
        
        /// <summary>
        /// Gets and Sets tree prefabs
        /// </summary>
        public GameObject[] TreePrefabs { get => m_treePrefabs; set => m_treePrefabs = value; }

        /// <summary>
        /// Gets and Sets plant prefabs
        /// </summary>
        public GameObject[] PlantPrefabs { get => m_plantPrefabs; set => m_plantPrefabs = value; }

        /// <summary>
        /// Gets and Sets other prefabs
        /// </summary>
        public GameObject[] OtherPrefabs { get => m_otherPrefabs; set => m_otherPrefabs = value; }

        /// <summary>
        /// Generates the terrain
        /// </summary>
        public void GenerateTerrain()
        {
            m_map = Noise.GenerateHeightMap(Settings);
            DrawTriangles();

            /*
            if (!Application.isPlaying)
                Debug.Log("World not getting filled if in editor mode =) !!!!");
            else
                FillWorld();
            */
        }

        /// <summary>
        /// Draws the triangles of the terrain
        /// </summary>
        private void DrawTriangles()
        {
            World = new Mesh();
            MeshFilter m = GetComponent<MeshFilter>();
            m.mesh = World;

            int worldLength = Settings.MapHeight;
            int worldWidth = Settings.MapWidth;

            Vertices = new Vector3[(worldLength + 1) * (worldWidth + 1)];
            Colors = new Color[(worldLength + 1) * (worldWidth + 1)];

            for (int idx = 0, x = 0; x < worldWidth; x++)
            {
                for (int y = 0; y < worldLength; y++)
                {
                    Vertices[idx] = new Vector3(x, m_map.HeightMapReal[x, y], y);
                    Colors[idx] = ColorPalette.Evaluate(m_map.HeightMapNormalized[x, y]);
                    idx++;
                }
            }

            int Size = worldWidth * worldLength * 6;
            Triangles = new int[Size];

            int t = 0, v = 0;
            for (int x = 0; x < worldWidth - 1; x++)
            {
                for (int y = 0; y < worldLength - 1; y++)
                {
                    Triangles[t + 0] = v + 0;
                    Triangles[t + 1] = v + 1;
                    Triangles[t + 2] = v + worldLength + 1;
                    Triangles[t + 3] = v + 0;
                    Triangles[t + 4] = v + worldLength + 1;
                    Triangles[t + 5] = v + worldLength;

                    v++;
                    t += 6;
                }

                v++;
            }

            UpdateMesh();
        }

        /// <summary>
        /// Update triangles and vertices of world mesh
        /// </summary>
        private void UpdateMesh()
        {
            if (!World) return;

            World.Clear();

            World.vertices = Vertices;
            World.triangles = Triangles;
            World.colors = Colors;

            World.RecalculateNormals();
        }

        /// <summary>
        /// Draw grid debug
        /// </summary>
        private void OnDrawGizmos()
        {
            if (!m_drawGizmosVertices || Vertices == null) return;

            Color oldCol = Gizmos.color;
            Gizmos.color = Color.magenta;
            for (int idx = 0; idx < Vertices.Length; idx++)
                Gizmos.DrawCube(Vertices[idx], new Vector3(0.1f, 0.1f, 0.1f));

            Gizmos.color = oldCol;
        }
    }
}