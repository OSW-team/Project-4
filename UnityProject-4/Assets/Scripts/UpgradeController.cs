using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UpgradeController : MonoBehaviour
{

    public List<string> Upgrades;
    [SerializeField]
    public string Workname;
    [SerializeField]
    public string ShowFrom;
    [SerializeField]
    public int FromIndex;
    [SerializeField]
    public int UntilIndex;
    public string ShowUntil;
    private Unit _unit;
    private bool _from;
    private bool _to;
    // Use this for initialization
	void Start () {
        FindObjectOfType<TestController>().UpgradeChanged += CheckUpgrades;
        FindObjectOfType<TestController>().UpgradeChange();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void CheckUpgrades()
    {
        var controller = FindObjectOfType<TestController>();
        if (controller.MyCitadel!=null)
        {
            var units = controller.MyCitadel.Units;
            _unit = units.FirstOrDefault(x => x.Workname == Workname);
            _from = true;
            _to = false;

            foreach (var upgrade in _unit.Upgrades)
            {
                if (upgrade.Workname.Contains(ShowFrom) || ShowFrom == "NONE")
                {
                    _from = true;
                }
                else
                {
                    _from = false;
                }
                if (upgrade.Workname.Contains(ShowUntil) || ShowUntil == "NONE")
                {
                    _to = true;
                }
                else
                {
                    _to = false;
                }
            }
            if (_from && !_to) gameObject.SetActive(true);
            else if (_to || !_from)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void GetUnitsUpgrades()
    {
        Upgrades = new List<string>();

        if (Upgrades != null)
        {
            Upgrades.Clear();
            var upgrs = XMLWorker.GetUpgardesMatchedToUnit(Workname);
            Upgrades.Add("NONE");
            foreach (var upgr in upgrs)
            {
                Upgrades.Add(upgr.Workname);
            }
        }
    }

}
