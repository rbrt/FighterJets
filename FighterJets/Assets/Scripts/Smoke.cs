using UnityEngine;
using System.Collections;

public class Smoke : MonoBehaviour {

    void Awake() {
        this.StartSafeCoroutine(Smokin());
    }

    IEnumerator Smokin(){
        float startTime = Time.time;
        float lifeTime = 1f;
        float direction = Random.Range(0,30);
        float spawnDuration = .2f;

        for (float i = 0; i < 1; i += Time.deltaTime / spawnDuration) {
            transform.localScale = new Vector3(i, i, i);
            yield return null;
        }

        while (Time.time - startTime < lifeTime){
            if (direction < 10){
                transform.Rotate(2, 0, 0);
            }
            else if (direction < 20){
                transform.Rotate(0, 2, 0);
            }
            else{
                transform.Rotate(0, 0, 2);
            }
            yield return null;
        }

        for (float i = 1; i > 0; i -= Time.deltaTime / spawnDuration){
            transform.localScale = new Vector3(i, i, i);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
