using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace LandMassCreator
{
    /// <summary>
    /// Landmass generator creates a terrain based on a height map
    /// </summary>
    public class LandmassGenerator : MonoBehaviour
    {
        /// <summary>
        /// Height map settings
        /// </summary>
        [SF] private HeightMapSettings m_settings = new HeightMapSettings();

        /// <summary>
        /// Generated height map
        /// </summary>
        private HeightMap m_map = null;

        /// <summary>
        /// gradiant for height colors
        /// </summary>
        [SF] private Gradient m_colorPalette = new Gradient();

        /// <summary>
        /// Auto updates all values if checked
        /// </summary>
        [SF] private bool m_autoUpdate = true;

        /// <summary>
        /// Draw all vertices
        /// </summary>
        [SF] private bool m_drawGizmosVertices = false;

        /// <summary>
        /// Terrain mesh
        /// </summary>
        private Mesh m_terrainMesh = null;
        
        /// <summary>
        /// Mesh vertices
        /// </summary>
        private Vector3[] m_vertices = null;                                         
        
        /// <summary>
        /// Mesh vertex colors
        /// </summary>
        private Color[] m_colors = null;
        
        /// <summary>
        /// Mesh triangles
        /// </summary>
        private int[] m_triangles = null;

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
        /// Gets the terrain Mesh
        /// </summary>
        public Mesh TerrainMesh { get => m_terrainMesh; }

        /// <summary>
        /// Gets the height map output
        /// </summary>
        public HeightMap Map { get => m_map;}

        /// <summary>
        /// Generates the terrain
        /// </summary>
        public void GenerateTerrain()
        {
            m_map = Noise.GenerateHeightMap(Settings);
            DrawTriangles();
        }

        /// <summary>
        /// Draws the triangles of the terrain
        /// </summary>
        private void DrawTriangles()
        {
            m_terrainMesh = new Mesh();

            try
            {
                MeshFilter m = gameObject.GetComponent<MeshFilter>();
                m.mesh = m_terrainMesh;
            }
            catch
            { 
            
            }

            int worldLength = Settings.MapHeight;
            int worldWidth = Settings.MapWidth;

            m_vertices = new Vector3[(worldLength + 1) * (worldWidth + 1)];
            m_colors = new Color[(worldLength + 1) * (worldWidth + 1)];

            for (int idx = 0, x = 0; x < worldWidth; x++)
            {
                for (int y = 0; y < worldLength; y++)
                {
                    m_vertices[idx] = new Vector3(x, m_map.HeightMapReal[x, y], y);
                    m_colors[idx] = ColorPalette.Evaluate(m_map.HeightMapNormalized[x, y]);
                    idx++;
                }
            }

            int Size = worldWidth * worldLength * 6;
            m_triangles = new int[Size];

            int t = 0, v = 0;
            for (int x = 0; x < worldWidth - 1; x++)
            {
                for (int y = 0; y < worldLength - 1; y++)
                {
                    m_triangles[t + 0] = v + 0;
                    m_triangles[t + 1] = v + 1;
                    m_triangles[t + 2] = v + worldLength + 1;
                    m_triangles[t + 3] = v + 0;
                    m_triangles[t + 4] = v + worldLength + 1;
                    m_triangles[t + 5] = v + worldLength;

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
            if (!m_terrainMesh) return;

            m_terrainMesh.Clear();

            m_terrainMesh.vertices = m_vertices;
            m_terrainMesh.triangles = m_triangles;
            m_terrainMesh.colors = m_colors;

            m_terrainMesh.RecalculateNormals();
        }

        /// <summary>
        /// Draw grid debug
        /// </summary>
        private void OnDrawGizmos()
        {
            if (!m_drawGizmosVertices || m_vertices == null) 
                return;

            Color oldCol = Gizmos.color;
            Gizmos.color = Color.magenta;
            for (int idx = 0; idx < m_vertices.Length; idx++)
                Gizmos.DrawCube(m_vertices[idx] + transform.position, new Vector3(0.1f, 0.1f, 0.1f));

            Gizmos.color = oldCol;
        }
    }
}