using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new Form1();
            var unitManager = new UnitManager(form);
            Application.Run(form);
        }
    }

  public class UnitManager
    {
        public Form1 MyForm;
        public string UnitsXmlPath = "UnityProject-4/Assets/Resources/Units.xml";
        public string UnitModelXMLPath = "UnityProject-4/Assets/Resources/UnitModel.xml";
        private XmlNode CurrentUnitNode;
        private XmlNode UnitNodeCopy;
        private XmlNode CurrentUpgradeNode;
        public string CurrentUnitWorkname;
        public string CurrentUpgradeWorkname;
        XmlDocument UnitsDoc = new XmlDocument();
        XmlDocument UnitsDocCopy = new XmlDocument();
        XmlDocument UnitModelDoc = new XmlDocument();
        public UnitManager(Form form)
        {
            MyForm = (Form1)form;
            MyForm.UnitManager = this;
        }

        public void Start()
        {

            if (MyForm.GetUnitModelPath() != "")
            {
                UnitsXmlPath = MyForm.GetUnitModelPath() + "/Units.xml";
                UnitModelXMLPath = MyForm.GetUnitModelPath() + "/UnitModel.xml";
            }

            if(UnitsXmlPath!=null) UnitsDoc.Load(UnitsXmlPath);
            UnitsDocCopy = UnitsDoc;
            UnitModelDoc.Load(UnitModelXMLPath);
            
            MyForm.SetUnitsCombobox(GetUnitList());
            FillUnitTable();
            MyForm.ShowUpgradesList();
        }

        private List<string> GetUnitList()
        {
            var resList = new List<string>();           
            XmlNodeList dataList = UnitsDoc.GetElementsByTagName("Unit");
            foreach (XmlNode node in dataList)
            {
                resList.Add(node.Attributes["Workname"].Value);
            }
            if (CurrentUnitNode == null && dataList.Count > 0)
            {
                CurrentUnitNode = dataList[0];
                UnitNodeCopy = CurrentUnitNode;
                CurrentUnitWorkname = dataList[0].Attributes["Workname"].Value;
            }
            return resList;
        }

        public List<string> GetUpgradesList()
        {
            var resList = new List<string>();
            XmlNodeList dataList = UnitsDoc.GetElementsByTagName("Upgrade");
            foreach (XmlNode node in dataList)
            {
                if (node.Attributes["Workname"].Value.Contains(CurrentUnitWorkname))
                {
                    resList.Add(node.Attributes["Workname"].Value);
                }
            }
            return resList;
        }

        public void LoadUnitFields(string Workname)
        {
            XmlNodeList dataList = UnitsDoc.GetElementsByTagName("Unit");
            foreach (XmlNode node in dataList)
            {
               if (node.Attributes["Workname"].Value==Workname)
                {
                    CurrentUnitNode = node;
                    UnitNodeCopy = CurrentUnitNode;
                    CurrentUnitWorkname = Workname;
                }
            }
        }



        public void LoadUpgradeFields(string Workname)
        {
            XmlNodeList dataList = UnitsDoc.GetElementsByTagName("Upgrade");
            foreach (XmlNode node in dataList)
            {
                if (node.Attributes["Workname"].Value == Workname)
                {
                    CurrentUpgradeNode = node;
                    CurrentUpgradeWorkname = Workname;
                }
            }
        }



        public void FillUnitTable()
        {
            MyForm.CheckBoxList.Clear();
            MyForm.SetUnitFields(GetUnitFieldsList(), GetUnitFieldsValues());
        }

        public void FillUpgradeTable()
        {
            MyForm.SetUpgradeFields(GetUpgradeFieldsList(), GetUpgradeFieldsValues());
        }

        public List<string> GetUnitFieldsList()
        {
            var resList = new List<string>();
            if (CurrentUnitNode != null)
            {
                foreach (XmlNode node in CurrentUnitNode.ChildNodes)
                {
                    resList.Add(node.Name);
                }
            }
            return resList;
        }

        private List<string> GetUpgradeFieldsList()
        {
            var resList = new List<string>();
            if (CurrentUpgradeNode != null)
            {
                foreach (XmlNode node in CurrentUpgradeNode.ChildNodes)
                {
                    resList.Add(node.Name);
                }
            }
            return resList;
        }

        public void ReplaceUnitValue(string name, string text)
        {
            foreach(XmlNode node in CurrentUnitNode.ChildNodes)
            {
                if (node.Name == name) node.InnerText = text;
            }
        }
        public void ReplaceUpgradeValue(string name, string text)
        {
            foreach (XmlNode node in CurrentUpgradeNode.ChildNodes)
            {
                if (node.Name == name) node.InnerText = text;
            }
        }

        public void AddUpgradeValues(string Workname, bool check, bool redraw = true)
        {
            
            XmlNodeList dataList = UnitsDoc.GetElementsByTagName("Upgrade");
            XmlNode upgr = CurrentUpgradeNode;
            foreach (XmlNode node in dataList)
            {
                if (node.Attributes["Workname"].Value == Workname)
                {
                    upgr = node; 
                }
            }
            foreach (XmlNode upgrNode in upgr.ChildNodes)
            {
                foreach (XmlNode unitNode in UnitNodeCopy.ChildNodes)
                {
                    if(unitNode.Name == upgrNode.Name&&upgrNode.InnerText!="")
                    {
                        try {
                            if (check)
                            {
                                var sum = Single.Parse(unitNode.InnerText) + Single.Parse(upgrNode.InnerText);
                                unitNode.InnerText = sum.ToString();
                            }
                            else
                            {
                                var sum = Single.Parse(unitNode.InnerText) - Single.Parse(upgrNode.InnerText);
                                unitNode.InnerText = sum.ToString();
                            }
                        }
                        catch { }
                    }
                }
            }
            if(redraw)MyForm.SetUnitFields(GetUnitFieldsList(), GetUnitUpgradedFieldsValues());
        }

        public List<string> GetUnitUpgradedFieldsValues()
        {
            var resList = new List<string>();
            if (UnitNodeCopy != null)
            {
                foreach (XmlNode node in UnitNodeCopy.ChildNodes)
                {
                    resList.Add(node.InnerText);
                }
            }
            return resList;
        }

        private List<string> GetUnitFieldsValues()
        {
            var resList = new List<string>();
            if (CurrentUnitNode != null)
            {
                foreach (XmlNode node in CurrentUnitNode.ChildNodes)
                {
                    resList.Add(node.InnerText);
                }
            }
            return resList;
        }

        private List<string> GetUpgradeFieldsValues()
        {
            var resList = new List<string>();
            if (CurrentUpgradeNode != null)
            {
                foreach (XmlNode node in CurrentUpgradeNode.ChildNodes)
                {
                    resList.Add(node.InnerText);
                }
            }
            return resList;
        }

        public void AddUnit(string workname)
        {
            XmlNode unitModel = UnitModelDoc.GetElementsByTagName("Unit")[0];
            unitModel.Attributes["Workname"].Value = workname;
            var importNode = UnitsDoc.ImportNode(unitModel, true);
            XmlNode mainNode = UnitsDoc.GetElementsByTagName("units")[0];

            if (importNode != null)
            {
                mainNode.AppendChild(importNode);
                Save();
                Start();
            }
        }

        public void AddUpgrade(string workname)
        {
            XmlNode unitModel = UnitModelDoc.GetElementsByTagName("Upgrade")[0];
            unitModel.Attributes["Workname"].Value = workname+"-"+CurrentUnitWorkname;
            var importNode = UnitsDoc.ImportNode(unitModel, true);
            XmlNode mainNode = UnitsDoc.GetElementsByTagName("units")[0];

            if (importNode != null)
            {
                mainNode.AppendChild(importNode);
                Save();
                Start();
            }

        }

        public void MoveUpUnitField(string Workname)
        {
            var targetNode = UnitModelDoc.GetElementsByTagName(Workname)[0];
            var parentNode = targetNode.ParentNode;
            var list = new List<XmlNode>();
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                list.Add(node);
            }
            var index = list.IndexOf(targetNode);
            if (index > 0)
            {
                var switchedNode = list[index - 1];
                parentNode.InsertBefore(targetNode, switchedNode);
                
                Save();
                Start();
            }
            
        }

        public  void Save()
        {
            UnitsDoc.Save(UnitsXmlPath);
            UnitModelDoc.Save(UnitModelXMLPath);
        }

    }


}
