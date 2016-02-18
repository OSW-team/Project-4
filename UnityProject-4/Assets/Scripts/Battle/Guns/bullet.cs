using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
    public float damage;
	public GameObject bang;
    void OnCollisionEnter(Collision c)
    {
        UnitStats other = c.other.gameObject.GetComponentInParent<UnitStats>();
		if (bang != null) {
			Instantiate (bang, transform.position, transform.rotation);
		}
        if (other != null)
        {
			Debug.Log ("Hit");
            other.Damage(damage);
        }
        Destroy(gameObject);
    }
}
