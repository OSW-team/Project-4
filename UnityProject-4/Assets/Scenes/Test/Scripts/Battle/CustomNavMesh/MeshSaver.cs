using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshSaver : MonoBehaviour {
	public List<MeshSaverElement> elements;

	
}
[System.Serializable]
public class MeshSaverElement{
	public Vector3 A;
	public Vector3 B;
	public Vector3 C;
	public MeshSaverElement(Vector3 _A, Vector3 _B, Vector3 _C){
		A = _A;
		B = _B;
		C = _C;
	}
}