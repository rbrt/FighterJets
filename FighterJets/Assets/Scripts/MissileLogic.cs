using UnityEngine;
using System.Collections;

public class MissileLogic : MonoBehaviour {

	[SerializeField] protected Transform target;
    
    [SerializeField] protected Explosion explosion;
    [SerializeField] protected Light trailLight;
    [SerializeField] protected GameObject smokePrefab;

    bool shooting = false;

    float targetDistance = 5;
    float force = 2000;
    float maxStep = 500f;

    Rigidbody rigid;

	void Awake(){
        rigid = GetComponent<Rigidbody>();
	}

    public void ShootMissile(Transform target, Transform instantiationPoint, Transform deployPoint){
        this.StartSafeCoroutine(MoveToDeployPoint(instantiationPoint, deployPoint));
        this.target = target;
    }

    IEnumerator MoveToDeployPoint(Transform instantiationPoint, Transform deployTarget){
        float duration = .5f;

        for (float i = 0; i < 1; i += Time.deltaTime / duration){
            transform.localPosition = Vector3.Lerp(instantiationPoint.transform.localPosition, deployTarget.transform.localPosition, i);
            yield return null;
        }

        trailLight.gameObject.SetActive(true);
        this.StartSafeCoroutine(FlashTrail());

        yield return new WaitForSeconds(.2f);

        this.transform.parent = null;
        shooting = true;

        this.StartSafeCoroutine(SpawnSmoke());
    }

    IEnumerator SpawnSmoke(){
        float spawnRate = .1f;
        while (true){
            GameObject.Instantiate(smokePrefab, transform.position, Quaternion.Euler(new Vector3(Random.Range(0,180), Random.Range(0,180), 0)));
            yield return new WaitForSeconds(spawnRate);
        }
    }

    IEnumerator FlashTrail() {
        float duration = .6f;
        while (true){ 
            for (float i = 1.5f; i < 2; i += Time.deltaTime / duration){
                trailLight.range = i;
                yield return null;
            }

            for (float i = 2f; i < 1.5f; i -= Time.deltaTime / duration)
            {
                trailLight.range = i;
                yield return null;
            }
        }
    }

    void FixedUpdate(){
        if (shooting && target != null){
            rigid.AddForce(transform.forward * force);

            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                                          Quaternion.LookRotation(target.position - transform.position),
                                                          maxStep * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < targetDistance) {
                GameObject.Instantiate(explosion.gameObject, target.transform.position, target.transform.rotation);
                Destroy(target.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
