using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ManageUnitsScreenController : ManagementScreen
{
    public Transform UnitsPanel;
    public Text BottomPanelTextField;
    public Text UnitOrUpgradeName;
    public Text SchematiquesText;
    public Text CostText;
    public Button FirstButton;
    public Text HP;
    public Text HPWreck;
    public Text Acceleration;
    public Text Speed;
    public Text Mass;
    public Text Accuracy;

	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowUnit(Unit unit)
    {

        ManagementScreenGameObject.SetActive(true);
        if (unit.GO == null)
        {
            unit.BuildMesh();
            var Center = new Vector3(-1.55f, -10.17f, 31.7f);
            if (CurrentManagingGameObject)
            {
                Destroy(CurrentManagingGameObject);
            }
            CurrentManagingGameObject = unit.GO;
            CurrentManagingGameObject.transform.position = Center;
            CurrentManagingGameObject.transform.eulerAngles = new Vector3(0, 150, 0);
        }
        AddUpgradesButtons(unit);
        AddUnitsToUnitsPanel();
        RefreshInfo(unit);
    }

    private void AddUnitsToUnitsPanel()
    {
        UnitsPanel = transform.FindChild("UnitsPanel");
        for (var i = 0; i < Controller.MyCitadel.Units.Count; i++)
        {
            var unitGO = UnitsPanel.GetChild(i).gameObject;
            var icon = unitGO.transform.GetChild(0).GetComponent<Image>();
            unitGO.GetComponent<Button>().onClick.RemoveAllListeners();
            var i1 = i;
            unitGO.GetComponent<Button>().onClick.AddListener(() => ShowUnit(Controller.MyCitadel.Units[i1]));

            icon.sprite = Resources.LoadAll<Sprite>("UI/Management screen UI/2. Units' screen/UnitButton").FirstOrDefault(x => x.name == Controller.MyCitadel.Units[i].IconName);
            icon.rectTransform.sizeDelta = unitGO.GetComponent<Image>().rectTransform.sizeDelta;
            unitGO.SetActive(true);
        }
    }

    private void AddUpgradesButtons(Unit unit)
    {
        Debug.Log("UnitName + " + unit.Workname);
        var categories = new List<string>() {"Category1", "Category2", "Category3" };
        for (var i = 0; i < categories.Count; i++)
        {
            var categoryTransform = transform.FindChild("Controls").GetChild(i);
            var upgrades = categoryTransform.FindChild("Upgrades");
            if (upgrades)
            {
                for (var k = 0; k < upgrades.childCount; k++)
                {
                    Destroy(upgrades.GetChild(k).gameObject);
                }
            }
            else
            {
                var upgradesGo = new GameObject("Upgrades");
                upgradesGo.transform.SetParent(categoryTransform);
            }

        }
        foreach (var cat in categories)
        {

            var matchedUpgrs = XMLWorker.GetUpgardesMatchedToUnit(unit.Workname);
            var upgs = new List<UnitUpgrade>();
            foreach (var unitUpgrade in matchedUpgrs)
            {
                if (unitUpgrade.Category == cat)
                {
                    upgs.Add(unitUpgrade);
                }
                }
            var R = 50;
            var LampsR = 30;
            for (var i = 0; i < upgs.Count; i++)
            {
                var parentTransform = transform.FindChild("Controls").GetChild(0);
                if (upgs[i].Category == "Category1") parentTransform = transform.FindChild("Controls").GetChild(0);
                if (upgs[i].Category == "Category2") parentTransform = transform.FindChild("Controls").GetChild(1);
                if (upgs[i].Category == "Category3") parentTransform = transform.FindChild("Controls").GetChild(2);
                //for (var l = 0; l < parentTransform.childCount; l++)
                //{ Destroy(parentTransform.GetChild(l).gameObject); }


                var Angle = Mathf.PI / 180.0f * (360.0f / upgs.Count * i - 90);
                var x = parentTransform.position.x + Mathf.Cos(Angle) * R;
                var y = parentTransform.position.y + Mathf.Sin(Angle) * R;
                var posOnCircle = new Vector3(x, y);
                var upgrGO = new GameObject("Up " + upgs[i].Workname);
                upgrGO.transform.position = posOnCircle;
                //upgrGO.transform.SetParent(gameObject.transform);



                var upgImage = upgrGO.AddComponent<Image>();
                upgImage.sprite = Resources.LoadAll<Sprite>("UI/Management screen UI/2. Units' screen/UpgradeFrame")[0];
                upgImage.rectTransform.sizeDelta = 0.8f*parentTransform.GetComponent<Button>().image.rectTransform.sizeDelta;
                upgImage.color = new Color(0.5f,0.5f,0.5f,0.5f);
                var iconGO = new GameObject("Icon");
                iconGO.transform.SetParent(upgrGO.transform);
                iconGO.transform.localPosition = Vector3.zero;
                var iconImage = iconGO.AddComponent<Image>();
                var Iconslist = Resources.LoadAll<Sprite>("UI/Management screen UI/2. Units' screen/CategoryAndUpgradeIcons");
                iconImage.sprite = Iconslist.FirstOrDefault(z => z.name == upgs[i].IconName);
                iconImage.rectTransform.sizeDelta = 0.8f*upgImage.rectTransform.sizeDelta;
                iconImage.color = new Color(0.5f, 0.5f, 0.5f,0.5f);
                var upgrButton = upgrGO.AddComponent<Button>();
                var i1 = i;
                upgrButton.onClick.AddListener(() =>
                {
                    BottomPanelTextField.text = upgs[i1].UnitDescription;
                    UnitOrUpgradeName.text = upgs[i1].UnitName;
                    CostText.text = "CostSM "+upgs[i1].CostSM+ " CostEnergy " + upgs[i1].CostEnergy+ " CostParts " + upgs[i1].CostParts;
                    AddFirstButtonAction(unit, upgs[i1]);
                });
                var upgradesGo = parentTransform.FindChild("Upgrades").gameObject; 
                upgrGO.transform.SetParent(upgradesGo.transform);
                upgrGO.transform.localScale = Vector3.one*1.5f;

                var x1 = parentTransform.position.x + Mathf.Cos(Angle) * LampsR;
                var y1 = parentTransform.position.y + Mathf.Sin(Angle) * LampsR;
                var posOnCircleLamps = new Vector3(x1, y1);
                var lampGO = new GameObject("Lamp");
                lampGO.transform.position = posOnCircleLamps;
                lampGO.transform.SetParent(gameObject.transform);
                var lampImage = lampGO.AddComponent<Image>();

                var lampsIcons = Resources.LoadAll<Sprite>("UI/Management screen UI/2. Units' screen/Lamps");
                lampImage.sprite = lampsIcons.FirstOrDefault(z => z.name == "Lamps_0");
                lampImage.rectTransform.sizeDelta = lampImage.sprite.rect.size;
                foreach (var unitUpgrade in unit.Upgrades)
                {
                    if (unitUpgrade.Workname == upgs[i].Workname)
                    {
                        upgImage.color = new Color(1, 1, 1, 1);
                        iconImage.color = new Color(1, 1, 1, 1);
                        lampImage.sprite = lampsIcons.FirstOrDefault(z => z.name == "Lamps_1");
                    }
                }



            }
        }

        
    }

    void AddFirstButtonAction(Unit unit, UnitUpgrade upgr)
    {
        FirstButton.transform.GetComponentInChildren<Text>().text = "Add Upgrade";
        FirstButton.onClick.RemoveAllListeners();
        FirstButton.onClick.AddListener(()=> {
            unit.Upgrades.Add(new UnitUpgrade(upgr.Workname));
            XMLWorker.SaveSC(Controller.MyCitadel);
            ShowUnit(unit);
            FirstButton.transform.GetComponentInChildren<Text>().text = "";
            Controller.UpgradeChange(unit);
            unit.RecountProps();
            RefreshInfo(unit);
            FirstButton.onClick.RemoveAllListeners();
        });

    }

    void RefreshInfo(Unit unit)
    {
        BottomPanelTextField.text = unit.UnitDescription;
        UnitOrUpgradeName.text = unit.UnitName;
        CostText.text = "CostSM " + unit.CostSM + " CostEnergy " + unit.CostEnergy + " CostParts " + unit.CostParts;
        SchematiquesText.text = Controller.MyCitadel.Units.Count + "/" + "5";//Controller.MyCitadel.MaxTotalSchemeAmount;
        HP.text = unit.HpMax.ToString();
        HPWreck.text = unit.HpWreckage.ToString();
        Acceleration.text = unit.Acceleration.ToString();
        Speed.text = unit.Speed.ToString();
        Mass.text = unit.Mass.ToString();
        Accuracy.text = unit.Accuracy.ToString();
    }

    void OnDisable()
    {
        Destroy(CurrentManagingGameObject);
    }
}
