using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using UnityEngine;

namespace LandMassCreator
{
    /// <summary>
    /// Holds the possible export and import status
    /// </summary>
    public enum Status
    {
        SUCCESS = 0x1,
        CANCEL = 0x2,
        FAILED = 0x4,
        UNKNOWN = 0x8
    }

    /// <summary>
    /// Holds the import and export status and error msg
    /// </summary>
    public struct PortState
    {
        /// <summary>
        /// Represents the export or import port state
        /// </summary>
        /// <param name="status">Import or export status</param>
        /// <param name="msg">Import or export status message</param>
        /// <param name="path">Import or export path</param>
        public PortState(Status status, string msg = "", string path = "") : this()
        {
            m_status = status;
            m_msg = msg;
            m_path = path;
        }

        /// <summary>
        /// Import or export status
        /// </summary>
        private Status m_status;

        /// <summary>
        /// Import or export status message
        /// </summary>
        private string m_msg;

        /// <summary>
        /// export or import path
        /// </summary>
        private string m_path;

        /// <summary>
        /// Gets and sets import or export status
        /// </summary>
        public Status Status { get => m_status; set => m_status = value; }

        /// <summary>
        /// Gets and sets import or export message
        /// </summary>
        public string Msg { get => m_msg; set => m_msg = value; }
        
        /// <summary>
        /// Gets or sets import or export path
        /// </summary>
        public string Path { get => m_path; set => m_path = value; }
    }

    /// <summary>
    /// Editor utility functions
    /// </summary>
    public class EditorUtils
    {
        /// <summary>
        /// json encoding
        /// </summary>
        private static readonly Encoding m_encoding = Encoding.UTF8;
        
        /// <summary>
        /// Standard import and export path
        /// </summary>
        private static readonly string m_standardPortPath = Application.persistentDataPath;

        /// <summary>
        /// Gets the standard export or import path
        /// </summary>
        public static string StandardPortPath => m_standardPortPath;

        /// <summary>
        /// Export terrain settings
        /// </summary>
        /// <param name="lmg">Landmass generator to export</param>
        /// <param name="exportFilePath">Export file path</param>
        /// <returns>Return the export state</returns>
        public static PortState ExportTerrainSettings(LandmassGenerator lmg, string exportFilePath)
        {
            if (string.IsNullOrEmpty(exportFilePath))
                return new PortState(Status.CANCEL);

            DataLMGSettings dataLGM = new DataLMGSettings(lmg);

            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DataLMGSettings));
                FileStream fileStream = new FileStream(exportFilePath, FileMode.Create);
                XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(fileStream, m_encoding, true, true, "  ");

                serializer.WriteObject(writer, dataLGM);
                
                writer.Flush();
                fileStream.Close();

                if(!File.Exists(exportFilePath))
                    throw new Exception("Failed to export settings");
            }
            catch (Exception exception)
            {
                return new PortState(Status.FAILED, exception.ToString(), exportFilePath);
            }

            return new PortState(Status.SUCCESS, "", Path.GetDirectoryName(exportFilePath));
        }

        /// <summary>
        /// Import terrain settings
        /// </summary>
        /// <param name="lmg">Landmass generator to overwrite</param>
        /// <param name="importPath">Import file path</param>
        /// <returns>Return the import state</returns>
        public static PortState ImportTerrainSettings(LandmassGenerator lmg, string importPath)
        {
            if (string.IsNullOrEmpty(importPath))
                return new PortState(Status.CANCEL);
    
            try
            {
                if(!File.Exists(importPath))
                    throw new Exception("File " + importPath + " doesnt exist.");

                DataLMGSettings serLMG;
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DataLMGSettings));
                FileStream fileStream = new FileStream(importPath, FileMode.Open);
                serLMG = (DataLMGSettings)serializer.ReadObject(fileStream);
                if (serLMG == null) 
                    throw new Exception("Failed to deserialize settings!");

                lmg.Settings = serLMG.HeightMapSettings;
                lmg.AutoUpdate = serLMG.AutoUpdate;
                lmg.DrawGizmosVertices = serLMG.DrawGizmosVertices;
                lmg.ColorPalette = serLMG.VertexColors.ConvertToUnityGradient();
            }
            catch (Exception exception)
            {
                return new PortState(Status.FAILED, exception.ToString(), importPath);
            }

            return new PortState(Status.SUCCESS, "", importPath);
        }
    }
}