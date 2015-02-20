using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class EditorStuff : MonoBehaviour {

	[MenuItem("Custom/Log out trailrenderer hidden stuff")]
	public static void KillAllBlompTurretComponents(){
		var trailRenderer = GameObject.FindObjectsOfType<TrailRenderer>().FirstOrDefault() as TrailRenderer;
		BindingFlags flags = BindingFlags.IgnoreReturn | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy | BindingFlags.IgnoreCase ;

		trailRenderer.GetType()
					 .GetProperties(flags)
					 .Where(x => x.ToString().ToLower().Contains("m_"))
					 .ToList()
					 .ForEach(x => Debug.Log(x.ToString()));

		trailRenderer.GetType()
					.GetFields(flags)
					.Where(x => x.ToString().ToLower().Contains("m_"))
					.ToList()
					.ForEach(x => Debug.Log(x.ToString()));

		trailRenderer.GetType()
					 .GetMethods(flags)
					 .Where(x => x.ToString().ToLower().Contains("m_"))
					 .ToList()
					 .ForEach(x => Debug.Log(x.ToString()));
	}

	[MenuItem("Custom/Print Application.Datapath")]
	public static void PrintApplicationDataPath(){
		Debug.Log(Application.dataPath);
	}

	[MenuItem("Custom/Print Stuff")]
	public static void PrintStuff(){
		Debug.Log(GameObject.Find("Blomp").GetComponentsInChildren<Transform>().Where(x => x.parent.name.Contains("Turret")).ToList().Count());
	}

}
