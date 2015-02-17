using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EditorStuff : MonoBehaviour {

	[MenuItem("Custom/Center and rotate blomp turret pivots")]
	public static void KillAllBlompTurretComponents(){
		var blompTurrets = GameObject.FindObjectsOfType<Transform>().Where(x => x.name.Contains("TurretPivot")).ToList();

		var baseRotation = blompTurrets.FirstOrDefault(x => x.name.Contains("TurretPivot.005")).transform.localRotation;

		for (int i = blompTurrets.Count -1; i >= 0; i--){
			blompTurrets[i].transform.localPosition = Vector3.zero;
			blompTurrets[i].transform.localRotation = baseRotation;
		}
	}

	[MenuItem("Custom/Print Application.Datapath")]
	public static void PrintApplicationDataPath(){
		Debug.Log(Application.dataPath);
	}

}
