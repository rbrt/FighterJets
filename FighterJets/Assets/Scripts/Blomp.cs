using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Blomp : MonoBehaviour {

    [SerializeField] protected GameObject leftFan,
                                          rightFan;

    [SerializeField] protected float moveSpeed;

    [SerializeField] protected GameObject blompsplosion,
                                          explosionPrefab;

    [SerializeField] protected GameObject[] gameObjectsToFallDuringDeath;

    Transform[] leftBlompsplosions,
                rightBlompsplosions,
                frontRingRight,
                frontRingLeft,
                middleRingRight,
                middleRingLeft,
                rearRingRight,
                rearRingLeft;

	void Awake(){
        this.StartSafeCoroutine(SpinBlompFans());

        leftBlompsplosions = GetBlompsplosionsByName("LeftExplosion");
        rightBlompsplosions = GetBlompsplosionsByName("RightExplosion");
        frontRingRight = GetBlompsplosionsByName("FrontRingRight");
        frontRingLeft = GetBlompsplosionsByName("FrontRingLeft");
        middleRingRight = GetBlompsplosionsByName("MiddleRingRight");
        middleRingLeft = GetBlompsplosionsByName("MiddleRingLeft");
        rearRingLeft = GetBlompsplosionsByName("RearRingLeft");
        rearRingRight = GetBlompsplosionsByName("RearRingRight");
    }

    Transform[] GetBlompsplosionsByName(string blompsplosionName){
        return blompsplosion.GetComponentsInChildren<Transform>()
                            .Where(x => x.parent.name.Equals(blompsplosionName))
                            .Select(x => x.transform)
                            .ToArray();
    }

    void Update() {
        var pos = transform.position;

        pos += transform.forward * moveSpeed * Time.deltaTime;

        transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Q)){
            Blompsplosion();
        }
    }

    void Blompsplosion(){
        this.StartSafeCoroutine(ExplodeBlomp());
    }

    void MakeBlompPartsFall(GameObject[] objects){
        objects.ToList().ForEach(blompPart => {
            blompPart.transform.parent = null;
            var rigid = blompPart.AddComponent<Rigidbody>();
            rigid.AddForce(blompPart.transform.forward * 500 * (Random.Range(1,10) > 5 ? -1 : 1));
            rigid.AddForce(blompPart.transform.right * 500 *(Random.Range(1,10) > 5 ? -1 : 1));
        });
    }

    IEnumerator ExplodeBlomp(){
        this.StartSafeCoroutine(ExplodeTargets(leftBlompsplosions, .2f));
        this.StartSafeCoroutine(ExplodeTargets(rightBlompsplosions, .2f));

        yield return new WaitForSeconds(1.5f);

        this.StartSafeCoroutine(ExplodeTargets(rearRingRight, .2f));
        this.StartSafeCoroutine(ExplodeTargets(rearRingLeft, .2f));

        this.StartSafeCoroutine(ExplodeTargets(middleRingRight, .2f));
        this.StartSafeCoroutine(ExplodeTargets(middleRingLeft, .2f));

        this.StartSafeCoroutine(ExplodeTargets(frontRingRight, .2f));
        this.StartSafeCoroutine(ExplodeTargets(frontRingLeft, .2f));

        yield return new WaitForSeconds(.4f);

        MakeBlompPartsFall(gameObjectsToFallDuringDeath);

        yield return new WaitForSeconds(.6f);

        var turrets = GetComponentsInChildren<Transform>().Where(x => x.parent != null &&  x.name.Contains("Turret")).ToArray();
        MakeBlompPartsFall(turrets.Select(x => x.gameObject).ToArray());
        this.StartSafeCoroutine(ExplodeTargets(turrets,
                                               .1f));

        yield return new WaitForSeconds(1f);

        var fans = GetComponentsInChildren<Transform>().Where(x => x.name.Contains("Fan")).ToArray();
        var fins = GetComponentsInChildren<Transform>().Where(x => x.name.Contains("Fin")).ToArray();

        this.StartSafeCoroutine(ExplodeTargets(fans, .2f));
        this.StartSafeCoroutine(ExplodeTargets(fins, .2f));

    }

    IEnumerator ExplodeTargets(Transform[] targets, float delay){
        for (int i = 0; i < targets.Length; i++){
            Explode(targets[i].position);
            yield return new WaitForSeconds(delay);
        }
    }

    void Explode(Vector3 position){
        var explosion = GameObject.Instantiate(explosionPrefab,
                                               position,
                                               Quaternion.Euler(new Vector3(Random.Range(0,180),
                                                                            Random.Range(0,180),
                                                                            Random.Range(0,180)
                                                                            )
                                                                )
                                               ) as GameObject;
    }

    IEnumerator SpinBlompFans(){
        while (true){
            leftFan.transform.Rotate(0, -2, 0);
            rightFan.transform.Rotate(0, 2, 0);
            yield return null;
        }
    }
}
