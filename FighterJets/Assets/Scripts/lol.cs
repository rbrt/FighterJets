using UnityEngine;
using System.Collections;

public class lol : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.StartSafeCoroutine(LOL());
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator LOL(){
		var mat = GetComponent<Renderer>().sharedMaterial;
		float lolFloat = .1f;
		while (true){
			mat.SetFloat("_WaveVector", lolFloat);
			lolFloat += .01f;
			yield return null;
		}
	}
}
