using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace VolturaTextClock
{
    internal static class Program
    {
        public static IConfiguration AppConfig;
        private static IConfiguration OriginalAppConfig;
        public static string ConfigurationFile;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Log.Init();
            SetupAppConfig();
            UpdateConfig();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Log.Info = "Starting form...";
            Application.Run(new VolturaTextClockForm());
        }

        private static void UpdateConfig()
        {
            StartWithWindows.Active = AppConfig.GetValue("autoStart", false);
        }
        private static void SetupAppConfig()
        {
            string sourceAppConfigJsonFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Properties\appsettings.json");
            ConfigurationFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "VolturaTextClockAppsettings.json");
            bool skipCheck = false;
            if (!File.Exists(ConfigurationFile))
            {
                File.Copy(sourceAppConfigJsonFile, ConfigurationFile, true);
                skipCheck = true;
            }
            AppConfig = new ConfigurationBuilder()
                .AddJsonFile(ConfigurationFile, true, true)
                .Build();
            if (!skipCheck)
            {
                OriginalAppConfig = new ConfigurationBuilder()
                    .AddJsonFile(sourceAppConfigJsonFile, true, true)
                    .Build();
                AppSettings sourceConfig = GetConfig(sourceAppConfigJsonFile);
                PropertyInfo[] properties = typeof(AppSettings).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (OriginalAppConfig.KeyExists(property.Name) && AppConfig.KeyExists(property.Name) == false)
                    {
                        object theValue = GetPropValue(sourceConfig, property.Name);
                        // found a appsetting property that does not exist in user settings => add it
                        Type myType = Type.GetType(property.PropertyType.FullName.ToString(), false);
                        Convert.ChangeType(theValue, myType);
                        AppConfig.AddOrUpdateAppSetting(property.Name, theValue);
                    }
                }
                OriginalAppConfig = null;
            }
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetRuntimeProperty(propName)?.GetValue(src);
        }
        public static AppSettings GetConfig(string configFile)
        {
            string json = File.ReadAllText(configFile);
            Root root = JsonConvert.DeserializeObject<Root>(json);
            return root.appSettings;
        }
    }

    public static class ConfigExtensions
    {
        public static T GetValue<T>(this IConfiguration configuration, string keyName, T valueIfMissing)
        {
            string configSection = "appSettings";
            ((IConfigurationRoot)configuration).Reload();
            if (configuration[$"{configSection}:{keyName}"] == null)
            {
                return valueIfMissing != null ? valueIfMissing : default;
            }
            return (T)Convert.ChangeType(configuration[$"{configSection}:{keyName}"], typeof(T));
        }

        public static bool KeyExists(this IConfiguration configuration, string keyName)
        {
            string configSection = "appSettings";
            ((IConfigurationRoot)configuration).Reload();
            return configuration[$"{configSection}:{keyName}"] != null;
        }

        public static void AddOrUpdateAppSetting<T>(this IConfiguration configuration, string keyName, T value)
        {
            string configSection = "appSettings";
            string sectionPathKey = $"{configSection}:{keyName}";
            try
            {
                string json = File.ReadAllText(Program.ConfigurationFile);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                SetValueRecursively(sectionPathKey, jsonObj, value);
                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(Program.ConfigurationFile, output);
            }
            catch (Exception ex)
            {
                Log.Error = ex;
            }
            return;
        }

        private static void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObj, T value)
        {
            // split the string at the first ':' character
            string[] remainingSections = sectionPathKey.Split(":", 2);

            string currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                // continue with the process, moving down the tree
                string nextSection = remainingSections[1];
                jsonObj[currentSection] ??= new JObject();
                SetValueRecursively(nextSection, jsonObj[currentSection], value);
            }
            else
            {
                // we've got to the end of the tree, set the value
                if (jsonObj[currentSection] != null)
                {
                    jsonObj[currentSection] = value;
                }
                else
                {
                    JObject jObject = jsonObj;
                    jObject.Add(new JProperty(currentSection, value));
                }
            }
        }
    }
}