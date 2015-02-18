using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour {
    [SerializeField] protected Renderer shieldRenderer;
    Material mat;

    Color clearColor,
          displayColor;

    SafeCoroutine shieldCoroutine,
                  shieldFadeCoroutine;

    IEnumerator Primer(){
        yield break;
    }

    void Awake(){
        shieldCoroutine = this.StartSafeCoroutine(Primer());
        shieldFadeCoroutine = this.StartSafeCoroutine(Primer());
        mat = shieldRenderer.sharedMaterial;
        displayColor = new Color(0,
                                 181/255f,
                                 1f,
                                 46/255f);
        clearColor = displayColor;
        clearColor.a = 0;
    }

    public void ShieldHit(Transform target){
        KillShield();
        KillShieldFade();

        // Rotate accordingly
        transform.LookAt(target);

        shieldFadeCoroutine = this.StartSafeCoroutine(FadeShieldIn(true));
        shieldCoroutine = this.StartSafeCoroutine(ShieldHitCoroutine());
    }

    void RotateShield(Vector3 pos){
    }

    void KillShieldFade(){
        if (shieldFadeCoroutine.IsRunning){
            shieldFadeCoroutine.Stop();
        }
    }

    void KillShield(){
        if (shieldCoroutine.IsRunning){
            shieldCoroutine.Stop();
        }
    }

    void CleanUp(){
        KillShieldFade();
        shieldFadeCoroutine = this.StartSafeCoroutine(FadeShieldIn(false));
        mat.SetFloat("_MinEffectBound", 0);
        mat.SetFloat("_MaxEffectBound", 0);
    }

    IEnumerator FadeShieldIn(bool fadeIn){
        float inDuration = .25f,
              outDuration = 1f;
        float percent = mat.GetColor("_Color").a / displayColor.a;
        var col = displayColor;

        if (fadeIn){
            for (float i = percent; i <= 1; i += Time.deltaTime / inDuration){
                col.a = i * displayColor.a;
                mat.SetColor("_Color", col);
                yield return null;
            }
        }
        else{
            for (float i = percent; i >= 0; i -= Time.deltaTime / outDuration){
                col.a = i * displayColor.a;
                mat.SetColor("_Color", col);
                yield return null;
            }
            col.a = 0;
        }
    }

    IEnumerator ShieldHitCoroutine(){
        float difference = .04f;
        float a = 0f;
        float b = difference;
        float increment = .075f;

        while(a <= 1.3f){
            mat.SetFloat("_MinEffectBound", a += increment);
            mat.SetFloat("_MaxEffectBound", b = a + difference);
            yield return null;
        }

        CleanUp();
    }
}
