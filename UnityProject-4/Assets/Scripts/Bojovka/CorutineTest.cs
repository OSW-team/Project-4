using UnityEngine;
using System.Collections;

public class CorutineTest : MonoBehaviour {
	float z = 16;
	public Transform enemy, aimVector;
	public float V;
	float g = Physics.gravity.magnitude;
	public int flattness = -1;

	void Start()
	{
		
		StartCoroutine(TestCoroutine());
	}

	IEnumerator TestCoroutine()
	{
		while (true) {
			yield return null;
			Vector3 shootPoint = enemy.position + new Vector3 (0, 2, 0);
			Vector3 onTarget = (shootPoint - aimVector.transform.position);

			float X = Mathf.Sqrt (onTarget.x * onTarget.x + onTarget.z * onTarget.z), Y = onTarget.y;

			float sqrtDiscriminant = Mathf.Sqrt ((Y * g * X * X - V * V * X * X) * (Y * g * X * X - V * V * X * X) - (X * X + Y) * g * g * X * X * X * X);

			Vector2 V0 = new Vector3 ();
			V0.x = (-(Y * g * X * X - V * V * X * X) - flattness * sqrtDiscriminant) / (2 * (X * X + Y));

			V0.y = Mathf.Sqrt (V * V - V0.x);
			V0.x = Mathf.Sqrt (V0.x);
			Debug.Log ("Я сделяль! " + V0);
		}
	}
}
