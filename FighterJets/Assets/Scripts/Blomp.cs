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

    Vector3[] leftBlompsplosions,
              rightBlompsplosions,
              frontRing,
              middleRing,
              rearRing;

	void Awake(){
        this.StartSafeCoroutine(SpinBlompFans());

        leftBlompsplosions = blompsplosion.GetComponentsInChildren<Transform>()
                                          .Where(x => x.parent.name.Equals("LeftExplosion"))
                                          .Select(x => x.position)
                                          .ToArray();
        rightBlompsplosions = blompsplosion.GetComponentsInChildren<Transform>()
                                           .Where(x => x.parent.name.Equals("RightExplosion"))
                                           .Select(x => x.position)
                                           .ToArray();
        frontRing = blompsplosion.GetComponentsInChildren<Transform>()
                                  .Where(x => x.parent.name.Equals("FrontRingRight") || x.parent.name.Equals("FrontRingLeft"))
                                  .Select(x => x.position)
                                  .ToArray();
        middleRing = blompsplosion.GetComponentsInChildren<Transform>()
                                  .Where(x => x.parent.name.Equals("MiddleRingRight") || x.parent.name.Equals("MiddleRingLeft"))
                                  .Select(x => x.position)
                                  .ToArray();
        rearRing = blompsplosion.GetComponentsInChildren<Transform>()
                                .Where(x => x.parent.name.Equals("RearRingRight") || x.parent.name.Equals("RearRingLeft"))
                                .Select(x => x.position)
                                .ToArray();

        Debug.Log(middleRing.Length);
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

    IEnumerator ExplodeBlomp(){
        this.StartSafeCoroutine(ExplodeTargets(leftBlompsplosions, .2f));
        yield return this.StartSafeCoroutine(ExplodeTargets(rightBlompsplosions, .2f));

        this.StartSafeCoroutine(ExplodeTargets(rearRing, .2f));

        yield return new WaitForSeconds(.4f);

        this.StartSafeCoroutine(ExplodeTargets(middleRing, .2f));

        yield return new WaitForSeconds(.6f);

        this.StartSafeCoroutine(ExplodeTargets(frontRing, .2f));
    }

    IEnumerator ExplodeTargets(Vector3[] targets, float delay){
        for (int i = 0; i < targets.Length; i++){
            Explode(targets[i]);
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
