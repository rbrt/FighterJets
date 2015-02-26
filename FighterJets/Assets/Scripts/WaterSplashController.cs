using UnityEngine;
using System.Collections;

public class WaterSplashController : MonoBehaviour {

	[SerializeField] protected GameObject rightPivot;
	[SerializeField] protected GameObject leftPivot;

	[SerializeField] protected float maxRotation;

	GameObject waterTransform;

	float refFloat;

	float deltaY;
	float currentDeltaY;

	float interpolationCap = .1f;
	float waterHeightOffset = 2;

	public float DeltaY{
		set { deltaY = 1 - value; }
	}

	void Awake(){
		deltaY = 0;
		currentDeltaY = 0;
		refFloat = 0;

		waterTransform = GameObject.Find("WaterMeshGenerator");
	}

	void Update () {
		if (waterTransform == null){
			return;
		}

		currentDeltaY = Mathf.SmoothDamp(currentDeltaY, deltaY, ref refFloat, interpolationCap);

		// TODO same or similar projection going on in shader

		rightPivot.transform.rotation = Quaternion.Euler(new Vector3(0, currentDeltaY * maxRotation, 0));
		leftPivot.transform.rotation = Quaternion.Euler(new Vector3(0, -currentDeltaY * maxRotation, 0));

		var pos = transform.position;
		pos.y = waterTransform.transform.position.y + waterHeightOffset;
		transform.position = pos;
	}
}
