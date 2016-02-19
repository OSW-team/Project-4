using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class ManualPathBuilder : MonoBehaviour {
	public Mesh mesh;
	int index = 0;
	public List<GameObject> mPoints;
	public List<Vector3> vertices;
	public List<int> triangles;
	public List<VertexTriangle> pointList;
	public List<MeshBalls> meshBalls;
	public GameObject point;
	public string meshName;
	GameObject grappedPoint;
	public GameObject Prefab;

	// Use this for initialization
	void Start () {
		if (meshName == null || meshName.Length == 0) {
			meshName = "new_mesh_" + Random.Range (0, 100);
		}
		//if (point == null) {
		point = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/Battle/CustomNavMesh/CustomMeshCreation/point.prefab");
		Debug.Log (point);
		if (Prefab == null || Prefab.GetComponent<MeshSaver>() == null) {
			Prefab = new GameObject ();
			Prefab.AddComponent<MeshSaver> ();
			PrefabUtility.CreatePrefab ("Assets/Resources/Prefabs/Battle/CustomNavMesh/Savers/" + meshName + "_Saver.prefab", Prefab);
			//AssetDatabase.CreateAsset (Prefab, "Assets/Prefabs/Savers/" + meshName + "_Saver.prefab");
			Prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/Battle/CustomNavMesh/Savers/" + meshName + "_Saver.prefab");

		}

		//}
		gameObject.AddComponent<MeshFilter> ();
		gameObject.AddComponent<MeshRenderer> ();
		mesh = GetComponent<MeshFilter> ().mesh;

		if (Prefab.GetComponent<MeshSaver> ().elements.Count > 0) {
			List<GameObject> balls = new List<GameObject> ();
			// = Instantiate(point, element.C, Quaternion.identity) as GameObject;
			foreach (MeshSaverElement element in Prefab.GetComponent<MeshSaver> ().elements) {
				GameObject a;
				GameObject b;
				GameObject c;
				a = Instantiate(point, element.A, Quaternion.identity) as GameObject;

				b = Instantiate(point, element.B, Quaternion.identity) as GameObject;

				c = Instantiate(point, element.C, Quaternion.identity) as GameObject;

				if (balls.Count == 0) {
					balls.Add (a);
					balls.Add (b);
					balls.Add (c);
				}
				List<GameObject> exist = new List<GameObject>();
				foreach (GameObject ball in balls) {
					Debug.Log (ball.transform.position);
					if (element.A == ball.transform.position && a != ball) {
						Destroy (a);
						a = ball;
					} else {
						exist.Add (a);
					}
					if (element.B == ball.transform.position && b != ball) {
						Destroy (b);
						b = ball;
					}else {
						exist.Add (b);
					}
					if (element.C == ball.transform.position && c != ball) {
						Destroy (c);
						c = ball;
					}else {
						exist.Add (c);
					}
				}
				foreach (GameObject ball in exist) {
					balls.Add (ball);
				}

				meshBalls.Add (new MeshBalls (a, b, c));



			}
		}

		int triIndex = 0;
		foreach (MeshBalls MB in meshBalls) {


			vertices.Add (MB.A.transform.position);
			vertices.Add (MB.B.transform.position);
			vertices.Add (MB.C.transform.position);

			triangles.Add (triIndex ++);
			triangles.Add (triIndex ++);
			triangles.Add (triIndex ++);
			//Debug.DrawLine (MB.A.transform.position, MB.B.transform.position, Color.black, 1000);
			//Debug.DrawLine (MB.A.transform.position, MB.B.transform.position, Color.black, 1000);
			//Debug.DrawLine (MB.A.transform.position, MB.B.transform.position, Color.black, 1000);


		}

		//mesh.Clear ();

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		gameObject.AddComponent<MeshCollider> ();	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.B)){
			Debug.Log (AssetDatabase.GetAssetPath (0));
			AssetDatabase.CreateAsset (mesh, "Assets/Resources/Prefabs/Battle/CustomNavMesh/Meshes/"+meshName+".asset");
			if (Prefab.GetComponent<MeshSaver> ().elements == null) {
				Prefab.GetComponent<MeshSaver> ().elements = new List<MeshSaverElement> ();
			}

			Prefab.GetComponent<MeshSaver> ().elements.Clear();
			foreach (MeshBalls ball in meshBalls) {
				Prefab.GetComponent<MeshSaver> ().elements.Add(new MeshSaverElement(ball.A.transform.position, ball.B.transform.position, ball.C.transform.position));
			}

			AssetDatabase.SaveAssets();
		}
		if(Input.GetKeyDown(KeyCode.F)){
			Flip();
		}
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction * 100, Color.red);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) {
			if (Input.GetMouseButtonDown (0)) {
				if (hit.collider.GetComponent<ManualPoint> () == null) {
					GameObject sphere0 = Instantiate (point, hit.point, Quaternion.identity) as GameObject;
					ManualPoint MP0 = sphere0.GetComponent<ManualPoint> ();
					MP0.id = index++;

					mPoints.Add (sphere0);
				} else {
					mPoints.Add(hit.collider.gameObject);
					index++;

				}
				/*
				if (hit.collider.GetComponent<ManualPoint> () == null && meshBalls.Count == 0) {
					GameObject sphere0 = Instantiate (point, hit.point, Quaternion.identity) as GameObject;
					ManualPoint MP0 = sphere0.GetComponent<ManualPoint> ();
					MP0.id = index++;

					mPoints.Add (sphere0);
				}
				if (meshBalls.Count > 0){
					if (index == 0) {
						GameObject sphere0 = Instantiate (point, hit.point, Quaternion.identity) as GameObject;
						ManualPoint MP0 = sphere0.GetComponent<ManualPoint> ();
						MP0.id = index++;

						mPoints.Add (sphere0);
					} else if (hit.collider.GetComponent<ManualPoint> () != null) {
						mPoints.Add(hit.collider.gameObject);
						index++;
					}
				}
				*/
				if (index == 3)
				{
					meshBalls.Add (new MeshBalls (mPoints.ToArray()[0], mPoints.ToArray()[1], mPoints.ToArray()[2]));
					index = 0;
					mPoints.Clear ();
				}


			}
			if (Input.GetKeyDown (KeyCode.C) && hit.collider.GetComponent<ManualPoint>()!= null) {
				DeleteBall ();
				GameObject.Destroy(hit.collider.gameObject);
			}
			if (Input.GetMouseButtonDown (1) && hit.collider.GetComponent<ManualPoint>()!= null) {

				grappedPoint = hit.collider.gameObject;
				grappedPoint.GetComponent<SphereCollider> ().enabled = false;

			}
			if (grappedPoint != null) {
				grappedPoint.transform.position = hit.point;
			}
		}

		if (Input.GetMouseButtonUp (1)) {

			if (grappedPoint != null) {
				grappedPoint.GetComponent<SphereCollider> ().enabled = true;
				grappedPoint =  null;
			}
		}


		vertices.Clear();
		triangles.Clear();
		int triIndex = 0;
		foreach (MeshBalls MB in meshBalls) {


			vertices.Add (MB.A.transform.position);
			vertices.Add (MB.B.transform.position);
			vertices.Add (MB.C.transform.position);

			triangles.Add (triIndex ++);
			triangles.Add (triIndex ++);
			triangles.Add (triIndex ++);
			//Debug.DrawLine (MB.A.transform.position, MB.B.transform.position, Color.black, 1000);
			//Debug.DrawLine (MB.A.transform.position, MB.B.transform.position, Color.black, 1000);
			//Debug.DrawLine (MB.A.transform.position, MB.B.transform.position, Color.black, 1000);


		}

		mesh.Clear ();

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();




















		/*Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction * 100, Color.red);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (Input.GetMouseButtonDown (0)) {
				if (pointList.Count < 2) {
					pointList.Add (new VertexTriangle (hit.point, pointList.Count));
					vertices.Add (pointList [pointList.Count - 1].coords);

				} else {


					//Нашли две ближайшие, сделаем с ними треугольник
					VertexTriangle near1 =  pointList[0], near2 =  pointList[1];
					foreach (VertexTriangle near in pointList) {
						if (near.coords != near1.coords && near.coords != near2.coords) {
							if (Vector3.SqrMagnitude (near.coords - hit.point) < Vector3.SqrMagnitude (hit.point - near1.coords)) {
								near1 = near;
								Debug.Log (Vector3.Magnitude (near.coords - hit.point) + " < " + Vector3.Magnitude (hit.point - near1.coords) + " YES");
							} else if (Vector3.SqrMagnitude (near.coords - hit.point) < Vector3.SqrMagnitude (hit.point - near2.coords)) {
								near2 = near;
								Debug.Log (Vector3.Magnitude (near.coords - hit.point) + " < " + Vector3.Magnitude (hit.point - near2.coords) + " YES");
							} else {
								Debug.Log (Vector3.Magnitude (near.coords - hit.point) + " > " + Vector3.Magnitude (hit.point - near1.coords)+ " and " + Vector3.Magnitude (near.coords - hit.point) + " > " + Vector3.Magnitude (hit.point - near2.coords)  + " NO");
//							}
//						}
//					}
//					Debug.Log (near1.index + " " + near2.index);
//					pointList.Add (new VertexTriangle (hit.point, pointList.Count));

//					if (near1.index < near2.index) {
//						triangles.Add (near1.index);
//						triangles.Add (near2.index);
//					} else {
//						triangles.Add (near2.index);
//						triangles.Add (near1.index);
//					}

//					vertices.Add (hit.point);

//					triangles.Add (pointList[pointList.Count-1].index);
//					mesh.Clear ();
//					mesh.vertices = vertices.ToArray();

//					mesh.triangles = triangles.ToArray();
//				}
//			}

//		}*/
	}

	public void Flip(){
		meshBalls [meshBalls.Count-1] = new MeshBalls (meshBalls [meshBalls.Count -1 ].C, meshBalls [meshBalls.Count -1].B, meshBalls [meshBalls.Count -1].A);
	}

	public void DeleteBall(){
		meshBalls.RemoveAll(EveryTriangle);
	}

	private static bool EveryTriangle(MeshBalls ball){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.GetComponent<ManualPoint> () != null) {
				return (ball.A == hit.collider.gameObject || ball.B == hit.collider.gameObject || ball.C == hit.collider.gameObject);
			}
			return false;
		}
		return false;
	}
}

[System.Serializable]
public struct VertexTriangle{
	public Vector3 coords;
	public int index;
	public VertexTriangle(Vector3 _coords, int _index){
		index = _index;
		coords = _coords;
	}
}
[System.Serializable]
public struct MeshBalls{
	public GameObject A;
	public GameObject B;
	public GameObject C;
	//public int index;
	public MeshBalls(GameObject _A, GameObject _B, GameObject _C){
		A = _A;
		B = _B;
		C = _C;
	}
}