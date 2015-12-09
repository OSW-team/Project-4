using UnityEngine;
using System.Collections;





public class GameInterface : MonoBehaviour {

	public float mouseSensivity = 0.3f;
	public float cameraMaxLeft, cameraMaxRight, cameraMaxUp, cameraMaxDown;
	public float maxCameraHeight = 5;
	public float cameraAngularSpeed = 2;
	Vector3 mouseNow = Vector3.zero;




	public void TimeScale(float scale)
	{
		Time.timeScale = scale;
	}

	public void TimeScaleAdd(float scale)
	{
		if (Time.timeScale + scale > 0) {
			Time.timeScale += scale;
		}
	}

	void CameraMoving()
	{
		if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width && Input.mousePosition.y >= 0 && Input.mousePosition.y <= Screen.height) {
			if ((Input.mousePosition.x < Screen.currentResolution.width/ 50 || Input.GetKey(KeyCode.A))) {
				Camera.main.transform.position -= Camera.main.transform.right * mouseSensivity;
			}
			if ((Screen.width - Input.mousePosition.x < Screen.currentResolution.width / 50 || Input.GetKey(KeyCode.D)) ) {
				Camera.main.transform.position += Camera.main.transform.right * mouseSensivity;
			}
			if ((Input.mousePosition.y < 10 || Input.GetKey(KeyCode.S)) &&Camera.main.transform.position.z > cameraMaxDown) {
				Camera.main.transform.position -= Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up) * mouseSensivity;
			}
			if ((Screen.height - Input.mousePosition.y < Screen.currentResolution.height / 50 ||Input.GetKey(KeyCode.W)) ) {
				Camera.main.transform.position += Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up) * mouseSensivity;
			}
		}
		Ray ray = new Ray(Camera.main.transform.position, Vector3.down);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			if(Vector3.Distance(Camera.main.transform.position, hit.point) < maxCameraHeight)
			{
				Camera.main.transform.position += Vector3.up * (maxCameraHeight - Vector3.Distance(Camera.main.transform.position, hit.point));
			}
		}
		Vector3 dir;
		Ray _ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit _hit;
		if(Input.GetKey(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") < 0)
		{

			if (Physics.Raycast (_ray, out _hit, Mathf.Infinity)) {
				dir = (Camera.main.transform.position - _hit.point).normalized;
				Camera.main.transform.position += dir*mouseSensivity;
			}
		}
		if(Input.GetKey(KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if (Physics.Raycast (_ray, out _hit, Mathf.Infinity)) {
				dir = (Camera.main.transform.position - _hit.point).normalized;
				Camera.main.transform.position -= dir * mouseSensivity;
			}
		}

		if (Input.GetMouseButtonDown (2)) {
			//Debug.Log("is");
			mouseNow = Input.mousePosition;
		}
		if(Input.GetMouseButton(2)) {
			Camera.main.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(Camera.main.transform.forward, Camera.main.transform.forward + Camera.main.transform.right*(Input.mousePosition - mouseNow).x + Camera.main.transform.up*(Input.mousePosition - mouseNow).y, cameraAngularSpeed / 180 * Mathf.PI/20, 1));
		}
		
	}


	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		CameraMoving ();

	}
}
