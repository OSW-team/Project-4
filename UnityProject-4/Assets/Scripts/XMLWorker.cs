using System;
using UnityEngine;
using System.Collections;
using System.IO;
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

    public static void SaveSC(string workName)
    {
        if (!Directory.Exists("Data"))
        {
            Directory.CreateDirectory("Data");
        }
        if (!File.Exists("Data/SCsTest.xml"))
        {
            XmlDocument doc = new XmlDocument();
            XmlNode mainNode = doc.CreateElement("Items");
            doc.AppendChild(mainNode);

            XmlElement SCNode = doc.CreateElement("SC");
            SCNode.SetAttribute("WorkName", workName);
            mainNode.AppendChild(SCNode);
            doc.Save("Data/SCsTest.xml");
        }
    }

    public static void SaveModule(string citadelName, string moduleName)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Data/SCsTest.xml");
        XmlNodeList dataList = doc.GetElementsByTagName("SC");
        foreach (XmlNode node in dataList)
        {
            var atrs = node.Attributes;
            if (atrs["WorkName"].Value == citadelName)
            {
                XmlElement ModuleNode = doc.CreateElement("Module");
                ModuleNode.SetAttribute("WorkName", moduleName);
                node.AppendChild(ModuleNode);
            }
        }

        doc.Save("Data/SCsTest.xml");
    }

    public static void SaveSubsystem(string citadelName, string moduleName, string subsystemName)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Data/SCsTest.xml");
        XmlNodeList dataList = doc.GetElementsByTagName("SC");
        foreach (XmlNode node in dataList)
        {
            var atrs = node.Attributes;
            if (atrs["WorkName"].Value == citadelName)
            {
                dataList = node.ChildNodes;
            }
        }
        foreach (XmlNode module in dataList)
        {
            var atrs = module.Attributes;
            if (atrs["WorkName"].Value == moduleName)
            {
                XmlElement SubNode = doc.CreateElement("Sub");
                SubNode.SetAttribute("WorkName", subsystemName);
                module.AppendChild(SubNode);
            }

        }

        doc.Save("Data/SCsTest.xml");
    }

    public static void LoadModule(string workName, Module module)
    {
        CitadelParams = new XmlDocument();
        TextAsset xmlAsset = Resources.Load("Modules") as TextAsset;
        if (xmlAsset) CitadelParams.LoadXml(xmlAsset.text);
        XmlNodeList dataList = CitadelParams.GetElementsByTagName("Module");
        foreach (XmlNode node in dataList)
        {
            var atrs = node.Attributes;
            if (atrs["Workname"].Value == workName)
            {
                XmlNodeList list = node.ChildNodes;
                foreach (XmlNode item in list)
                {
                    if (item.InnerText != "")
                    {
                        switch (item.Name)
                        {
                            case "Workmame":
                                module.Workmame = item.InnerText;
                                break;
                            case "GameName":
                                module.GameName = item.InnerText;
                                break;
                            case "Description":
                                module.Description = item.InnerText;
                                break;
                            case "ModelName":
                                module.ModelName = item.InnerText;
                                break;
                            case "Mass":
                                module.Mass = Single.Parse(item.InnerText);
                                break;
                            case "Type":
                                module.Type = GameModelsAndEnums.GetModuleTypeByString(item.InnerText);
                                break;
                            case "MaxSlotAmount":
                                module.MaxSlotAmount = Int32.Parse(item.InnerText);
                                break;
                            case "CurrntSlotAmount":
                                module.CurrntSlotAmount = Int32.Parse(item.InnerText);
                                break;
                            case "MaxHP":
                                module.MaxHP = Single.Parse(item.InnerText);
                                break;
                            case "CurrentHP":
                                module.CurrentHP = Single.Parse(item.InnerText);
                                break;
                            case "Exhaust":
                                module.Exhaust = Single.Parse(item.InnerText);
                                break;
                            case "MalfunctionThreshold":
                                module.MalfunctionThreshold = GameModelsAndEnums.GetMalfunctionTreshold(item.InnerText);
                                break;
                            case "ExhaustSpeed":
                                module.ExhaustSpeed = Single.Parse(item.InnerText);
                                break;
                            case "PhysycReflectArmor":
                                module.PhysycReflectArmor = Single.Parse(item.InnerText);
                                break;
                            case "EnergyReflectArmor":
                                module.EnergyReflectArmor = Single.Parse(item.InnerText);
                                break;
                            case "HeatReflectArmor":
                                module.HeatReflectArmor = Single.Parse(item.InnerText);
                                break;
                            case "PhysycConsumArmor":
                                module.PhysycConsumArmor = Single.Parse(item.InnerText);
                                break;
                            case "EnergyConsumArmor":
                                module.EnergyConsumArmor = Single.Parse(item.InnerText);
                                break;
                            case "HeatConsumArmor":
                                module.HeatConsumArmor = Single.Parse(item.InnerText);
                                break;
                            case "Vulnerability":
                                module.Vulnerability = Single.Parse(item.InnerText);
                                break;
                            case "MetalCost":
                                module.MetalCost = Single.Parse(item.InnerText);
                                break;
                            case "EnergyCost":
                                module.EnergyCost = Single.Parse(item.InnerText);
                                break;
                            case "PurchaseRestrictions":
                                module.PurchaseRestrictions =
                                    GameModelsAndEnums.GetPurchaseRestrictionsByString(item.InnerText);
                                break;
                            case "RadarRange":
                                module.RadarRange = Single.Parse(item.InnerText);
                                break;
                            case "RadarAcc":
                                module.RadarAcc = Single.Parse(item.InnerText);
                                break;
                            case "MaxStorage":
                                module.MaxStorage = Single.Parse(item.InnerText);
                                break;
                            case "Force":
                                module.Force = Single.Parse(item.InnerText);
                                break;
                            case "Consumption":
                                module.Consumption = Single.Parse(item.InnerText);
                                break;
                            case "MaxControl":
                                module.MaxControl = Int32.Parse(item.InnerText);
                                break;
                            case "MaxShield":
                                module.MaxShield = Single.Parse(item.InnerText);
                                break;
                            case "PhysConShieldArm":
                                module.PhysConShieldArm = Single.Parse(item.InnerText);
                                break;
                            case "EnergyConShieldArm":
                                module.EnergyConShieldArm = Single.Parse(item.InnerText);
                                break;
                            case "HeatConShieldArm":
                                module.HeatConShieldArm = Single.Parse(item.InnerText);
                                break;
                            case "OnlineBaseShieldRegen":
                                module.OnlineBaseShieldRegen = Single.Parse(item.InnerText);
                                break;
                            case "OfflineBaseShieldRegen":
                                module.OfflineBaseShieldRegen = Single.Parse(item.InnerText);
                                break;
                            case "ShieldPower":
                                module.ShieldPower = Single.Parse(item.InnerText);
                                break;
                            case "ShieldTreshold":
                                module.ShieldTreshold = Single.Parse(item.InnerText);
                                break;
                            case "MaxSchemeAmount":
                                module.MaxSchemeAmount = Int32.Parse(item.InnerText);
                                break;
                            case "ConstructionSpeed":
                                module.ConstructionSpeed = Single.Parse(item.InnerText);
                                break;
                            case "MaxStorageParts":
                                module.MaxStorageParts = Int32.Parse(item.InnerText);
                                break;
                            case "CurrentStorageParts":
                                module.CurrentStorageParts = Int32.Parse(item.InnerText);
                                break;
                            case "ConversionSpeed":
                                module.ConversionSpeed = Single.Parse(item.InnerText);
                                break;
                            case "ConversionEff":
                                module.ConversionEff = Single.Parse(item.InnerText);
                                break;
                            case "ConversionReverseEff":
                                module.ConversionReverseEff = Single.Parse(item.InnerText);
                                break;
                            case "Potential":
                                module.Potential = Single.Parse(item.InnerText);
                                break;
                            case "Multyply":
                                module.Multyply = GameModelsAndEnums.GetMultyply(item.InnerText);
                                break;
                            case "Drain":
                                module.Drain = GameModelsAndEnums.GetDrain(item.InnerText);
                                break;
                        }
                    }

                }
                
            }
        }

    }

    public enum EnumGameItemType
    {
        Citadel,
        Modules
    }
}
