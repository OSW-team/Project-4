using UnityEngine;
using System.Collections;

public class TestGears : MonoBehaviour {
	public GearboxTransmission[] transMissions;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetKeyDown(KeyCode.Space)){
			transMissions[0].currentGear = transMissions[0].currentGear < 1 ? 3 : transMissions[0].currentGear - 1;
			transMissions[1].currentGear = transMissions[1].currentGear < 1 ? 3 : transMissions[1].currentGear - 1;
		}*/




		if (Input.GetKey (KeyCode.RightArrow)) {
			transMissions [0].currentGear = 0;
			transMissions [1].currentGear = 1;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			
			transMissions [0].currentGear = 1;
			transMissions [1].currentGear = 0;
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			transMissions [0].currentGear = 1;
			transMissions [1].currentGear = 1;
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			transMissions [0].currentGear = 0;
			transMissions [1].currentGear = 0;
		} else {
			transMissions [0].currentGear = 1;
			transMissions [1].currentGear = 1;
		}
	}
}
