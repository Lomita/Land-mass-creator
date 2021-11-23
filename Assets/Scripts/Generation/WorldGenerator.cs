using System.Collections.Generic;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace LandMassCreator
{
    public class WorldGenerator : MonoBehaviour
    {
        [Header("Prefabs Settings")]
        [SF] private GameObject CubePrefab;
        [SF] private GameObject[] TreePrefabs;
        [SF] private GameObject[] BushPrefabs;
        [SF] private GameObject[] ObjectPrefabs;

        [Header("World Settings")]
        [SF] private int WorldLength;
        [SF] private int WorldWide;
        [SF] private int TreeCount;
        [SF] private int BushCount;
        [SF] private int ObjectCount;
        [SF] private bool UseCubes = true;

        [Header("Noise Settings")]
        [SF] private int Seed;
        [SF] private float NoiseScale;
        [SF] private Vector2 ScaleOffset;
        [SF] private int Octaves;
        [SF] private float Persistance;
        [SF] private float Density;
        [SF] private float OceanLevel;
        [SF] private float CapMountainHeight;

        [Header("Color Settings")]
        [SF] private Gradient Gradiant;

        [Header("Generation")]
        [SF] public bool AutoUpdate = true;
        [SF] public bool DrawGizmosVertices = false;

        Mesh World;                                                 //World mesh
        Vector3[] Vertices;                                         //mesh vertices
        Color[] Colors;                                             //world colors
        int[] Triangles;                                            //mesh triangles
        HeightMap NoiseMapOut;

        private void Awake()
        {
            GenerateWorld();
        }

        //Generate worlds triangles and vertices
        public void GenerateWorld()
        {
            HeightMapSettings heightMapSettings = new HeightMapSettings(WorldWide, WorldLength,
               Seed, NoiseScale, ScaleOffset.x, ScaleOffset.y, Octaves, Persistance, Density, OceanLevel, CapMountainHeight);

            NoiseMapOut = Noise.GenerateHeightMap(heightMapSettings);

            if (UseCubes)
            {
                if (World) World.Clear();
                CubeWorld();
            }
            else
                TriangleWorld();

            if (!Application.isPlaying)
                Debug.Log("World not getting filled if in editor mode =) !!!!");
            /*else
                FillWorld();*/
        }

        private void CubeWorld()
        {
            if (!Application.isPlaying)
            {
                Debug.LogError("Dont use Cubes in editor mode =)!!!");
                return;
            }

            Vertices = new Vector3[(WorldLength + 1) * (WorldWide + 1)];
            Colors = new Color[(WorldLength + 1) * (WorldWide + 1)];

            for (int idx = 0, x = 0; x < WorldWide; x++)
            {
                for (int y = 0; y < WorldLength; y++)
                {
                    Vertices[idx] = new Vector3(x, NoiseMapOut.HeightMapReal[x, y], y);
                    Colors[idx] = Gradiant.Evaluate(NoiseMapOut.HeightMapNormalized[x, y]);

                    GameObject cube = Instantiate(CubePrefab, Vertices[idx], Quaternion.identity, transform);
                    MeshFilter mf = cube.GetComponent<MeshFilter>();
                    Mesh m = mf.mesh;

                    Color[] c = new Color[m.vertexCount];

                    for (int v = 0; v < m.vertexCount - 1; v++)
                        c[v] = Colors[idx];

                    m.SetColors(c);

                    idx++;
                }
            }
        }

        private void TriangleWorld()
        {
            World = new Mesh();
            MeshFilter m = GetComponent<MeshFilter>();
            m.mesh = World;

            Vertices = new Vector3[(WorldLength + 1) * (WorldWide + 1)];
            Colors = new Color[(WorldLength + 1) * (WorldWide + 1)];

            for (int idx = 0, x = 0; x < WorldWide; x++)
            {
                for (int y = 0; y < WorldLength; y++)
                {
                    Vertices[idx] = new Vector3(x, NoiseMapOut.HeightMapReal[x, y], y);
                    Colors[idx] = Gradiant.Evaluate(NoiseMapOut.HeightMapNormalized[x, y]);
                    idx++;
                }
            }

            int Size = WorldWide * WorldLength * 6;
            Triangles = new int[Size];

            int t = 0, v = 0;
            for (int x = 0; x < WorldWide - 1; x++)
            {
                for (int y = 0; y < WorldLength - 1; y++)
                {
                    Triangles[t + 0] = v + 0;
                    Triangles[t + 1] = v + 1;
                    Triangles[t + 2] = v + WorldLength + 1;
                    Triangles[t + 3] = v + 0;
                    Triangles[t + 4] = v + WorldLength + 1;
                    Triangles[t + 5] = v + WorldLength;

                    v++;
                    t += 6;
                }

                v++;
            }

            UpdateMesh();
        }

        /*
        private void FillWorld()
        {
            //Spawn Trees
            InstantiateObjects(TreeCount, TreePrefabs, NoiseMapOut.ForestValues, Vector3.one);

            //Spawn Bushes
            //only 1/8 of the bushes should spawn on mountain terrain
            int newBushCount = BushCount - (BushCount % 8);
            int mountainBushCount = newBushCount / 8;
            int forestBushCount = newBushCount - mountainBushCount;

            InstantiateObjects(forestBushCount, BushPrefabs, NoiseMapOut.ForestValues, Vector3.one);
            InstantiateObjects(mountainBushCount, BushPrefabs, NoiseMapOut.MountainValues, Vector3.one);

            //Spawn Objects
            //only 1/8 of the objects should spawn on mountain terrain
            int newObjectCount = ObjectCount - (ObjectCount % 8);
            int mountainObjectCount = newObjectCount / 8;
            int forestObjectCount = newObjectCount - mountainObjectCount;
            Vector3 size = new Vector3(0.5f, 0.5f, 0.5f);

            InstantiateObjects(forestObjectCount, ObjectPrefabs, NoiseMapOut.ForestValues, size);
            InstantiateObjects(mountainObjectCount, ObjectPrefabs, NoiseMapOut.MountainValues, size);
        }*/

        private List<GameObject> InstantiateObjects(int count, GameObject[] prefabs, Vector3[] possiblePositions, Vector3 size)
        {
            List<GameObject> objs = new List<GameObject>();

            if (prefabs.Length == 0 || possiblePositions.Length == 0)
                return objs;

            for (int idx = 0; idx < count; idx++)
            {
                GameObject bushPrefab = prefabs[Random.Range(0, prefabs.Length)];
                Vector3 position = possiblePositions[Random.Range(0, possiblePositions.Length)];
                GameObject obj = Instantiate(bushPrefab, position, Quaternion.identity, transform);
                obj.transform.localScale = size;
                objs.Add(obj);
            }

            return objs;
        }

        //update triangles and vertices of world mesh
        private void UpdateMesh()
        {
            if (!World) return;

            World.Clear();

            World.vertices = Vertices;
            World.triangles = Triangles;
            World.colors = Colors;

            World.RecalculateNormals();
        }

        //draw grid debug
        private void OnDrawGizmos()
        {
            if (!DrawGizmosVertices || Vertices == null) return;

            for (int idx = 0; idx < Vertices.Length; idx++)
                Gizmos.DrawCube(Vertices[idx], new Vector3(0.1f, 0.1f, 0.1f));
        }

        private void OnValidate()
        {
            if (WorldLength < 1) WorldLength = 1;
            if (WorldWide < 1) WorldWide = 1;
        }
    }
}