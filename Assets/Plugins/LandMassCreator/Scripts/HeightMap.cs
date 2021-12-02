namespace LandMassCreator
{
	/// <summary>
	/// Stores height map data
	/// </summary>
	public class HeightMap
	{
		/// <summary>
		/// Height map with real values 
		/// </summary>
		private float[,] m_heightMapReal = null;
		
		/// <summary>
		/// Normalized height map with values beetween 0 and 1.
		/// </summary>
		private float[,] m_heightMapNormalized = null;
		
		/// <summary>
		/// The biggest height value inside the height map 
		/// </summary>
		private float m_maxHeight = float.MinValue;
		
		/// <summary>
		/// The smallest height value inside the height map 
		/// </summary>
		private float m_minHeight = float.MaxValue;
	
        /// <summary>
        /// Gets or sets the real hight map
        /// </summary>
        public float[,] HeightMapReal { get => m_heightMapReal; set => m_heightMapReal = value; }
		
		/// <summary>
		/// Gets or sets the real hight map
		/// </summary>
		public float[,] HeightMapNormalized { get => m_heightMapNormalized; set => m_heightMapNormalized = value; }
        
		/// <summary>
		/// Gets or sets the maximum height
		/// </summary>
		public float MaxHeight { get => m_maxHeight; set => m_maxHeight = value; }
		
		/// <summary>
		/// Gets or sets the minimum height
		/// </summary>
		public float MinHeight { get => m_minHeight; set => m_minHeight = value; }

		/// <summary>
		/// Height map constructor
		/// </summary>
		/// <param name="heightMapReal">Height map with real values</param>
		/// <param name="heightMapNormalized">Height map with normalized values</param>
		public HeightMap(float[,] heightMapReal, float[,] heightMapNormalized)
		{
			HeightMapReal = heightMapReal;
			HeightMapNormalized = heightMapNormalized;
		}
	}
}