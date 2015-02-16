using UnityEngine;
using System.Collections;

public class Blomp : MonoBehaviour {

    [SerializeField] protected GameObject leftFan,
                                          rightFan;

    [SerializeField] protected float moveSpeed;

	void Awake(){
        this.StartSafeCoroutine(SpinBlompFans());
    }

    void Update() {
        var pos = transform.position;

        pos += transform.forward * moveSpeed * Time.deltaTime;

        transform.position = pos;
    }

    IEnumerator SpinBlompFans(){
        while (true){
            leftFan.transform.Rotate(0, -1, 0);
            rightFan.transform.Rotate(0, 1, 0);
            yield return null;
        }
    }
}
