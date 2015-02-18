using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

public class Boost : MonoBehaviour {

	[SerializeField] protected TrailRenderer innerRenderer,
											 outerRenderer;
	[SerializeField] protected MeshRenderer innerBoost,
										    outerBoost;

	[SerializeField] protected Color innerMinColour,
									 outerMinColour,
									 innerMaxColour,
									 outerMaxColour;

	[SerializeField] protected FighterController FighterController;

	SafeCoroutine boostCoroutine,
				  scalingCoroutine,
				  colourCoroutine;

	FighterController fighterController;

	IEnumerator Primer(){
		yield break;
	}

	void Awake(){
		innerRenderer.sharedMaterial.SetColor("_TintColor", innerMinColour);
		outerRenderer.sharedMaterial.SetColor("_TintColor", outerMinColour);

		boostCoroutine = this.StartSafeCoroutine(Primer());
		scalingCoroutine = this.StartSafeCoroutine(Primer());
		colourCoroutine  = this.StartSafeCoroutine(Primer());
	}

	void Start(){
		fighterController = FighterController.Instance;
	}

	public void StartBoost(){
		KillBoostCoroutines();
		boostCoroutine = this.StartSafeCoroutine(BoostUp());
		scalingCoroutine = this.StartSafeCoroutine(ScaleBoostUp());
		colourCoroutine = this.StartSafeCoroutine(ColourUp());
	}

	public void StopBoost(){
		KillBoostCoroutines();
		scalingCoroutine = this.StartSafeCoroutine(ScaleBoostDown());
		colourCoroutine = this.StartSafeCoroutine(ColourDown());
	}

	void KillBoostCoroutines(){
		if (boostCoroutine.IsRunning){
			boostCoroutine.Stop();
		}
		if (scalingCoroutine.IsRunning){
			scalingCoroutine.Stop();
		}
		if (colourCoroutine.IsRunning){
			colourCoroutine.Stop();
		}
	}

	IEnumerator ColourUp(){
		float duration = 2f;
		float percent = Mathf.Abs((innerRenderer.sharedMaterial.GetColor("_TintColor").r - innerMinColour.r) /
								  (innerMaxColour.r - innerMinColour.r));

		for (float i = percent; i <= 1; i += Time.deltaTime / duration){
			SetInnerAndOuterRendererColours(i);
			fighterController.BoostAmount = i;
			yield return null;
		}
		fighterController.BoostAmount = 1;
		SetInnerAndOuterRendererColours(1);
	}

	IEnumerator ColourDown(){
		float duration = .5f;
		float percent = Mathf.Abs((innerRenderer.sharedMaterial.GetColor("_TintColor").r - innerMinColour.r) /
								  (innerMaxColour.r - innerMinColour.r));

		for (float i = percent; i >= 0; i -= Time.deltaTime / duration){
			SetInnerAndOuterRendererColours(i);
			fighterController.BoostAmount = i;
			yield return null;
		}
		fighterController.BoostAmount = 0;
		SetInnerAndOuterRendererColours(0);
	}

	void SetInnerAndOuterRendererColours(float i){
		innerRenderer.sharedMaterial.SetColor("_TintColor", Color.Lerp(innerMinColour,
																	   innerMaxColour,
																	   i));
		outerRenderer.sharedMaterial.SetColor("_TintColor", Color.Lerp(outerMinColour,
																	   outerMaxColour,
																	   i));
		innerBoost.sharedMaterial.SetColor("_Color", Color.Lerp(innerMinColour,
																	innerMaxColour,
																	Mathf.Pow(i,2)));
		outerBoost.sharedMaterial.SetColor("_Color", Color.Lerp(outerMinColour,
																	outerMaxColour,
																	Mathf.Pow(i,2)));
	}


/*

	string[] indexStrings = {"m_Colors.m_Color[0]",
							 "m_Colors.m_Color[1]",
							 "m_Colors.m_Color[2]",
							 "m_Colors.m_Color[3]",
							 "m_Colors.m_Color[4]"};

	IEnumerator ColourUp(){
		float duration = .2f;
		SerializedObject trail = innerObj;

		float percent = Mathf.Abs((trail.FindProperty(indexStrings[0]).colorValue.r - innerMinColour.r) /
								  (innerMaxColour.r - innerMinColour.r));

		for (float i = 0; i <= 1.5f; i += Time.deltaTime / duration){
			bool inner = true;

			for (int k = 0; k < 2; k++){
				for (int j = 0; j < 5; j++){

					trail.FindProperty(indexStrings[j]).colorValue = Color.Lerp(inner ? innerMinColour : outerMinColour,
											     								inner ? innerMaxColour : outerMaxColour,
												 								i - (i * .1f));
				}
				inner = false;
				trail.ApplyModifiedProperties();
				trail = outerObj;
			}

			yield return null;
		}
	}
*/

	IEnumerator BoostUp(){
		float rotationDegrees = 5;
		while (true){
			innerBoost.transform.Rotate(0, rotationDegrees, 0);
			outerBoost.transform.Rotate(0, -rotationDegrees, 0);
			rotationDegrees += Random.Range(-.1f, .1f);
			yield return null;
		}
	}

	IEnumerator ScaleBoostUp(){
		float duration = .2f;
		float percent = innerBoost.transform.localScale.x / 1;

		for (float i = percent; i <= 1; i += Time.deltaTime / duration){
			innerBoost.transform.localScale = new Vector3(i, i, i);
			outerBoost.transform.localScale = new Vector3(i, i, i);
			yield return null;
		}
	}

	IEnumerator ScaleBoostDown(){
		float duration = .2f;
		float percent = innerBoost.transform.localScale.x / 1;

		for (float i = percent; i >= 0; i -= Time.deltaTime / duration){
			innerBoost.transform.localScale = new Vector3(i, i, i);
			outerBoost.transform.localScale = new Vector3(i, i, i);
			yield return null;
		}

		innerBoost.transform.localScale = Vector3.zero;
		outerBoost.transform.localScale = Vector3.zero;
	}







}
