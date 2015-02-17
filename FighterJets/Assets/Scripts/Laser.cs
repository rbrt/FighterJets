using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	[SerializeField] protected float laserSpeed,
									 lifeTime = 10;

	float laserCharge,
		  startTime;

	public void FireLaser(float laserCharge){
		this.laserCharge = laserCharge;
		startTime = Time.time;
	}

	void Update(){
		transform.position = transform.position + transform.forward * laserSpeed * Time.deltaTime;

		if (startTime - Time.time > lifeTime){
			Destroy(this.gameObject);
		}
	}

}
