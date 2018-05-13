using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class lerpColor : MonoBehaviour
{
    Color currentColor, originalColor;
    public Color targetColor;
    float currentTime;
    bool startLerp;
    public bool changeSize = false, _colorLerp = true, repeat = false, lerpFromStart = false;

    Vector3 originalSize;
    public float scale = 2f, duration;
    public bool setAtRuntime = false;

    public Renderer mesh = null;

    public enum MeshType
    {
        Renderer, SkinnedMesh
    }

    enum LerpType { Color, Alpha }
    LerpType _lerpType;
    public MeshType meshType = MeshType.Renderer;
    // Use this for initialization
    void Start()
    {
        

        originalSize = this.transform.localScale;

        if(setAtRuntime)
            InitializeMesh();

        getColor();

        if (lerpFromStart)
            Lerp();
    }

    void InitializeMesh()
    {
            switch (meshType)
            {
                case MeshType.Renderer:
                    mesh = this.GetComponent<Renderer>();
                    break;
                case MeshType.SkinnedMesh:
                    mesh = this.GetComponent<SkinnedMeshRenderer>();
                    break;
        }
    }

    void getColor()
    {
       originalColor = mesh.material.color;
       
    }

    void setColor(Color color)
    {
        mesh.material.color = color;
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

    public void Lerp(float _duration = 0f)
    {
        this.duration = _duration == 0 ? duration : _duration;
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
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Lerp();
        }

        if (!startLerp)
            return;

        if (currentTime >= 1.0f)
        {
            if (repeat)
            {
                currentTime = 0f;
                targetColor = originalColor;
                originalColor = currentColor;
            }
            else
            {
                startLerp = false;
                setColor(originalColor);
           
            }
            return;
        }
         

        currentTime += Time.deltaTime / duration;

        switch (_lerpType)
        {
            case LerpType.Alpha:
                currentColor.a = Mathf.Lerp(0, 1, currentTime);//Color.Lerp(targetColor, originalColor, currentTime);
                break;
            case LerpType.Color:
                currentColor = Color.Lerp(originalColor, targetColor, currentTime);
                break;
        }

        if (_colorLerp)
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
