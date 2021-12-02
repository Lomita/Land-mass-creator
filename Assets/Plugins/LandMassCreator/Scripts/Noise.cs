using UnityEngine;

namespace LandMassCreator
{
	/// <summary>
	/// Class to generate the height map with perlin noise
	/// </summary>
	public static class Noise
	{
		/// <summary>
		/// Generates the height map based on the settings
		/// </summary>
		/// <param name="hms">Height map settings</param>
		/// <returns></returns>
		public static HeightMap GenerateHeightMap(HeightMapSettings hms)
		{
			HeightMap Output = new HeightMap(new float[hms.MapWidth, hms.MapHeight],
				new float[hms.MapWidth, hms.MapHeight]);

			System.Random rng = new System.Random(hms.Seed);
			Vector2[] octaveOffsets = new Vector2[hms.Octaves];
			for (int i = 0; i < hms.Octaves; i++)
			{
				float scaleOffsetX = rng.Next(-100000, 100000) + hms.ScaleOffsetX;
				float scaleOffsetY = rng.Next(-100000, 100000) + hms.ScaleOffsetY;
				octaveOffsets[i] = new Vector2(scaleOffsetX, scaleOffsetY);
			}

			if (hms.Scale <= 0) hms.Scale = 0.0001f;

			//to scale into the center
			float halfWidth = hms.MapWidth / 2.0f;
			float halfHeight = hms.MapHeight / 2.0f;

			for (int y = 0; y < hms.MapHeight; y++)
			{
				for (int x = 0; x < hms.MapWidth; x++)
				{
					float amplitude = 1.0f;
					float frequency = 1.0f;
					float noiseHeight = 0.0f;

					for (int i = 0; i < hms.Octaves; i++)
					{
						float xCoord = (x - halfWidth) / hms.Scale * frequency + octaveOffsets[i].x;
						float yCoord = (y - halfHeight) / hms.Scale * frequency + octaveOffsets[i].y;

						//2.0f - 1.0f for negative perlin values 
						float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2.0f - 1.0f;
						noiseHeight += perlinValue * amplitude;

						amplitude *= hms.Persistance;
						frequency *= hms.Density;
					}

					if (noiseHeight < hms.OceanLevel)
						noiseHeight = hms.OceanLevel;

					if (noiseHeight > hms.CapMountainHeight)
						noiseHeight = hms.CapMountainHeight;

					if (noiseHeight > Output.MaxHeight)
						Output.MaxHeight = noiseHeight;
					else if (noiseHeight < Output.MinHeight)
						Output.MinHeight = noiseHeight;

					Output.HeightMapReal[x, y] = noiseHeight;
				}
			}

			//normalize noise map
			for (int y = 0; y < hms.MapHeight; y++)
			{
				for (int x = 0; x < hms.MapWidth; x++)
					Output.HeightMapNormalized[x, y] = Mathf.InverseLerp(Output.MinHeight, Output.MaxHeight, Output.HeightMapReal[x, y]);
			}

			return Output;
		}
	}
}