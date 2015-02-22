using UnityEngine;
using System.Collections;

public class AffectWater : MonoBehaviour {

	Material water;

	// Use this for initialization
	void Start () {
		water = (GameObject.Find("MeshGenerator") as GameObject).GetComponent<Renderer>().sharedMaterial;
	}

	// Update is called once per frame
	void Update () {
		water.SetVector("_PlayerPosition", transform.position);
		water.SetVector("_PlayerForward", transform.forward);
	}
}
