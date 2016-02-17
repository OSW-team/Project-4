using UnityEngine;
using System.Collections;

public class DrawCollision : MonoBehaviour {
    void OnCollisionEnter(Collision c)
    {
        GameObject mark = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mark.GetComponent<Collider>().enabled = false;
        mark.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Instantiate(mark, c.contacts[0].point, Quaternion.identity);
    }
}
