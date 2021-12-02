using System;
using System.Runtime.Serialization;

namespace LandMassCreator
{
	/// <summary>
	/// Stores the necessary settings to create a height map
	/// </summary>
	[DataContract]
	public class HeightMapSettings
	{
		/// <summary>
		/// The map width 
		/// </summary>
		[DataMember(Name = "Map Width")]
		private int m_mapWidth = 200;

		/// <summary>
		/// The map Height
		/// </summary>
		[DataMember(Name = "Map Height")]
		private int m_mapHeight = 200;

		/// <summary>
		/// Random map seed
		/// </summary>
		[DataMember(Name = "Seed")]
		private int m_seed = 0;

		/// <summary>
		/// Noise scale
		/// </summary>
		[DataMember(Name = "Noise Scale")]
		private float m_scale = 4.0f;

		/// <summary>
		/// Noise offset X axis
		/// </summary>
		[DataMember(Name = "Scale Offset X")]
		private float m_scaleOffsetX = 0.0f;

		/// <summary>
		/// Noise offset Y axis
		/// </summary>
		[DataMember(Name = "Scale Offset Y")]
		private float m_scaleOffsetY = 0.0f;

		/// <summary>
		/// The amount of noise iterations 
		/// </summary>
		[DataMember(Name = "Octaves")]
		private int m_octaves = 6;

		/// <summary>
		/// The persistance of equal noise values
		/// </summary>
		[DataMember(Name = "Persistance")]
		private float m_persistance = 2.0f;

		/// <summary>
		/// The density of equal noise values
		/// </summary>
		[DataMember(Name = "Density")]
		private float m_density = 0.45f;

		/// <summary>
		/// Cap minimal values
		/// </summary>
		[DataMember(Name = "Ocean Level")]
		private float m_oceanLevel = 0.0f;

		/// <summary>
		/// Cap maximal values
		/// </summary>
		[DataMember(Name = "Max MountainHeight")]
		private float m_CapMountainHeight = 200.0f;

		/// <summary>
		/// Get and set the map width
		/// </summary>
		public int MapWidth 
		{ 
			get => m_mapWidth; 
			set => m_mapWidth = value.Clamp(2, 200);
		}

        /// <summary>
        /// Get and set the map height
        /// </summary> 
        public int MapHeight 
		{ 
			get => m_mapHeight; 
			set => m_mapHeight = value.Clamp(2, 200); 
		}
		
		/// <summary>
		/// Get and set map seed
		/// </summary>
		public int Seed { get => m_seed; set => m_seed = value; }

		/// <summary>
		/// Get and set noise scale
		/// </summary>
		public float Scale { get => m_scale; set => m_scale = value; }

		/// <summary>
		/// Get and set noise offset x
		/// </summary>
		public float ScaleOffsetX { get => m_scaleOffsetX; set => m_scaleOffsetX = value; }

		/// <summary>
		/// Get and set noise offset y
		/// </summary>
		public float ScaleOffsetY { get => m_scaleOffsetY; set => m_scaleOffsetY = value; }

		/// <summary>
		/// Get and set octaves
		/// </summary>
		public int Octaves 
		{ 
			get => m_octaves; 
			set => m_octaves = value.Clamp(1, 15); 
		}

		/// <summary>
		/// Get and set persistance
		/// </summary>
		public float Persistance 
		{ 
			get => m_persistance; 
			set => m_persistance = value.Clamp(0.0f, 5.0f); 
		}

		/// <summary>
		/// Get and set density
		/// </summary>
		public float Density 
		{ 
			get => m_density; 
			set => m_density = value.Clamp(0.0f, 1.0f); 
		}

		/// <summary>
		/// Get and set ocean level
		/// </summary>
		public float OceanLevel { get => m_oceanLevel; set => m_oceanLevel = value; }

		/// <summary>
		/// Get and set Mountain Height cap
		/// </summary>
		public float CapMountainHeight { get => m_CapMountainHeight; set => m_CapMountainHeight = value; }

		/// <summary>
		/// HeightMapSettings Constructor
		/// </summary>
		public HeightMapSettings() {}

		/// <summary>
		/// HeightMapSettings Constructor
		/// </summary>
		/// <param name="mapWidth">Map width</param>
		/// <param name="mapHeight">Map height</param>
		/// <param name="seed">Map seed</param>
		/// <param name="scale">Noise scale</param>
		/// <param name="scaleOffsetX">Noise offset x axis</param>
		/// <param name="scaleOffsetY">Noise offset y axis</param>
		/// <param name="octaves">The amount of noise iterations </param>
		/// <param name="persistance">The persistance of equal noise values</param>
		/// <param name="density">The density of equal noise values</param>
		/// <param name="oceanLevel">Cap minimal values</param>
		/// <param name="capMountainHeight">Cap maximal values</param>
		public HeightMapSettings(int mapWidth, int mapHeight, int seed, 
			float scale, float scaleOffsetX, float scaleOffsetY, int octaves, 
			float persistance, float density, float oceanLevel, float capMountainHeight)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            Seed = seed;
            Scale = scale;
            ScaleOffsetX = scaleOffsetX;
            ScaleOffsetY = scaleOffsetY;
            Octaves = octaves;
            Persistance = persistance;
            Density = density;
            OceanLevel = oceanLevel;
            CapMountainHeight = capMountainHeight;
        }
    }
}