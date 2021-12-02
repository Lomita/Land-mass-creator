using SFB;
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
        SUCCESS,
        CANCEL,
        FAILED,
        UNKNOWN
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
        /// Export terrain settings
        /// </summary>
        /// <param name="lmg"></param>
        /// <returns>Return the export state<</returns>
        public static PortState ExportTerrainSettings(LandmassGenerator lmg)
        {
            string exportFilePath = StandaloneFileBrowser.SaveFilePanel("Select export directory", m_standardPortPath, "terrain", "lmg");
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
            }
            catch (Exception exception)
            {
                return new PortState(Status.FAILED, exception.ToString(), exportFilePath);
            }

            return new PortState(Status.SUCCESS, "", exportFilePath);
        }

        /// <summary>
        /// Import terrain settings
        /// </summary>
        /// <returns>Return the import state</returns>
        public static PortState ImportTerrainSettings(LandmassGenerator lmg)
        {
            string[] importFile = StandaloneFileBrowser.OpenFilePanel("Select terrain settings for import", m_standardPortPath, "lmg", false);
            if (importFile.Equals(null) || importFile.Length.Equals(0) || string.IsNullOrEmpty(importFile[0]))
                return new PortState(Status.CANCEL);

            DataLMGSettings serLMG = null;
            
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DataLMGSettings));
                FileStream fileStream = new FileStream(importFile[0], FileMode.Open);
                serLMG = (DataLMGSettings)serializer.ReadObject(fileStream);
                if (serLMG == null) 
                    throw new Exception();
            }
            catch (Exception exception)
            {
                return new PortState(Status.FAILED, exception.ToString(), importFile[0]);
            }

            lmg.Settings = serLMG.HeightMapSettings;
            lmg.AutoUpdate = serLMG.AutoUpdate;
            lmg.DrawGizmosVertices = serLMG.DrawGizmosVertices;
            lmg.ColorPalette = serLMG.VertexColors.ConvertToUnityGradient();
              
            /*
            string json = "";
            FileStream fileStream = new FileStream(importFile[0], FileMode.Open);
            bool success = ReadFileStream(fileStream, out json);          
            fileStream.Close();


            if (string.IsNullOrEmpty(json))
                return new PortState(Status.FAILED, "Could not read terrain settings");

            HeightMapSettings heightMapSettings = new HeightMapSettings();
            JsonUtility.FromJsonOverwrite(json, heightMapSettings);
            lmg.Settings = heightMapSettings;
            */

            return new PortState(Status.SUCCESS, "", importFile[0]);
        }

        /// <summary>
        /// Write a string to filestream
        /// </summary>
        /// <param name="write">The string that will be written to the data stream</param>
        /// <param name="fileStream">The filestream to write to</param>
        /// <returns>Returns true if successful</returns>
        private static bool WriteToFileStream(string write, FileStream fileStream)
        {
            if (write.Length > 0)
            {
                byte[] bytes = m_encoding.GetBytes(write);
                fileStream.Write(bytes, 0, bytes.Length);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Read a filestream to content string
        /// </summary>
        /// <param name="fileStream">The filestream to read from</param>
        /// <param name="content">Stores the contntent from the filestream</param>
        /// <returns></returns>
        private static bool ReadFileStream(FileStream fileStream, out string content)
        {
            content = "";
            if (fileStream.Length > 0)
            {
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, (int)fileStream.Length);
                content = m_encoding.GetString(bytes);

                return true;
            }

            return false;
        }
    }
}