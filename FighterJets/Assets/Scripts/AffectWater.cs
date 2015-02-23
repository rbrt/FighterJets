using UnityEngine;
using System.Collections;

public class AffectWater : MonoBehaviour {

	Material water;

	// Use this for initialization
	void Start () {
		if ((GameObject.Find("WaterMeshGenerator") as GameObject)){
			water = (GameObject.Find("WaterMeshGenerator") as GameObject).GetComponent<Renderer>().sharedMaterial;
		}
	}

	// Update is called once per frame
	void Update () {

		if (!water){
			return;
		}

		var playerForward = transform.forward;
		var playerPosition = transform.position;

		water.SetVector("_PlayerPosition", transform.position);
		water.SetVector("_PlayerForward", transform.forward);
		water.SetVector("_PlayerRight", transform.right);

		float deltaY = 2;			// Should be the diff between playerY and vertexY
		float scaleWidth = 5;		// Should dependent on deltaY somehow
		Vector3 forwardPoint = (playerPosition + playerForward);
		Vector3 rearPoint = (playerPosition - playerForward);

		Vector3 rightPoint = rearPoint + transform.right;
		Vector3 leftPoint = rearPoint - transform.right;

		leftPoint.y = playerPosition.y;
		rightPoint.y = playerPosition.y;
	}
}
