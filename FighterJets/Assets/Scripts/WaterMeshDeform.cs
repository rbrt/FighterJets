using UnityEngine;
using System.Collections;

public class WaterMeshDeform : MonoBehaviour {

	[SerializeField] protected GameObject meshA;
	[SerializeField] protected GameObject meshB;

	void Start () {
		this.StartSafeCoroutine(BoostUp());
		this.StartSafeCoroutine(PingPongScale(meshA));
		this.StartSafeCoroutine(PingPongScale(meshB));
	}

	IEnumerator BoostUp(){
		float rotationDegrees = 5;
		while (true){
			meshA.transform.Rotate(0, rotationDegrees, 0);
			meshB.transform.Rotate(0, -rotationDegrees, 0);

			rotationDegrees = 4 + Mathf.PingPong(rotationDegrees, 5);
			yield return null;
		}
	}

	IEnumerator PingPongScale(GameObject toScale){
		float duration = Random.Range(.2f, .4f);
		yield return new WaitForSeconds(Random.Range(0,.2f));
		while (true){
			for (float i = .7f; i <= 1; i += Time.deltaTime / duration){
				toScale.transform.localScale = Vector3.one * i;
				yield return null;
			}
			toScale.transform.localScale = Vector3.one;
			for (float i = 1; i >= .7f; i -= Time.deltaTime / duration){
				toScale.transform.localScale = Vector3.one * i;
				yield return null;
			}
			toScale.transform.localScale = Vector3.one * .5f;
		}
	}
}
