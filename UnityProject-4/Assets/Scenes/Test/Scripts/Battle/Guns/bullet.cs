using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
    public float damage;
    void OnCollisionEnter(Collision c)
    {
        UnitStats other = c.other.gameObject.GetComponentInParent<UnitStats>();
        if (other != null)
        {
			Debug.Log ("Hit");
            other.Damage(damage);
        }
        Destroy(gameObject);
    }
}
