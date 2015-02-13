using UnityEngine;
using System.Collections;

public class SendCameraPositions : MonoBehaviour {

	[SerializeField] protected CameraFollow cameraFollow;

	void Start () {

	}

	void Update () {
		cameraFollow.AddFollowStep(transform.position, transform.rotation);
	}
}
