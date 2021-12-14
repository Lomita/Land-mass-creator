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

        /// <summary>
        /// DataLMGSettings constructor
        /// </summary>
        /// <param name="heightMapSettings">Height map settings</param>
        /// <param name="autoUpdate">Auto update inspector settings</param>
        /// <param name="drawGizmosVertices">Show vertices</param>
        /// <param name="vertexColors">Vertex color data gradient</param>
        public DataLMGSettings(HeightMapSettings heightMapSettings, bool autoUpdate, 
            bool drawGizmosVertices, DataGradient vertexColors)
        {
            m_heightMapSettings = heightMapSettings;
            m_autoUpdate = autoUpdate;
            m_drawGizmosVertices = drawGizmosVertices;
            m_vertexColors = vertexColors;
        }

        /// <summary>
        /// Gets and sets auto update
        /// </summary>
        public bool AutoUpdate { get => m_autoUpdate; set => m_autoUpdate = value; }

        /// <summary>
        /// Gets and sets draw vertices
        /// </summary>
        public bool DrawGizmosVertices { get => m_drawGizmosVertices; set => m_drawGizmosVertices = value; }

        /// <summary>
        /// Gets and sets the height map settings
        /// </summary>
        public HeightMapSettings HeightMapSettings { get => m_heightMapSettings; set => m_heightMapSettings = value; }

        /// <summary>
        /// Gets and sets the vertex colors
        /// </summary>
        public DataGradient VertexColors { get => m_vertexColors; set => m_vertexColors = value; }
    }

    /// <summary>
    /// Gradient data contract
    /// </summary>
    [DataContract]
    public class DataGradient
    {
        /// <summary>
        /// Gradient mode Blend = 0 and fixed = 1 see UnityEngine.GradientMode
        /// </summary>
        [DataMember(Name = "Gradient Mode", Order = 1)]
        private int m_mode;

        #region GradientAlphaKeys

        /// <summary>
        /// Gradient Alpha alpha keys see GradientAlphaKey.alpha
        /// </summary>
        [DataMember(Name = "Alpha Keys Alpha", Order = 2)]
        private float[] m_alphaAlphaKeys;

        /// <summary>
        /// Gradient Alpha time keys see GradientAlphaKey.time
        /// </summary>
        [DataMember(Name = "Alpha Keys Time", Order = 3)]
        private float[] m_timeAlphaKeys;

        #endregion

        #region GradientColorKeys

        /// <summary>
        /// Gradient Color color keys see GradientColorKeys.color
        /// </summary>
        [DataMember(Name = "Color Keys Color", Order = 4)]
        private DataColor[] m_colorColorKeys;

        /// <summary>
        /// Gradient Color time keys see GradientColorKeys.time
        /// </summary>
        [DataMember(Name = "Time Keys Color", Order = 5)]
        private float[] m_timeColorKeys;

        #endregion

        /// <summary>
        /// Data Gradient constructor
        /// </summary>
        public DataGradient() { }

        /// <summary>
        /// Data Gradient constructor
        /// </summary>
        /// <param name="mode">Gradient mode Blend = 0 and fixed = 1 see UnityEngine.GradientMode</param>
        /// <param name="alphaAlphaKeys">Gradient Alpha alpha keys see UnityEngine.GradientAlphaKey.alpha</param>
        /// <param name="timeAlphaKeys">Gradient Alpha time keys see UnityEngine.GradientAlphaKey.time</param>
        /// <param name="colorColorKeys">Gradient Color color keys see UnityEngine.GradientColorKeys.color</param>
        /// <param name="timeColorKeys">Gradient Color time keys see UnityEngine.GradientColorKeys.time</param>
        public DataGradient(int mode, float[] alphaAlphaKeys, float[] timeAlphaKeys,
            DataColor[] colorColorKeys, float[] timeColorKeys)
        {
            m_mode = mode;
            m_alphaAlphaKeys = alphaAlphaKeys;
            m_timeAlphaKeys = timeAlphaKeys;
            m_colorColorKeys = colorColorKeys;
            m_timeColorKeys = timeColorKeys;
        }

        /// <summary>
        /// Gets and sets the gradient mode Blend = 0 and fixed = 1 see UnityEngine.GradientMode
        /// </summary>
        public int Mode { get => m_mode; set => m_mode = value; }

        /// <summary>
        /// Gets and sets the gradient Alpha alpha keys see UnityEngine.GradientAlphaKey.alpha
        /// </summary>
        public float[] AlphaAlphaKeys { get => m_alphaAlphaKeys; set => m_alphaAlphaKeys = value; }

        /// <summary>
        /// Gets and sets the gradient Alpha time keys see UnityEngine.GradientAlphaKey.time
        /// </summary>
        public float[] TimeAlphaKeys { get => m_timeAlphaKeys; set => m_timeAlphaKeys = value; }

        /// <summary>
        /// Gets and sets the gradient Color color keys see UnityEngine.GradientColorKeys.color
        /// </summary>
        public DataColor[] ColorColorKeys { get => m_colorColorKeys; set => m_colorColorKeys = value; }

        /// <summary>
        /// Gets and sets the gradient Color time keys see UnityEngine.GradientColorKeys.time
        /// </summary>
        public float[] TimeColorKeys { get => m_timeColorKeys; set => m_timeColorKeys = value; }

        /// <summary>
        /// Converts a Unity Gradient to this DataGradient
        /// </summary>
        /// <param name="gradient">Gradient to convert</param>
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

        /// <summary>
        /// Convert this DataGradient to a Unity Gradient
        /// </summary>
        /// <returns></returns>
        public Gradient ConvertToUnityGradient()
        {
            Gradient vertexColors = new Gradient();
            vertexColors.mode = (GradientMode)m_mode;

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

    /// <summary>
    /// Color data contract
    /// </summary>
    [DataContract]
    public class DataColor
    {
        /// <summary>
        /// Red color channel
        /// </summary>
        [DataMember(Name = "Red")]
        private float m_r;
        
        /// <summary>
        /// Green color channel
        /// </summary>
        [DataMember(Name = "Green")]
        private float m_g;

        /// <summary>
        /// Blue color channel
        /// </summary>
        [DataMember(Name = "Blue")]
        private float m_b;

        /// <summary>
        /// Alpha color channel
        /// </summary>
        [DataMember(Name = "Alpha")]
        private float m_a;

        /// <summary>
        /// Data Color constructor for unity colors
        /// </summary>
        /// <param name="color">Color to save in the data contract</param>
        public DataColor(Color color)
        {
            m_r = color.r;
            m_g = color.g;
            m_b = color.b;
            m_a = color.a;
        }

        /// <summary>
        /// Data Color constructor for raw rgba values
        /// </summary>
        /// <param name="r">Red color channel</param>
        /// <param name="g">Green color channel</param>
        /// <param name="b">Blue color channel</param>
        /// <param name="a">Alpha color channel</param>
        public DataColor(float r, float g, float b, float a)
        {
            m_r = r;
            m_g = g;
            m_b = b;
            m_a = a;
        }

        /// <summary>
        /// Gets the color as unity color
        /// </summary>
        public Color RGBA 
        {
            get => new Color(m_r, m_g, m_b, m_a);
        }
    }
}