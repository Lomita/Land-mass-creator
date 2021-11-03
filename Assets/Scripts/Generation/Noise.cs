using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
	public static NoiseMapOutput GenerateNoiseMap(NoiseSettings ns)
	{
		NoiseMapOutput Output = new NoiseMapOutput(new float[ns.MapWidth, ns.MapHeight], 
			new float[ns.MapWidth, ns.MapHeight]);

		System.Random rng = new System.Random(ns.Seed);
		Vector2[] octaveOffsets = new Vector2[ns.Octaves];
		for (int i = 0; i < ns.Octaves; i++)
		{
			float scaleOffsetX = rng.Next(-100000, 100000) + ns.ScaleOffset.x;
			float scaleOffsetY = rng.Next(-100000, 100000) + ns.ScaleOffset.y;
			octaveOffsets[i] = new Vector2(scaleOffsetX, scaleOffsetY);
		}

		if (ns.Scale <= 0) ns.Scale = 0.0001f;

		//to scale into the center
		float halfWidth = ns.MapWidth / 2.0f;
		float halfHeight = ns.MapHeight / 2.0f;

		for (int y = 0; y < ns.MapHeight; y++)
		{
			for (int x = 0; x < ns.MapWidth; x++)
			{
				float amplitude = 1.0f;
				float frequency = 1.0f;
				float noiseHeight = 0.0f;

				for (int i = 0; i < ns.Octaves; i++)
				{
					float xCoord = (x - halfWidth) / ns.Scale * frequency + octaveOffsets[i].x;
					float yCoord = (y - halfHeight) / ns.Scale * frequency + octaveOffsets[i].y;

					//2.0f - 1.0f for negative perlin values 
					float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2.0f - 1.0f; 
					noiseHeight += perlinValue * amplitude;

					amplitude *= ns.Persistance;
					frequency *= ns.Density;
				}

				if (noiseHeight < ns.OceanLevel)
					noiseHeight = ns.OceanLevel;
				
				if (noiseHeight > ns.CapMountainHeight)
					noiseHeight = ns.CapMountainHeight;

				if (noiseHeight > Output.MaxHeight)
					Output.MaxHeight = noiseHeight;
				else if(noiseHeight < Output.MinHeight)
					Output.MinHeight = noiseHeight;

				Output.NoiseMap[x, y] = noiseHeight;
			}
		}

		//normalize noise map
		for (int y = 0; y < ns.MapHeight; y++)
		{
			for (int x = 0; x < ns.MapWidth; x++)
				Output.NoiseMapNormalized[x, y] = Mathf.InverseLerp(Output.MinHeight, Output.MaxHeight, Output.NoiseMap[x, y]);
		}
		
		List<Vector3> OceanValues = new List<Vector3>();
		List<Vector3> ForestValues = new List<Vector3>();
		List<Vector3> MountainValues = new List<Vector3>();
		List<Vector3> PeakValues = new List<Vector3>();

		//Filter the noise
		for (int y = 0; y < ns.MapHeight; y++)
		{
			for (int x = 0; x < ns.MapWidth; x++)
            {
				float normalizedValue = Output.NoiseMapNormalized[x, y];
				float value = Output.NoiseMap[x, y];

				if (normalizedValue < 0.08f)
					OceanValues.Add(new Vector3(x, value, y));
				else if (normalizedValue < 0.55f)
					ForestValues.Add(new Vector3(x, value, y));
				else if (normalizedValue < 0.95f)
					MountainValues.Add(new Vector3(x, value, y));
				else
					PeakValues.Add(new Vector3(x, value, y));
			}
		}

		Output.OceanValues = OceanValues.ToArray();
		Output.ForestValues = ForestValues.ToArray();
		Output.MountainValues = MountainValues.ToArray();
		Output.PeakValues = PeakValues.ToArray();

		return Output;
	}
}

//Data class for generating a noise map 
public struct NoiseSettings
{
    public int MapWidth;
	public int MapHeight;
	public int Seed;
	public float Scale;
	public Vector2 ScaleOffset;
	public int Octaves;
	public float Persistance;
	public float Density;
	public float OceanLevel;
	public float CapMountainHeight;

    public NoiseSettings(int mapWidth, int mapHeight, int seed, 
		float scale, Vector2 scaleOffset, int octaves, 
		float persistance, float density, float oceanLevel, 
		float capMountainHeight)
    {
        MapWidth = mapWidth;
        MapHeight = mapHeight;
        Seed = seed;
        Scale = scale;
		ScaleOffset = scaleOffset;
        Octaves = octaves;
        Persistance = persistance;
		Density = density;
        OceanLevel = oceanLevel;
        CapMountainHeight = capMountainHeight;
    }
}

//Noise map generation Output
public class NoiseMapOutput
{
	public float[,] NoiseMap;									//noise map with real values
	public float[,] NoiseMapNormalized;                         //normalized noise map with values beetween 0 and 1, max height is 1 and min height is 0

	public Vector3[] OceanValues;								//Ocean vertices
	public Vector3[] ForestValues;                              //Forest vertices
	public Vector3[] MountainValues;                            //Mountain vertices
	public Vector3[] PeakValues;                                //Peak (snow) vertices

	public float MaxHeight = float.MinValue;					//Max Height 
	public float MinHeight = float.MaxValue;                    //Min Height

    public NoiseMapOutput(float[,] noiseMap, float[,] noiseMapNormalized)
    {
		NoiseMap = noiseMap;
		NoiseMapNormalized = noiseMapNormalized;
    }
}