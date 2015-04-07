using UnityEngine;
using System.Collections;

public class WaterMeshDeform : MonoBehaviour {

	[SerializeField] protected GameObject meshA;
	[SerializeField] protected GameObject meshB;

    float minScale = .7f,
          maxScale = 1;

	void Start () {
        meshA.transform.Rotate(Vector3.up, Random.Range(0, 180));
        meshB.transform.Rotate(Vector3.up, Random.Range(0, 180));

        meshA.transform.localScale = Random.Range(minScale, maxScale) * Vector3.one;
        meshB.transform.localScale = Random.Range(minScale, maxScale) * Vector3.one;

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
            for (float i = minScale; i <= maxScale; i += Time.deltaTime / duration) {
				toScale.transform.localScale = Vector3.one * i;
				yield return null;
			}
			toScale.transform.localScale = Vector3.one;
            for (float i = maxScale; i >= minScale; i -= Time.deltaTime / duration) {
				toScale.transform.localScale = Vector3.one * i;
				yield return null;
			}
			toScale.transform.localScale = Vector3.one * .5f;
		}
	}
}


