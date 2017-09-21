using accouting_system_manager.DB;
using accouting_system_manager.License.Exceptions;
using accouting_system_manager.Log;
using cryptography;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accouting_system_manager.License
{
    public static class LicenseManager
    {
        const string REGISTRY_KEY_PATH = "Software\\ASMAN";
        const string REGISTRY_KEY_NAME = "KEYS";

        private static LicContent licContent;

        public static bool IsValid
        {
            get {

                if (licContent != null && licContent.validUntil < DateTime.Now)
                {
                    if (!IsLicenseInvalid())
                    {
                        invalidLicense();
                    }
                }

                return licContent is LicContent && licContent.IsDateValid && licContent.IsComputerExists && !IsLicenseInvalid();
            }
        }

        public static void Load(string content)
        {
            licContent = GetLicContent(content);
        }

        public static void Load()
        {
            DBManager.RunQueryResults("select TOP 1 content FROM license", (System.Data.SqlClient.SqlDataReader reader) => {
                licContent = GetLicContent(reader["content"].ToString());
            });
        }

        public static void Import(string path)
        {
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);

                try
                {
                    licContent = GetLicContent(content);

                    DBManager.ExecuteQuery("DELETE FROM license");
                    DBManager.ExecuteQuery(string.Format("INSERT INTO license(content) VALUES('{0}');", content));
                }
                catch(Exception e)
                {
                    LoggerSingleton.GetInstance.Error(e.Message);
                    throw new InvalidFileException();
                }
            }
            else
            {
                throw new InvalidFileException();
            }
        }

        private static bool IsLicenseInvalid()
        {
            return GetRegistryInvalidKeys().Split(',').Contains(licContent.id.ToString());
        }

        private static string GetRegistryInvalidKeys() {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(REGISTRY_KEY_PATH))
                {
                    if (key != null)
                    {
                        var o = key.GetValue(REGISTRY_KEY_NAME);

                        if (o != null && licContent != null)
                        {
                            return o.ToString();
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception) 
            {
                return string.Empty;
            }
        }

        private static void invalidLicense()
        {
            var keys = GetRegistryInvalidKeys();
            var allKeys = keys.Trim().Equals(string.Empty) ? string.Format("{0},", licContent.id.ToString()) : string.Format("{0},{1},", GetRegistryInvalidKeys(), licContent.id.ToString());

            RegistryKey registry = Registry.LocalMachine.OpenSubKey(REGISTRY_KEY_PATH, true);

            if (registry == null)
            {
                Registry.LocalMachine.CreateSubKey(REGISTRY_KEY_PATH);
                registry = Registry.LocalMachine.OpenSubKey(REGISTRY_KEY_PATH, true);
            }

            registry.SetValue(REGISTRY_KEY_NAME, allKeys);
        }

        private static LicContent GetLicContent(string cryptedContent)
        {
            string content = StringCipher.Decrypt(cryptedContent);

            var root = new System.Xml.Serialization.XmlRootAttribute("root");

            var ser = new System.Xml.Serialization.XmlSerializer(typeof(LicContent), root);

            using (var xmlContent = System.Xml.XmlReader.Create(new StringReader(content)))
            {
                try
                {
                    return (LicContent)ser.Deserialize(xmlContent);
                }
                catch(Exception e)
                {
                    return new LicContent();
                }
                
            }
        }
    }
}
