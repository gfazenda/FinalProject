using UnityEngine;
using System.Collections;

public class lerpColor : MonoBehaviour {
    Color currentColor, originalColor; 
    public Color targetColor;
    float duration, currentTime;
    bool startLerp;
    public bool changeSize = false, _colorLerp = true;
    Vector3 originalSize;
    public float scale = 2f;

    public enum MeshType
    {
        Renderer, SkinnedMesh
    }

    enum LerpType {Color, Alpha}
    LerpType _lerpType;
    public MeshType meshType = MeshType.Renderer;
	// Use this for initialization
	void Start () {
        getColor();

        originalSize = this.transform.localScale;
    }

    void getColor()
    {
        switch (meshType)
        {
            case MeshType.Renderer: {
                    originalColor = this.GetComponent<Renderer>().material.color;
            }
            break;
            case MeshType.SkinnedMesh:{
                    originalColor = this.GetComponent<SkinnedMeshRenderer>().material.color;
            }
            break;

        }
    }

    void setColor(Color currentColor)
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

    //public void Lerp(Color a, Color b, float duration)
    //{
    //    if (startLerp)
    //        return;
    //    currentTime = 0;
    //    ColorA = a;// a;
    //    ColorB = currentColor;
    //    this.duration = duration;
    //    startLerp = true;
    //}

    public void Lerp(float duration)
    {
        this.duration = duration;
        _lerpType = LerpType.Color;
        setLerp();
    }

    public void lerpAlpha(float duration)
    {       
        _lerpType = LerpType.Alpha;
        this.duration = duration;
        currentColor = originalColor;

        setLerp();   
    }

    void setLerp()
    {
        startLerp = true;
        currentTime = 0;
        setColor(originalColor);
    }


    // Update is called once per frame
    void Update () {
        if (!startLerp)
            return;
        if (currentTime >= 1.0f)
            startLerp = false;

        currentTime += Time.deltaTime / duration;

        switch (_lerpType)
        {
            case LerpType.Alpha:
                currentColor.a = Mathf.Lerp(0, 1, currentTime);//Color.Lerp(targetColor, originalColor, currentTime);
                break;
            case LerpType.Color:
                currentColor = Color.Lerp(targetColor, originalColor, currentTime);
                break;
        }
       
        if(_colorLerp)
            setColor(currentColor);
        //this.GetComponent<Renderer>().material.color = currentColor;
        //this.GetComponent<SkinnedMeshRenderer>().material.color = currentColor;
        if (changeSize)
        {
            float size = Mathf.Lerp(scale, 1, currentTime);
            
            this.transform.localScale = new Vector3(originalSize.x * size, originalSize.y * size, originalSize.z * size);
        }
            

        
	}
}
