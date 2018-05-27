using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextAnimator : MonoBehaviour {
    public float effectTime;
    public Vector3 endScale;
    Vector3 originalScale, targetScale;

    public Color endColor;
    bool animating = false;
    Color startColor, currentColor, originalColor;
    Text text;

    void Start () {
        originalScale = transform.localScale;
        text = this.GetComponent<Text>();
        startColor = originalColor = text.color;
        currentColor = new Color();
        StartCoroutine(Animate());
    }

    private void LateUpdate()
    {
        if(!animating)
            StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
     
        float time = 0, perc = 0, lasTime = Time.realtimeSinceStartup;
        animating = true;
        //  Vector3 tempScale = new Vector3();
        // Vector2 tempPos = new Vector2();
        Color tempColor = new Color();
        do
        {
            time += Time.realtimeSinceStartup - lasTime;
            lasTime = Time.realtimeSinceStartup;
            perc = Mathf.Clamp01(time / effectTime);
            //tempScale = Vector3.LerpUnclamped(startScale, endScale, scaleCurve.Evaluate(perc));
            //tempPos = Vector2.LerpUnclamped(startPosition, endPosition, positionCurve.Evaluate(perc));
            currentColor = Color.Lerp(startColor, endColor, perc);
            // transform.localScale = tempScale;
            // SetPosition(tempPos);
            text.color = currentColor;
            yield return null;

        } while (perc < 1);

        endColor = startColor;
        startColor = currentColor;

        animating = false;
        // transform.localScale = endScale;
        // SetPosition(endPosition);
        yield return null;
    }
}
