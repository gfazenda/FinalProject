using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    float HP = 1;
    //Image _image;
    public Text _text;
    public bool lerp = false, scaleImage = true;

    void Start ()
    {
        //_image = this.GetComponent<Image>();
    }
	
	void Update ()
    {

    }

    public void updateBar(float value)
    {
        HP = value;
        Vector3 scale = this.transform.localScale;
        scale.x = value < 0 ? 0 : value;
        this.transform.localScale = scale;
        if (lerp) setColor();
    }

    public void updateBar(int current, int max)
    {
        Vector3 scale = this.transform.localScale;
       
        HP = (float)current / (float)max; 
        scale.x = HP < 0 ? 0 : HP;
        _text.text = (int)(scale.x * 100f) + "%";
        if (scaleImage)
            this.transform.localScale = scale;
        if (lerp)
            setColor();
    }

    void setColor()
    {
        this.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, HP);
    }
}
