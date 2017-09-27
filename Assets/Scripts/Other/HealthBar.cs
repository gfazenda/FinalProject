using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    float HP = 1;
    public Image _image;
    public Text _text;
    public bool lerp = false, scaleImage = true;

    void Start ()
    {
       // _image = this.GetComponent<Image>();
    }
	
	void Update ()
    {

    }

    public void UpdateBar(float value)
    {
        HP = value;
        Vector3 scale = _image.transform.localScale;
        scale.x = value < 0 ? 0 : value;
        _image.transform.localScale = scale;
        if (lerp) setColor();
    }

    public void updateBar(int current, int max)
    {
        Vector3 scale = this.transform.localScale;
       
        HP = (float)current / (float)max; 
        scale.x = HP < 0 ? 0 : HP;
        _text.text = (int)(scale.x * 100f) + "%";
        if (scaleImage)
            _image.transform.localScale = scale;
        if (lerp)
            setColor();
    }

    void setColor()
    {
        _image.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, HP);
    }
}
