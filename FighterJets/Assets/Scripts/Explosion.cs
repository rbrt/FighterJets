using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    [SerializeField] protected GameObject explosion1,
                                          explosion2;

    void Awake(){
        this.StartSafeCoroutine(RotateExplosion(explosion1));
        this.StartSafeCoroutine(RotateExplosion(explosion2));
        this.StartSafeCoroutine(ScaleExplosion(explosion1));
        this.StartSafeCoroutine(ScaleExplosion(explosion2));
    }

    IEnumerator ScaleExplosion(GameObject explosion){
        float expandTime = .3f;

        for (float i = 0; i < 1; i += Time.deltaTime / expandTime){
            explosion.transform.localScale = new Vector3(i, i, i);
            yield return null;
        }
        explosion.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(.2f);

        for (float i = 1; i > 0; i -= Time.deltaTime / expandTime * 2){
            explosion.transform.localScale = new Vector3(i, i, i);
            yield return null;
        }
        explosion.transform.localScale = Vector3.zero;
        Destroy(this.gameObject);
    }

    IEnumerator RotateExplosion(GameObject explosion){
        float x = Random.Range(0, 30);

        while (true){
            if (x < 10){
                explosion.transform.Rotate(1, 0, 0);
            }
            else if (x < 20){
                explosion.transform.Rotate(0, 1, 0);
            }
            else {
                explosion.transform.Rotate(0, 0, 1);
            }

            yield return null;
        }
    }

}
