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

    List<GameObject> leftBlompsplosions,
                     rightBlompsplosions;

	void Awake(){
        this.StartSafeCoroutine(SpinBlompFans());

        leftBlompsplosions = blompsplosion.GetComponentsInChildren<Transform>()
                                          .Where(x => x.parent.name.Equals("LeftExplosion"))
                                          .Select(x => x.gameObject)
                                          .ToList();
        rightBlompsplosions = blompsplosion.GetComponentsInChildren<Transform>()
                                           .Where(x => x.parent.name.Equals("RightExplosion"))
                                           .Select(x => x.gameObject)
                                           .ToList();

        leftBlompsplosions.ForEach(x => Debug.Log(x, x.gameObject));
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
        for (int i = 0; i < leftBlompsplosions.Count; i++){
            Explode(leftBlompsplosions[i].transform.position);
            Explode(rightBlompsplosions[i].transform.position);
            yield return new WaitForSeconds(.1f);
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
        //explosion.transform.localScale *= 2;
    }

    IEnumerator SpinBlompFans(){
        while (true){
            leftFan.transform.Rotate(0, -2, 0);
            rightFan.transform.Rotate(0, 2, 0);
            yield return null;
        }
    }
}
