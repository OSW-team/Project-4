using UnityEngine;
using System.Collections;
using System.Xml;

public static class XMLWorker
{
    public static XmlDocument CitadelParams;
    // Use this for initialization
    public static string LoadItem(EnumGameItemType itemType, string itemName, string paramName)
    {
        var xmlPath = "";
        switch (itemType)
        {
            case EnumGameItemType.Modules:
                xmlPath = "Modules";
                break;
        }

        var res = "";
        CitadelParams = new XmlDocument();
        TextAsset xmlAsset = Resources.Load(xmlPath) as TextAsset;
        if (xmlAsset) CitadelParams.LoadXml(xmlAsset.text);
        XmlNodeList dataList = CitadelParams.GetElementsByTagName("item");
        foreach (XmlNode item in dataList)
        {
            var atrs = item.Attributes;
            if (atrs["Name"].Value == itemName)
            {
                dataList = item.ChildNodes;
            }
        }
        foreach (XmlNode item in dataList)
        {
            if (item.Name == paramName)
            {
                res = item.InnerText;
            }

        }

        return res;
    }

    public enum EnumGameItemType
    {
        Citadel,
        Modules
    }
}
