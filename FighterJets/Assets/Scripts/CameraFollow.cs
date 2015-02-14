using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

	[SerializeField] protected Transform targetPosition;
	[SerializeField] protected float followSpeed,
									 rotateSpeed;

	[SerializeField] protected int frameDelay;

	List<Vector3> positions;
	List<Quaternion> rotations;

	void Awake(){
		refVector = Vector3.zero;
		positions = new List<Vector3>();
		rotations = new List<Quaternion>();
	}

	// Update is called once per frame
	void Update () {

		if (positions.Count > 0){
			transform.position = positions[0];
			transform.rotation = rotations[0];
			positions.RemoveAt(0);
			rotations.RemoveAt(0);
		}
	}

	public void AddFollowStep(Vector3 pos, Quaternion rot){
		StartCoroutine(WaitThenAddStep(pos, rot));
	}

	IEnumerator WaitThenAddStep(Vector3 pos, Quaternion rot){
		int count = frameDelay;
		while (count-- > 0){
			yield return null;
		}

		positions.Add(pos);
		rotations.Add(rot);
	}
}
