using UnityEngine;
using System.Collections;

public class SpanwMatrix : MonoBehaviour {
	public GameObject prefab;
	public float distance = 10;

	void Start(){
	}


	public void Spawn(int i){
		int line = 0;
		int pos = 0;

		for(int j = 0; j < i; j++){
			GameObject GO = Instantiate(prefab, transform.position + new Vector3(pos * distance, 0, line * distance), Quaternion.identity) as GameObject;
			GO.transform.SetParent (transform);
			pos++;
			if (pos > 10) {
				line++;
				pos = 0;
			}
		}

	}


	public void Remove (int units){
		units = Mathf.Min (units, transform.childCount);
		for (int i = 0; i < units; i++) {
			Destroy(transform.GetChild (i).gameObject);
		}
	}
}
