using UnityEngine;
using System.Collections;

public class FastWaves : MonoBehaviour {

	[SerializeField] protected GameObject waveA;
	[SerializeField] protected GameObject waveB;
	[SerializeField] protected float scrollSpeed;

	Material waveAMat;
	Material waveBMat;

	void Awake(){
		waveBMat = waveB.GetComponent<Renderer>().sharedMaterial;
		waveAMat = waveA.GetComponent<Renderer>().sharedMaterial;
		this.StartSafeCoroutine(ScrollWave(waveAMat, true));
		this.StartSafeCoroutine(ScrollWave(waveBMat, false));
	}

	IEnumerator ScrollWave(Material wave, bool positive){
		Vector2 offset = wave.GetTextureScale("_MainTex");
		while (true){
			offset.x = 1 + Mathf.PingPong(Time.time * 5, .5f);
			wave.SetTextureScale("_MainTex", offset);
			yield return null;
		}

	}
}
