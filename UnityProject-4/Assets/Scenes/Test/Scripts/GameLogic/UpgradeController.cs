using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UpgradeController : MonoBehaviour
{

    public List<string> Upgrades;
    public List<string> Filters;
    [SerializeField]
    public string Workname;
    [SerializeField]
    public string A;
    [SerializeField]
    public string B;
    [SerializeField]
    public string C;
    [SerializeField]
    public string A1;
    [SerializeField]
    public string B1;
    [SerializeField]
    public string C1;
    [SerializeField]
    public int AIndex;
    [SerializeField]
    public int A1Index;
    [SerializeField]
    public int BIndex;
    [SerializeField]
    public int B1Index;
    [SerializeField]
    public int CIndex;
    [SerializeField]
    public int C1Index;
    [SerializeField]
    public string CurrentFromFilter;
    [SerializeField]
    public string CurrentToFilter;
    [SerializeField]
    public int FromFilterIndex;
    [SerializeField]
    public int ToFilterIndex;

    private Unit _unit;
    private bool _from;
    private bool _to;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckUpgrades()
    {
        var controller = FindObjectOfType<TestController>();
        if (controller.MyCitadel != null)
        {
            var units = controller.MyCitadel.Units;
            _unit = units.FirstOrDefault(x => x.Workname == Workname);
            _from = true;
            _to = false;
            var upgrades = new List<string>();
            foreach (var upgrade in _unit.Upgrades)
            {
                upgrades.Add(upgrade.Workname);
            }
            switch (CurrentFromFilter)
            {
                case "NONE":
                        if (upgrades.Contains(A) || A == "NONE")
                        {
                            _from = true;
                        }
                        else
                        {
                            _from = false;
                        }
                    if (upgrades.Count < 1) if (A == "NONE") { _from = true; } else { _from = false; }
                    break;
                case "AND":
                        if ((upgrades.Contains(A) || A == "NONE") && (upgrades.Contains(B) || B == "NONE"))
                        {
                            _from = true;
                        }
                        else
                        {
                            _from = false;
                        }
                    if (upgrades.Count < 1) if (( A == "NONE") && (B == "NONE")) { _from = true; } else { _from = false; }
                    break;
                case "OR":
                        if ((upgrades.Contains(A) || A == "NONE") || (upgrades.Contains(B) || B == "NONE"))
                        {
                            _from = true;
                        }
                        else
                        {
                            _from = false;
                        }
                    if (_unit.Upgrades.Count < 1) if ((A == "NONE") || (B == "NONE")) { _from = true; } else { _from = false; }
                    break;
                case "AND(_OR_)":
                        if ((upgrades.Contains(A) || A == "NONE") && ((upgrades.Contains(B) || B == "NONE") || (upgrades.Contains(C) || C == "NONE")))
                        {
                            _from = true;
                        }
                        else
                        {
                            _from = false;
                        }
                    if (_unit.Upgrades.Count < 1) if ((A == "NONE") && ((B == "NONE") || (C == "NONE"))) { _from = true; } else { _from = false; }
                    break;
                case "OR(_AND_)":
                        if ((upgrades.Contains(A) || A == "NONE") || ((upgrades.Contains(B) || B == "NONE") && (upgrades.Contains(C) || C == "NONE")))
                        {
                            _from = true;
                        }
                        else
                        {
                            _from = false;
                        }
                    if (_unit.Upgrades.Count < 1) if ((A == "NONE") || ((B == "NONE") && (C == "NONE"))) { _from = true; } else { _from = false; }
                    break;                   
                case "AND_AND_":
                        if ((upgrades.Contains(A) || A == "NONE") && (upgrades.Contains(B) || B == "NONE") && (upgrades.Contains(C) || C == "NONE"))
                        {
                            _from = true;
                        }
                        else
                        {
                            _from = false;
                        }
                    if (_unit.Upgrades.Count < 1) if ((A == "NONE") && (B == "NONE") && (C == "NONE")) { _from = true; } else { _from = false; }
                    break;
            }
            switch (CurrentToFilter)
            {
                case "NONE":
                        if (upgrades.Contains(A1))
                        {
                            _to = true;
                        }
                        else
                        {
                            _to = false;
                        }
                    break;
                case "AND":
                        if ((upgrades.Contains(A1)) && (upgrades.Contains(B1)))
                    {
                        _to = true;
                    }
                    else
                    {
                        _to = false;
                    }
                    break;
                case "OR":
                        if ((upgrades.Contains(A1)) || (upgrades.Contains(B1)))
                    {
                        _to = true;
                    }
                    else
                    {
                        _to = false;
                    }
                    break;
                case "AND(_OR_)":
                        if ((upgrades.Contains(A1)) && ((upgrades.Contains(B1)) || (upgrades.Contains(C1))))
                    {
                        _to = true;
                    }
                    else
                    {
                        _to = false;
                    }
                    break;
                case "OR(_AND_)":
                        if ((upgrades.Contains(A1)) || ((upgrades.Contains(B1)) && (upgrades.Contains(C1))))
                    {
                        _to = true;
                    }
                    else
                    {
                        _to = false;
                    }
                    break;
                case "AND_AND_":
                        if ((upgrades.Contains(A1)) && (upgrades.Contains(B1)) && (upgrades.Contains(C1)))
                    {
                        _to = true;
                    }
                    else
                    {
                        _to = false;
                    }
                    break;
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
            Filters = new List<string>()
                {
                  "NONE",
                  "AND",
                  "OR",
                  "AND(_OR_)",
                  "OR(_AND_)",
                  "AND_AND_"
                };
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
