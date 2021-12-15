using UnityEngine;
using LandMassCreator;

public class API : MonoBehaviour
{
    void Start()
    {
        //Create Height map settings
        HeightMapSettings hms = new HeightMapSettings();
        hms.MapHeight = 100;
        hms.MapWidth = 100;
        hms.Octaves = 5;
        hms.OceanLevel = -5.0f;
        //....etz

        //Set height map settings and color gradient
        LandmassGenerator lmg = new LandmassGenerator();
        lmg.Settings = hms;
        lmg.ColorPalette = new Gradient(); // You can set your own color gradient here

        //generate terrain
        lmg.GenerateTerrain();

        //height map Output
        HeightMap heightmap = lmg.Map;
        Mesh terrain = lmg.TerrainMesh;

        Vector3[] vertices = terrain.vertices;
        Color[] colors = terrain.colors;
        int[] triangles = terrain.triangles;

        //your code here 
    }
}