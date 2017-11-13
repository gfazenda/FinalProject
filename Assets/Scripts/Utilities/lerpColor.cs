using UnityEngine;
using System.Collections;

public class lerpColor : MonoBehaviour {
    public Color startColor;
    public Color finalColor;
    Color currentColor;

    public enum MeshType
    {
        Renderer, SkinnedMesh
    }
    public MeshType meshType = MeshType.Renderer;

    public enum LerpType {Color, Alpha}
    public LerpType _lerpType;


    public float lerpDuration;
    float lerpTimer;
    public bool repeat = false;


	// Use this for initialization
	void Start () {
        currentColor = startColor;
    }

    void getColor()
    {
        //switch (meshType)
        //{
        //    case MeshType.Renderer: {
        //            originalColor = this.GetComponent<Renderer>().material.color;
        //    }
        //    break;
        //    case MeshType.SkinnedMesh:{
        //            originalColor = this.GetComponent<SkinnedMeshRenderer>().material.color;
        //    }
        //    break;

        //}
    }

    void UpdateColor()
    {
        switch (meshType)
        {
            case MeshType.Renderer:
                {
                    this.GetComponent<Renderer>().material.color = currentColor;
                }
                break;
            case MeshType.SkinnedMesh:
                {
                    this.GetComponent<SkinnedMeshRenderer>().material.color = currentColor;
                }
                break;

        }
    }


   

    //public void Lerp(float duration)
    //{
    //    this.duration = duration;
    //    _lerpType = LerpType.Color;
    //    setLerp();
    //}

    //public void LerpAlpha(float duration)
    //{       
    //    _lerpType = LerpType.Alpha;
    //    this.duration = duration;
    //    currentColor = originalColor;

    //    setLerp();   
    //}

    //void setLerp()
    //{
    //    startLerp = true;
    //    currentTime = 0;
    //    setColor(originalColor);
    //}


    // Update is called once per frame
    void Update () {

        lerpTimer += Time.deltaTime;
        float step = lerpTimer / lerpDuration;
        //transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        currentColor = Color.Lerp(startColor, finalColor , step);
        UpdateColor();

        if (repeat && step >= 1f)
        {
            lerpTimer = 0f;
            finalColor = startColor;
            startColor = currentColor;
        }
        //if (!startLerp)
        //    return;
        //if (currentTime >= 1.0f)
        //{
        //    initialAlpha = currentColor.a;
        //    finalAlpha = 1f - initialAlpha;
        //}
        ////    startLerp = false;

        //currentTime += Time.deltaTime / duration;

        //switch (_lerpType)
        //{
        //    case LerpType.Alpha:
        //        currentColor.a = Mathf.Lerp(initialAlpha, finalAlpha, currentTime);//Color.Lerp(targetColor, originalColor, currentTime);
        //        break;
        //    case LerpType.Color:
        //        currentColor = Color.Lerp(targetColor, originalColor, currentTime);
        //        break;
        //}

        //if(_colorLerp)
        //    setColor(currentColor);

        ////if (repeat)
        ////{

        ////}
        ////this.GetComponent<Renderer>().material.color = currentColor;
        ////this.GetComponent<SkinnedMeshRenderer>().material.color = currentColor;
        //if (changeSize)
        //{
        //    float size = Mathf.Lerp(scale, 1, currentTime);

        //    this.transform.localScale = new Vector3(originalSize.x * size, originalSize.y * size, originalSize.z * size);
        //}



    }
}
