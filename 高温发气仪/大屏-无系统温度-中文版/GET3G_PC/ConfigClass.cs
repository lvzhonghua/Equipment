using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace GET3G_PC
{
    public class ConfigClass
    {
        public string strFileName;
        public string configName;
        public string configValue;
        public ConfigClass(){ }

         public void SetConfigName(string strConfigName)
        {
            configName = strConfigName;
            strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString()+configName;    //获得配置文件的全路径
        }

         public void SaveConfig(string configkey, string configValue)
         {
             XmlDocument doc = new XmlDocument();
             doc.Load(strFileName);
             //找出名为"add"的所有元素
             XmlNodeList nodes = doc.GetElementsByTagName("add");
             for (int i = 0; i < nodes.Count; i++)
             {
                 //获得当前元素的key属性
                 XmlAttribute att = nodes[i].Attributes["key"];
                 if (att.Value == "" + configkey + "")
                 {
                     att = nodes[i].Attributes["value"];
                     att.Value = configValue;
                     break;
                 }
             }
             doc.Save(strFileName);

         }
    }
}
