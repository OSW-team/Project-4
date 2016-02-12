using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public static void SaveSC(SteamCitadel citadel)
    {
        if (!Directory.Exists("Data"))
        {
            Directory.CreateDirectory("Data");
        }
        XmlDocument doc = new XmlDocument();
        if (!File.Exists("Data/SCsTest.xml"))
        {
            SaveSCItem(citadel.Name);
        }
        else
        {
            doc.Load("Data/SCsTest.xml");
            XmlNodeList scs = doc.GetElementsByTagName("SC");
            foreach (XmlNode sc in scs.Cast<XmlNode>().Where(sc => sc.Attributes["WorkName"].Value == citadel.Name))
            {
                sc.RemoveAll();
                doc.Save("Data/SCsTest.xml");
            }
           
        }
        SaveSCItem(citadel.Name);
        //Save Units
        foreach (var unit in citadel.Units)
        {
            var upgradesList = unit.Upgrades.Select(upgr => upgr.Workname).ToList();
            SaveUnit(citadel.Name, unit.Workname, upgradesList);
        }
        //Save Modules and Subs
        foreach (var module in citadel.Modules)
        {
            SaveModule(citadel.Name,module.Workname,module.Slot);
            foreach (var subsystem in module.Subs)
            {
                SaveSubsystem(citadel.Name,module.Workname,subsystem.Workname,subsystem.Slot);
            }
        }
    }

    public static void SaveSCItem(string workName)
    {
            XmlDocument doc = new XmlDocument();
            XmlNode mainNode = doc.CreateElement("Items");
            doc.AppendChild(mainNode);

            XmlElement SCNode = doc.CreateElement("SC");
            SCNode.SetAttribute("WorkName", workName);
            mainNode.AppendChild(SCNode);
            doc.Save("Data/SCsTest.xml");
    }

    public static void LoadSC(SteamCitadel citadel)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Data/SCsTest.xml");
        XmlNodeList dataList = doc.GetElementsByTagName("SC");
        foreach (XmlNode node in dataList)
        {
            var atrs = node.Attributes;
            if (atrs["WorkName"].Value == citadel.Name)
            {
                XmlNodeList modules = node.ChildNodes;
                foreach (XmlElement item in modules)
                {
                    if (item.Name == "Module")
                    {
                        var moduleSlot = "";
                        var subs = new List<XmlNode>();
                        foreach (XmlNode childNode in item.ChildNodes)
                        {
                            if (childNode.Name == "Slot") moduleSlot = childNode.Attributes["Name"].Value;
                            if (childNode.Name == "Sub") subs.Add(childNode);
                        }
                        var newModule = new Module(citadel.Name, item.Attributes["WorkName"].Value, moduleSlot);
                        citadel.Modules.Add(newModule);
                        foreach (XmlNode sub in subs)
                        {
                            var subSlot = "";
                            foreach (XmlNode childNode in sub)
                            {
                                if (childNode.Name == "Slot") subSlot = childNode.Attributes["Name"].Value;
                            }
                            var newSub = new Subsystem(citadel.Name, item.Attributes["WorkName"].Value,
                                sub.Attributes["WorkName"].Value, subSlot);
                            newModule.Subs.Add(newSub);
                        }
                    }
                    if (item.Name == "Unit")
                    {
                        var upgrades = new List<XmlNode>();
                        foreach (XmlNode childNode in item.ChildNodes)
                        {
                            if (childNode.Name == "Upgrade") upgrades.Add(childNode);
                        }
                        var newUnit = new Unit(citadel.Name, item.Attributes["WorkName"].Value);
                        citadel.Units.Add(newUnit);
                        foreach (XmlNode upg in upgrades)
                        {
                            var newUpg = new UnitUpgrade(citadel.Name, item.Attributes["WorkName"].Value,upg.Attributes["Name"].Value);
                            newUnit.Upgrades.Add(newUpg);
                        }

                    }
                }
            }
        }
    }


    public static void SaveModule(string citadelName, string moduleName, string slot)
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
                XmlElement Slot = doc.CreateElement("Slot");
                Slot.SetAttribute("Name", slot);
                ModuleNode.AppendChild(Slot);
            }
        }

        doc.Save("Data/SCsTest.xml");
    }

    public static void SaveSubsystem(string citadelName, string moduleName, string subsystemName, string slot)
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
                XmlElement Slot = doc.CreateElement("Slot");
                Slot.SetAttribute("Name", slot);
                SubNode.AppendChild(Slot);
            }

        }

        doc.Save("Data/SCsTest.xml");
    }

    public static void SaveUnit(string citadelName, string unitName, List<string> upgradesNames)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Data/SCsTest.xml");
        XmlNodeList dataList = doc.GetElementsByTagName("SC");
        foreach (XmlNode node in dataList)
        {
            var atrs = node.Attributes;
            if (atrs["WorkName"].Value == citadelName)
            {
                XmlElement UnitNode = doc.CreateElement("Unit");
                UnitNode.SetAttribute("WorkName", unitName);
                node.AppendChild(UnitNode);
                for (var i = 0; i < upgradesNames.Count; i++)
                {
                    XmlElement Upgrade = doc.CreateElement("Upgrade");
                    Upgrade.SetAttribute("Name", upgradesNames[i]);
                    UnitNode.AppendChild(Upgrade);
                }
            }
        }
        doc.Save("Data/SCsTest.xml");
    }

    public static void LoadModuleOrSub(string workName, Module module)
    {
        CitadelParams = new XmlDocument();
        var xmlPath = "Modules";
        var itemType = "Module";
        if (module.GetType() == typeof (Subsystem))
        {
            xmlPath = "Subsystems";
            itemType = "Subsystem";
        }
        TextAsset xmlAsset = Resources.Load(xmlPath) as TextAsset;
        if (xmlAsset) CitadelParams.LoadXml(xmlAsset.text);
        XmlNodeList dataList = CitadelParams.GetElementsByTagName(itemType);
        foreach (XmlNode node in dataList)
        {
            var atrs = node.Attributes;
            if (atrs["Workname"].Value == workName)
            {
                XmlNodeList list = node.ChildNodes;
                module.Workname = workName;
                foreach (XmlNode item in list)
                {

                    if (item.InnerText != "")
                    {

                        switch (item.Name)
                        {
                            case "GameName":
                                module.GameName = item.InnerText;
                                break;
                            case "Description":
                                module.Description = item.InnerText;
                                break;
                            case "ModelName":
                                module.ModelName = item.InnerText;
                                break;
                            case "IconName":
                                module.IconName = item.InnerText;
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
                            case "Compatibility":
                                ((Subsystem) module).Compatibility = GameModelsAndEnums.GetCompatibility(item.InnerText);
                                break;
                            case "BuildConditions":
                                ((Subsystem)module).BuildConditions = new List<bool>();//GameModelsAndEnums.GetCompatibility(item.InnerText);
                                break;
                            case "Upgradeability":
                                ((Subsystem)module).Upgradeability = GameModelsAndEnums.GetCompatibility(item.InnerText);
                                break;
                        }
                    }

                }
                
            }
        }

    }

    public static void LoadUnit(string workName, Unit unit)
    {
        CitadelParams = new XmlDocument();
        var xmlPath = "Units";
        var itemType = "Unit";
        if (unit.GetType() == typeof (UnitUpgrade))
        {
            itemType = "Upgrade";
        }
            TextAsset xmlAsset = Resources.Load(xmlPath) as TextAsset;
        if (xmlAsset) CitadelParams.LoadXml(xmlAsset.text);
        XmlNodeList dataList = CitadelParams.GetElementsByTagName(itemType);
        var nodeList = new List<XmlNode>();
        if (unit.GetType() == typeof(UnitUpgrade))
        {
            nodeList.AddRange(dataList.Cast<XmlNode>().Where(o => o.Name.Contains("Upg")));
        }
        else
        {
            nodeList.AddRange(from XmlNode o in dataList where !o.Name.Contains("Upg") select o);
        }
        unit.Workname = workName;
        foreach (XmlNode node in nodeList)
        {

            var atrs = node.Attributes;
            if (atrs["Workname"].Value == workName)
            {
                XmlNodeList list = node.ChildNodes;
                foreach (XmlNode item in list)
                {
                    
                    if (item.InnerText != "")
                    {
                        var boosterName = "";
                        var isBoostedField = false;
                        var originalValue = "0";
                        var boostValue = "0";
                        boostValue = GameModelsAndEnums.CheckBoost(item.InnerText, ref isBoostedField, ref boosterName, ref originalValue);
                        if (isBoostedField) item.InnerText = originalValue;
                        var fieldName = "";
                        switch (item.Name)
                        {
                            case "UnitName": 
                                unit.UnitName = item.InnerText;
                                break;
                            case "UnitDescription":
                                unit.UnitDescription = item.InnerText;
                                break;
                            case "ModelName":
                                unit.ModelName = item.InnerText;
                                break;
                            case "IconName":
                                unit.IconName = item.InnerText;
                                break;
                            case "HpMax":
                                fieldName = "HpMax";
                                unit.HpMax = Single.Parse(item.InnerText);
                                break;
                            case "HpWreckage":
                                fieldName = "HpWreckage";
                                unit.HpWreckage = Single.Parse(item.InnerText);
                                break;
                            case "HpRegen":
                                fieldName = "HpRegen";
                                unit.HpRegen = Single.Parse(item.InnerText);
                                break;
                            case "CostParts":
                                fieldName = "CostParts";
                                unit.CostParts = Single.Parse(item.InnerText);
                                break;
                            case "CostEnergy":
                                fieldName = "CostEnergy";
                                unit.CostEnergy = Single.Parse(item.InnerText);
                                break;
                            case "CostSM":
                                fieldName = "CostSM";
                                unit.CostSM = Single.Parse(item.InnerText);
                                break;
                            case "BasicBuildTime":
                                fieldName = "BasicBuildTime";
                                unit.BasicBuildTime = Single.Parse(item.InnerText);
                                break;
                            case "DeployRadius":
                                fieldName = "DeployRadius";
                                unit.DeployRadius = Single.Parse(item.InnerText);
                                break;
                            case "SpeedGlobal":
                                fieldName = "SpeedGlobal";
                                unit.SpeedGlobal = Single.Parse(item.InnerText);
                                break;
                            case "Speed":
                                fieldName = "Speed";
                                unit.Speed = Single.Parse(item.InnerText);
                                break;
                            case "ReverseFactor":
                                fieldName = "ReverseFactor";
                                unit.ReverseFactor = Single.Parse(item.InnerText);
                                break;
                            case "TurnSpeed":
                                fieldName = "TurnSpeed";
                                unit.TurnSpeed = Single.Parse(item.InnerText);
                                break;
                            case "Accuracy":
                                fieldName = "Accuracy";
                                unit.Accuracy = Single.Parse(item.InnerText);
                                break;
                            case "Acceleration":
                                fieldName = "Acceleration";
                                unit.Acceleration = Single.Parse(item.InnerText);
                                break;
                            case "Mass":
                                fieldName = "Mass";
                                unit.Mass = Single.Parse(item.InnerText);
                                break;
                            case "Recovery":
                                unit.Recovery = GameModelsAndEnums.GetUnitRecovery(item.InnerText);
                                break;
                            case "Class":
                                unit.UnitClasses = GameModelsAndEnums.GetUnitClasses(item.InnerText);
                                break;
                            case "Priority":
                                unit.Priorities = GameModelsAndEnums.GetUnitPriorities(item.InnerText);
                                break;
                            case "Sight":
                                fieldName = "Sight";
                                unit.Sight = Single.Parse(item.InnerText);
                                break;
                            case "OptimalDistance":
                                fieldName = "OptimalDistance";
                                unit.OptimalDistance = Single.Parse(item.InnerText);
                                break;
                            case "RefArmPHYS":
                                fieldName = "RefArmPHYS";
                                unit.RefArmPHYS = Single.Parse(item.InnerText);
                                break;
                            case "RefArmEN":
                                fieldName = "RefArmEN";
                                unit.RefArmEN = Single.Parse(item.InnerText);
                                break;
                            case "RefArmHEAT":
                                fieldName = "RefArmHEAT";
                                unit.RefArmHEAT = Single.Parse(item.InnerText);
                                break;
                            case "ConArmPHYS":
                                fieldName = "ConArmPHYS";
                                unit.ConArmPHYS = Single.Parse(item.InnerText);
                                break;
                            case "ConArmEN":
                                fieldName = "ConArmEN";
                                unit.ConArmEN = Single.Parse(item.InnerText);
                                break;
                            case "ConArmHEAT":
                                fieldName = "ConArmHEAT";
                                unit.ConArmHEAT = Single.Parse(item.InnerText);
                                break;
                            case "UpgradeCost":
                                fieldName = "UpgradeCost";
                                unit.UpgradeCost = Single.Parse(item.InnerText);
                                break;
                            case "UpgradeRequirements":
                                unit.UpgradeRequirements = GameModelsAndEnums.GetUpgradeRequirements(item.InnerText);
                                break;
                            case "UpgradeCategory":
                                unit.Category = item.InnerText;
                                break;
                            case "SchemeCostMetal":
                                fieldName = "SchemeCostMetal";
                                unit.SchemeCostMetal = Single.Parse(item.InnerText);
                                break;
                            case "SchemeCostEnergy":
                                fieldName = "SchemeCostEnergy";
                                unit.SchemeCostEnergy = Single.Parse(item.InnerText);
                                break;
                           
                        }
                        if (isBoostedField)
                        {                    
                            var booster = new UnitBooster(fieldName, Single.Parse(boostValue), boosterName, unit);
                            unit.Boosters.Add(booster);
                        }
                    }

                }

            }
        }
    }

    public static List<UnitUpgrade> GetUpgardesMatchedToUnit(string unitWorkname)
    {
        var finalList = new List<UnitUpgrade>();
        CitadelParams = new XmlDocument();
        var xmlPath = "Units";
        var itemType = "Upgrade";
        TextAsset xmlAsset = Resources.Load(xmlPath) as TextAsset;
        if (xmlAsset) CitadelParams.LoadXml(xmlAsset.text);
        XmlNodeList dataList = CitadelParams.GetElementsByTagName(itemType);
        foreach (XmlNode upgrade in dataList)
        {
            if (upgrade.Attributes["Workname"].Value.Contains(unitWorkname))
            {
                 finalList.Add(new UnitUpgrade(upgrade.Attributes["Workname"].Value));          
            }
        }
        return finalList;
    }   

    public enum EnumGameItemType
    {
        Citadel,
        Modules
    }
}
