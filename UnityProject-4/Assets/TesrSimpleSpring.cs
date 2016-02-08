using UnityEngine;
using System.Collections;

public class TesrSimpleSpring : MonoBehaviour {
	public Transform wheel;
	SphereCollider WC;
	public float springLength, springForce;
	bool collides = false;
	// Use this for initialization
	void Start () {
		
		WC = wheel.GetComponent<SphereCollider> ();
		Physics.IgnoreCollision (WC, GetComponent<SphereCollider> ());
	}
	void OnCollisionEnter(Collision col){
		//Debug.Log ("Enter");
		collides = true;
	}
	void OnCollisionExit(Collision col){
		collides = false;
		//Debug.Log ("Exit");
	}
	// Update is called once per frame
	void Update () {
		transform.localPosition = wheel.localPosition;
		//Debug.Log (collides);
		if (collides) {
			wheel.transform.localPosition =  new Vector3(0, wheel.transform.localPosition.y - Mathf.Abs( 0.1f/wheel.transform.localPosition.y) * Time.deltaTime , 0);
			//wheel.transform.localPosition =  new Vector3(0, Mathf.Max (-0.1f,wheel.transform.localPosition.y - Time.deltaTime) , 0);
		} else {
			wheel.transform.localPosition = new Vector3(0, Mathf.Min (0.1f, wheel.transform.localPosition.y + Mathf.Abs( 0.1f/wheel.transform.localPosition.y) * Time.deltaTime) ,0);
		}
	}
}
