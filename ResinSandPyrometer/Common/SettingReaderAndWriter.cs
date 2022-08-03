using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Common
{
    /// <summary>
    /// 应用程序配置文件的读写
    /// </summary>
    public class SettingReaderAndWriter
    {
        /// <summary>
        /// 获取所有的AppSettings配置项
        /// </summary>
        /// <returns>所有的AppSettings配置项</returns>
        public static Dictionary<string, string> GetAllSettings()
        {
            Dictionary<string, string> allSettings = new Dictionary<string, string>();

            foreach (var key in ConfigurationManager.AppSettings.Keys)
            {
                allSettings.Add(key.ToString(), ConfigurationManager.AppSettings[key.ToString()]);
            }

            return allSettings;
        }

        /// <summary>
        /// 根据配置项的Key值删除配置项
        /// </summary>
        /// <param name="key">配置项的Key值</param>
        public static void DeleteAppSetting(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove(key);

            config.AppSettings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 添加配置项
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">值</param>
        public static void AddAppSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Add(key, value);

            config.AppSettings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 根据Key值写入程序配置项的值
        /// </summary>
        /// <param name="key">Key值</param>
        /// <param name="value">配置项的值</param>
        public static void WriteAppSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings[key].Value = value.ToString();

            config.AppSettings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 根据Key值读取配置项的值
        /// </summary>
        /// <param name="key">Key值</param>
        /// <returns>配置项的值</returns>
        public static string ReadAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
