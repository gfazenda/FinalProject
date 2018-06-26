using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerper : MonoBehaviour {
    public float effectTime;
    public Vector3 endScale;
    Vector3 originalScale, targetScale;

    public Color endColor;
    bool animating = false, useDiffColor = false;
    public bool lerpAtStartup = true, lerpScale = false, repeat = false;

    public Renderer mesh = null;
    Color startColor, currentColor, originalColor, differentColor;

    void Start () {
        originalScale = transform.localScale;
        startColor = originalColor = mesh.material.color;
        currentColor = new Color();

        if(lerpAtStartup)
            StartCoroutine(Lerp());
    }

    void setColor(Color color)
    {
        mesh.material.color = color;
    }


    public void DoLerp(float _duration = 0f)
    {
        effectTime = _duration == 0 ? effectTime : _duration;
        StartCoroutine(Lerp());
    }

    public void DoLerp(Color dColor, float _duration = 0f)
    {
        effectTime = _duration == 0 ? effectTime : _duration;
        differentColor = dColor;
        useDiffColor = true;
        StartCoroutine(Lerp());
    }

    IEnumerator Lerp()
    {
     
        float time = 0, perc = 0, lasTime = Time.realtimeSinceStartup;
        animating = true;
         Vector3 tempScale = new Vector3();
        // Vector2 tempPos = new Vector2();
        Color tempColor = new Color();
        do
        {
            time += Time.realtimeSinceStartup - lasTime;
            lasTime = Time.realtimeSinceStartup;
            perc = Mathf.Clamp01(time / effectTime);

            tempScale = Vector3.LerpUnclamped(endScale,originalScale, perc);
            //tempPos = Vector2.LerpUnclamped(startPosition, endPosition, positionCurve.Evaluate(perc));
            if (useDiffColor)
            {
                currentColor = Color.Lerp(differentColor, startColor, perc);
            }
            else
            {
                currentColor = Color.Lerp(endColor, startColor, perc);//repeat ? Color.Lerp(startColor, endColor, perc) : Color.Lerp(endColor, startColor, perc);
            }

            if (lerpScale)
                transform.localScale = tempScale;
            // SetPosition(tempPos);
            setColor(currentColor);
            yield return null;

        } while (perc < 1);

        if (repeat && !useDiffColor)
        {
            startColor = endColor;
            endColor = currentColor;
        }

        if (useDiffColor)
        {
            useDiffColor = false;
        }

        animating = false;
        // transform.localScale = endScale;
        // SetPosition(endPosition);
        yield return null;
    }

    private void Update()
    {
        if (!animating && repeat)
            StartCoroutine(Lerp());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Lerp());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            DoLerp(Color.yellow);
        }
    }
}
