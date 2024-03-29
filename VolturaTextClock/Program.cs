using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Runtime;
using System.Windows.Forms;

namespace VolturaTextClock
{
    internal static class Program
    {
        internal static IConfiguration AppConfig;
        private static IConfiguration OriginalAppConfig;
        internal static string ConfigurationFile;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Log.Info = "===Starting application===";
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            SetGCSettings();
            SetupAppConfig();
            UpdateConfig();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new VolturaTextClockForm());
        }

        internal static void SetGCSettings()
        {
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GCSettings.LatencyMode = GCLatencyMode.Batch;
        }

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error = (Exception)e.ExceptionObject;
            Log.Close("=== Application ended ===");
            Environment.Exit(1);
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

        internal static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetRuntimeProperty(propName)?.GetValue(src);
        }
        internal static AppSettings GetConfig(string configFile)
        {
            string json = File.ReadAllText(configFile);
            Root root = JsonConvert.DeserializeObject<Root>(json);
            return root.AppSettings;
        }
    }

    internal static class ConfigExtensions
    {
        internal static T GetValue<T>(this IConfiguration configuration, string keyName, T valueIfMissing)
        {
            string configSection = "appSettings";
            ((IConfigurationRoot)configuration).Reload();
            if (configuration[$"{configSection}:{keyName}"] == null)
            {
                return valueIfMissing != null ? valueIfMissing : default;
            }
            return (T)Convert.ChangeType(configuration[$"{configSection}:{keyName}"], typeof(T));
        }

        internal static bool KeyExists(this IConfiguration configuration, string keyName)
        {
            string configSection = "appSettings";
            ((IConfigurationRoot)configuration).Reload();
            return configuration[$"{configSection}:{keyName}"] != null;
        }

#pragma warning disable IDE0060 // Remove unused parameter
        internal static void AddOrUpdateAppSetting<T>(this IConfiguration configuration, string keyName, T value)
#pragma warning restore IDE0060 // Remove unused parameter
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
