using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour {

    public float effectTime;
    public bool animateAtStartup = true, animateOnEnable = false;

    [Space(5)]
    [Header("Position")]
    public Vector2 startPosition;
    public AnimationCurve positionCurve;


    [Space(5)]
    [Header("Scale")]
    public Vector3 startScale;
    public AnimationCurve scaleCurve;


    private Vector3 endScale;
    private Vector2 endPosition;
    RectTransform objTransform;
    Vector2 objSize = new Vector2();

    // Use this for initialization
    private void Awake()
    {
        InitializeVariables();
    }


    void Start () {
        if(animateAtStartup)
            StartCoroutine(Animate());
	}

    private void OnEnable()
    {
        if (!animateAtStartup && animateOnEnable)
            StartCoroutine(Animate());
    }

    

    void InitializeVariables()
    {
        objTransform = this.GetComponent<RectTransform>();
        endPosition = objTransform.anchorMin;
        endScale = transform.localScale;
        objSize.x = objTransform.anchorMax.x - objTransform.anchorMin.x;
        objSize.y = objTransform.anchorMax.y - objTransform.anchorMin.y;
    }


	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space)){
            StartCoroutine(Animate());
        }
	}

    void SetPosition(Vector2 position)
    {
        objTransform.anchorMin = position;
        objTransform.anchorMax = new Vector2(position.x+objSize.x,position.y+objSize.y);

    }

    IEnumerator Animate()
    {
        SetPosition(startPosition);
        transform.localScale = startScale;
        float time = 0, perc = 0, lasTime = Time.realtimeSinceStartup;
        Vector3 tempScale = new Vector3();
        Vector2 tempPos = new Vector2();
        do
        {
            time += Time.realtimeSinceStartup - lasTime;
            lasTime = Time.realtimeSinceStartup;
            perc = Mathf.Clamp01(time / effectTime);
            tempScale = Vector3.LerpUnclamped(startScale, endScale, scaleCurve.Evaluate(perc));
            tempPos = Vector2.LerpUnclamped(startPosition,endPosition, positionCurve.Evaluate(perc));
            transform.localScale = tempScale;
            SetPosition(tempPos);
            yield return null;

        } while (perc < 1);

        transform.localScale = endScale;
        SetPosition(endPosition);
        yield return null;
    }



}
