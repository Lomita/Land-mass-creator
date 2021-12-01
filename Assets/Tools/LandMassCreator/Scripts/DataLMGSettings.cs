using System.Runtime.Serialization;

namespace LandMassCreator
{
    [DataContract]
    public class DataLMGSettings
    {
        [DataMember(Name = "Height Map Settings")]
        private HeightMapSettings m_heightMapSettings = null;

        [DataMember(Name = "Auto Update")]
        private bool m_AutoUpdate;

        public DataLMGSettings(HeightMapSettings heightMapSettings, bool autoUpdate)
        {
            m_heightMapSettings = heightMapSettings;
            m_AutoUpdate = autoUpdate;
        }
    }
}
