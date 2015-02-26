using UnityEngine;
using System.Collections;

public class AffectWater : MonoBehaviour {

	Material water;
	Transform waterTransform;
	[SerializeField] protected float effectMinHeight;

	[SerializeField] protected WaterSplashController waterSplashController;

	Vector3 haltEffectSignal;

	void Awake(){
		haltEffectSignal = new Vector3(-9999, 0, 0);
		if ((GameObject.Find("WaterMeshGenerator") as GameObject)){
			waterTransform = (GameObject.Find("WaterMeshGenerator") as GameObject).GetComponent<Transform>();
			water = waterTransform.GetComponent<Renderer>().sharedMaterial;
		}
	}

	void Update () {
		if (!water){
			return;
		}

		float deltaY = transform.position.y - waterTransform.position.y;

		if (deltaY > effectMinHeight){
			water.SetVector("_PlayerPosition", haltEffectSignal);
			return;
		}

		waterSplashController.DeltaY = deltaY / effectMinHeight;

		var playerForward = transform.forward;
		var playerPosition = transform.position;

		water.SetVector("_PlayerPosition", transform.position);
		water.SetVector("_PlayerForward", transform.forward);
		water.SetVector("_PlayerRight", transform.right);
		water.SetFloat("_DeltaY", effectMinHeight - deltaY);
	}
}
