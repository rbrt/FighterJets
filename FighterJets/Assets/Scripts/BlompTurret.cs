using UnityEngine;
using System.Collections;
using System.Linq;

public class BlompTurret : MonoBehaviour {

	[SerializeField] protected GameObject laserPrefab;

	GameObject player;
	Quaternion originalRotation;

	GameObject barrel1,
			   barrel2;

	float rotationSpeed = 4,
		  thresholdAngle = 115,
		  targetingThreshold = 10,
		  laserCoolDown = 1f,
		  laserRange = 1200;

	bool canFireLaser = true;

	void Start(){
		originalRotation = transform.rotation;
		var barrels = GetComponentsInChildren<Transform>().Where(x => x.name.Contains("Barrel")).ToList();
		barrel1 = barrels[0].gameObject;
		barrel2 = barrels[1].gameObject;

		player = FighterController.Instance.gameObject;
	}

	void Update () {
		var look = player.transform.position - transform.position;
		var targetRotation = Quaternion.RotateTowards(transform.rotation,
													  Quaternion.LookRotation(look),
													  rotationSpeed);

		if (Quaternion.Angle(targetRotation, originalRotation) <= thresholdAngle){
			transform.rotation = targetRotation;
		}

		if (Vector3.Angle(player.transform.position - transform.position, transform.forward) < targetingThreshold){
			if (canFireLaser && Vector3.Distance(transform.position, player.transform.position) < laserRange){
				FireLaser();
			}
		}
	}

	void FireLaser(){
		this.StartSafeCoroutine(LaserCoolDown());
		var laser = GameObject.Instantiate(laserPrefab, barrel1.transform.position, barrel1.transform.rotation) as GameObject;
		var laser1 = GameObject.Instantiate(laserPrefab, barrel2.transform.position, barrel2.transform.rotation) as GameObject;
		laser.GetComponent<Laser>().FireLaser(0);
		laser1.GetComponent<Laser>().FireLaser(0);
	}

	IEnumerator LaserCoolDown(){
		float startTime = Time.time;
		canFireLaser = false;

		while (Time.time - startTime < laserCoolDown){
			yield return null;
		}

		canFireLaser = true;
	}
}
