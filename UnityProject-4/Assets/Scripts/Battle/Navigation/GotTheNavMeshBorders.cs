using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GotTheNavMeshBorders : MonoBehaviour {
	public NavMesh NMesh1;
	NavMeshTriangulation NMesh;
	public List<Vector3> GranList;
	public GameObject GranPrefab;


	// Use this for initialization
	void Start () {
		if (GranPrefab.GetComponent<GotTheNavMeshBorders> ().GranList.Count == 0) {
			NMesh = NavMesh.CalculateTriangulation ();

			Vector3[,] Vect = new Vector3[(int)NMesh.indices.Length / 3, 3];
			int j = 0;
			for (int i = 0; i < NMesh.indices.Length; i += 3) {

				Vect [j, 0] = NMesh.vertices [NMesh.indices [i + 0]];
				Vect [j, 1] = NMesh.vertices [NMesh.indices [i + 1]];
				Vect [j, 2] = NMesh.vertices [NMesh.indices [i + 2]];
				j++;
			}

			Vector3[,] Gran = new Vector3[j * 3, 2];
			for (int i = 0; i < j; i++) {
				Gran [i * 3 + 0, 0] = Vect [i, 0];
				Gran [i * 3 + 0, 1] = Vect [i, 1];

				Gran [i * 3 + 1, 0] = Vect [i, 1];
				Gran [i * 3 + 1, 1] = Vect [i, 2];

				Gran [i * 3 + 2, 0] = Vect [i, 2];
				Gran [i * 3 + 2, 1] = Vect [i, 0];
			}

			for (int i = 0; i < j * 3; i++) {
				for (int k = 0; k < j * 3; k++) {
					if (Gran [i, 0] == Gran [k, 1] && Gran [i, 1] == Gran [k, 0]) {
						Gran [i, 1] = Vector3.zero;
						Gran [k, 0] = Vector3.zero;
						Gran [k, 1] = Vector3.zero;

					}
				}
			}

			for (int i = 0; i < j * 3; i++) {
				if (Gran [i, 0] != Vector3.zero && Gran [i, 1] != Vector3.zero) {
				
					GranList.Add (Gran [i, 0]);
					GranList.Add (Gran [i, 1]);


					//Debug.DrawLine (Gran [i, 0], Gran [i, 1], Color.black, 1000);
				}
			}
			GranPrefab.GetComponent<GotTheNavMeshBorders> ().GranList = GranList;
		}
		//Debug.Log(GranList.ToArray ().Length);


	}

}
