using UnityEngine;
using System.Collections;

public class InfantryController : MonoBehaviour {
	public float RaycastLength, height, attackDistance, angularSpeed, damage;
	public Transform target, weapon;
	NavMeshAgent agent;
	Animator anim;
	public ParticleSystem sparks;
	TargetSeek seeker;
	ORCAController controller;
	float controllerPower;
	// Use this for initialization
	void Start () {
		agent = GetComponentInChildren<NavMeshAgent> ();
		seeker = GetComponent<TargetSeek> ();
		anim = GetComponent<Animator> ();
		controller = GetComponent<ORCAController> ();
		controllerPower = controller.power;

	}
	
	// Update is called once per frame
	void Update () {
		if (seeker.chosenTarget != null) {
			target = seeker.chosenTarget.transform;
		}
		RaycastHit hit;
		Debug.DrawRay (transform.position, Vector3.down * RaycastLength);

		//transform.rotation = agent.transform.rotation;
		//agent.transform.localRotation = Quaternion.identity;


		if (target != null) {
			//agent.SetDestination (target.position);

			Debug.DrawRay (weapon.position, target.position - weapon.position, Color.green);
			if (Physics.Raycast (weapon.position, target.position - weapon.position, out hit, RaycastLength)) {
				Vector3 toTarget = (hit.point - weapon.position);
				if (toTarget.sqrMagnitude < attackDistance * attackDistance) {
				
					//Debug.Log(anim.runtimeAnimatorController.animationClips[1].frameRate);

					Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (new Vector3 (toTarget.x, 0, toTarget.z)), angularSpeed * Time.deltaTime);
					anim.SetInteger ("State", 1);
					controller.power = 0;
				} else {
					anim.SetInteger ("State", 0);
					controller.power = controllerPower;
				}
			} else {
				anim.SetInteger ("State", 0);
				agent.Resume ();
			}
		} else {
			anim.SetInteger ("State", 0);
			agent.Resume ();
		}
			
	}

	public void DealDamage(float _damage){
		//Debug.Log ("You make " + damage + " damage for your enemy!");
		sparks.Emit (100);
		UnitStats stats = target.gameObject.GetComponent<UnitStats> ();
		if (stats != null) {
			Debug.Log(stats.gameObject);
			stats.Damage (damage);

		}
	}
}
