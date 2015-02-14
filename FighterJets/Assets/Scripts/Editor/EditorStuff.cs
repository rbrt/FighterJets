using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorStuff : MonoBehaviour {

	[MenuItem("Custom/Print Application.Datapath")]
	public static void PrintApplicationDataPath(){
		Debug.Log(Application.dataPath);
	}

}
