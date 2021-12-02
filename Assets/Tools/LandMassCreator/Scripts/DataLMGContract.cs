using System.Runtime.Serialization;
using UnityEngine;

namespace LandMassCreator
{
    /// <summary>
    /// Landmass Generator settings data contract
    /// </summary>
    [DataContract]
    public class DataLMGSettings
    {
        /// <summary>
        /// Auto updates all values if checked
        /// </summary>
        [DataMember(Name = "Auto Update", Order = 1)]
        private bool m_autoUpdate;

        /// <summary>
        /// Draw all vertices
        /// </summary>
        [DataMember(Name = "Show Vertices", Order = 2)]
        private bool m_drawGizmosVertices;

        /// <summary>
        /// Height map settings
        /// </summary>
        [DataMember(Name = "Height Map Settings", Order = 3)]
        private HeightMapSettings m_heightMapSettings;

        /// <summary>
        /// Color pellet of the vertices
        /// </summary>
        [DataMember(Name = "Vertex Colors", Order = 4)]
        private DataGradient m_vertexColors;

        /// <summary>
        /// DataLGMSettings Constructor 
        /// </summary>
        /// <param name="lgm">The landmass generator to be serialized</param>
        public DataLMGSettings(LandmassGenerator lgm)
        {
            m_heightMapSettings = lgm.Settings;
            m_autoUpdate = lgm.AutoUpdate;
            m_drawGizmosVertices = lgm.DrawGizmosVertices;
            m_vertexColors = new DataGradient();
            m_vertexColors.FromUnityGradient(lgm.ColorPalette);
        }

        public DataLMGSettings(HeightMapSettings heightMapSettings, bool autoUpdate, 
            bool drawGizmosVertices, DataGradient vertexColors)
        {
            m_heightMapSettings = heightMapSettings;
            m_autoUpdate = autoUpdate;
            m_drawGizmosVertices = drawGizmosVertices;
            m_vertexColors = vertexColors;
        }

        public bool AutoUpdate { get => m_autoUpdate; set => m_autoUpdate = value; }
        public bool DrawGizmosVertices { get => m_drawGizmosVertices; set => m_drawGizmosVertices = value; }
        public HeightMapSettings HeightMapSettings { get => m_heightMapSettings; set => m_heightMapSettings = value; }
        public DataGradient VertexColors { get => m_vertexColors; set => m_vertexColors = value; }
    }

    [DataContract]
    public class DataGradient
    {
        [DataMember(Name = "Gradient Mode", Order = 1)]
        private int m_mode;

        #region GradientAlphaKeys

        [DataMember(Name = "Alpha Keys Alpha", Order = 2)]
        private float[] m_alphaAlphaKeys;

        [DataMember(Name = "Alpha Keys Time", Order = 3)]
        private float[] m_timeAlphaKeys;

        #endregion

        #region GradientColorKeys

        [DataMember(Name = "Color Keys Color", Order = 4)]
        private DataColor[] m_colorColorKeys;

        [DataMember(Name = "Time Keys Color", Order = 5)]
        private float[] m_timeColorKeys;

        #endregion

        public DataGradient() { }

        public DataGradient(int mode, float[] alphaAlphaKeys, float[] timeAlphaKeys,
            DataColor[] colorColorKeys, float[] timeColorKeys)
        {
            m_mode = mode;
            m_alphaAlphaKeys = alphaAlphaKeys;
            m_timeAlphaKeys = timeAlphaKeys;
            m_colorColorKeys = colorColorKeys;
            m_timeColorKeys = timeColorKeys;
        }

        public int Mode { get => m_mode; set => m_mode = value; }
        public float[] AlphaAlphaKeys { get => m_alphaAlphaKeys; set => m_alphaAlphaKeys = value; }
        public float[] TimeAlphaKeys { get => m_timeAlphaKeys; set => m_timeAlphaKeys = value; }
        public DataColor[] ColorColorKeys { get => m_colorColorKeys; set => m_colorColorKeys = value; }
        public float[] TimeColorKeys { get => m_timeColorKeys; set => m_timeColorKeys = value; }

        public void FromUnityGradient(Gradient gradient)
        {
            m_mode = (int)gradient.mode;

            GradientAlphaKey[] alphaKeys = gradient.alphaKeys;
            GradientColorKey[] colorKeys = gradient.colorKeys;

            m_alphaAlphaKeys = new float[alphaKeys.Length];
            m_timeAlphaKeys = new float[alphaKeys.Length];

            for (int i = 0; i < alphaKeys.Length; i++)
            {
                m_alphaAlphaKeys[i] = alphaKeys[i].alpha;
                m_timeAlphaKeys[i] = alphaKeys[i].time;
            }

            m_colorColorKeys = new DataColor[colorKeys.Length];
            m_timeColorKeys = new float[colorKeys.Length];

            for (int i = 0; i < colorKeys.Length; i++)
            {
                m_colorColorKeys[i] = new DataColor(colorKeys[i].color);
                m_timeColorKeys[i] = colorKeys[i].time;
            }
        }

        public Gradient ConvertToUnityGradient()
        {
            Gradient vertexColors = new Gradient();
            
            GradientAlphaKey[] alphaKeys;
            GradientColorKey[] colorKeys;

            if (m_alphaAlphaKeys.Length.Equals(m_timeAlphaKeys.Length))
            {
                alphaKeys = new GradientAlphaKey[m_alphaAlphaKeys.Length];

                for (int i = 0; i < m_alphaAlphaKeys.Length; i++)
                {
                    alphaKeys[i].alpha = m_alphaAlphaKeys[i];
                    alphaKeys[i].time = m_timeAlphaKeys[i];
                }
            }
            else
            {
                Debug.LogError("Could not load Alpha Keys, arrays are not equal!");
                alphaKeys = vertexColors.alphaKeys;
            }

            if (m_colorColorKeys.Length.Equals(m_timeColorKeys.Length))
            {
                colorKeys = new GradientColorKey[m_colorColorKeys.Length];

                for (int i = 0; i < m_colorColorKeys.Length; i++)
                {
                    colorKeys[i].color = m_colorColorKeys[i].RGBA;
                    colorKeys[i].time = m_timeColorKeys[i];
                }
            }
            else 
            {
                Debug.LogError("Could not load Color Keys, arrays are not equal!");
                colorKeys = vertexColors.colorKeys;
            }

            vertexColors.SetKeys(colorKeys, alphaKeys);
            return vertexColors;
        }
    }

    [DataContract]
    public class DataColor
    {
        [DataMember(Name = "Red")]
        private float m_r;
        
        [DataMember(Name = "Green")]
        private float m_g;

        [DataMember(Name = "Blue")]
        private float m_b;

        [DataMember(Name = "Alpha")]
        private float m_a;

        public DataColor(Color color)
        {
            m_r = color.r;
            m_g = color.g;
            m_b = color.b;
            m_a = color.a;
        }

        public DataColor(float r, float g, float b, float a)
        {
            m_r = r;
            m_g = g;
            m_b = b;
            m_a = a;
        }

        public Color RGBA 
        {
            get => new Color(m_r, m_g, m_b, m_a);
        }
    }
}