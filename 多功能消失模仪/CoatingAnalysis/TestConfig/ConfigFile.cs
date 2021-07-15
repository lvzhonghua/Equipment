using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CoatingAnalysis
{
    public class ConfigFile
    {
        private string filePath;

        //构造函数，确定xml文件的路径
        public ConfigFile(string programName)            //programName就是namespace后的项目名称
        {
            filePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + programName + ".exe.config";//获得配置文件的全路径  
        }

        //保存key元素为对应的字符串
        public void SaveConfig(string configKey, string configValue)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            //找出名为"add"的所有元素
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得当前元素的key属性
                XmlAttribute att = nodes[i].Attributes["key"];
                if (att.Value == "" + configKey + "")
                {
                    att = nodes[i].Attributes["value"];
                    att.Value = configValue;
                    break;
                }
            }
            doc.Save(filePath);
        }

        //读取对应key的字符串值
        public string ReadConfig(string configKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            //找出名为"add"的所有元素
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得当前元素的key属性
                XmlAttribute att = nodes[i].Attributes["key"];
                if (att.Value == "" + configKey + "")
                {
                    att = nodes[i].Attributes["value"];
                    return att.Value;
                }
            }
            return string.Empty;
        }
    }
}
