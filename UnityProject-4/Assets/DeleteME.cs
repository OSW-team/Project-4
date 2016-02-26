using UnityEngine;
using System.Collections;

public class DeleteME : MonoBehaviour {
	public float Speed = 100;
	bool dir = false;
	Rigidbody rigBody;
	// Use this for initialization
	void Start () {
		rigBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		//if (rigBody == null) {
			transform.position += Vector3.right * Speed * Time.deltaTime;
			/*if (dir) {
				//transform.localScale += new Vector3 (100 * Time.deltaTime, 100 * Time.deltaTime, 100 * Time.deltaTime);
				if (transform.localScale.x > 100) {
					dir = !dir;
				}
			} else {
				//transform.localScale -= new Vector3 (100 * Time.deltaTime, 100 * Time.deltaTime, 100 * Time.deltaTime);
				if (transform.localScale.x < 10) {
					dir = !dir;
				}
			}*/
		//} else {
			//rigBody.AddForce (Vector3.right * (Speed / Time.fixedDeltaTime));
			//rigBody.velocity = Vector3.zero;
		//}

	}
}
