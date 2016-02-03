using UnityEngine;
using System.Collections;

public class UnitStats : MonoBehaviour {
    public Agent meAgent;
    public int team;
    public float maxHP = 100;
    
    public int type; //0 - пехота, 1 - легкая техника, 2 - тяж. техника, 3 - арта, 4 - ракетница, 5 - авиация, 6 - здание, 7 - обломки.
    float HP;
    MasterMindNHWheels master;
    void Start()
    {
        master = GameObject.FindWithTag("masterMind").GetComponent<MasterMindNHWheels>();
        HP = maxHP;
    }
    public void Damage(float damage)
    {
        HP -= damage;
		//Debug.Log (gameObject.name + "HP" + HP);
        if(HP <= 0)
        {
            Death();
        }
    }
    void Death()
	{	
		if (meAgent.index != 0) {
			master.RemoveAgent (meAgent);
			Destroy (gameObject);
		} else {
			//Debug.Log ("Oh God! You kill me!");
		}
    }
}
