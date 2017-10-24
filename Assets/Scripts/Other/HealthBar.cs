using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    float HP = 1;
    public Image _image;
    public Text _text;
    public float showDuration = 0.5f;
    public bool lerp = false, scaleImage = true, hideBar = false, healthInPercent = false;
    public GameObject hpBar;
    void Start ()
    {
        if (hideBar)
            DisableBar();
       // _image = this.GetComponent<Image>();
    }

    void DisableBar()
    {
        hpBar.SetActive(false);
    }
	
    public void UpdateBar(float value)
    {
        HP = value;
        Vector3 scale = _image.transform.localScale;
        scale.x = value < 0 ? 0 : value;

        if (!hpBar.activeInHierarchy)
            hpBar.SetActive(true);

        _image.transform.localScale = scale;

        if (lerp)
            setColor();
        if (hideBar)
        {
            CancelInvoke();
            Invoke("DisableBar", showDuration);
        }
    }

    public void UpdateBarWithText(float current, float max)
    {
        Vector3 scale = _image.transform.localScale;

        HP = current / max; 
        scale.x = HP < 0 ? 0 : HP;

        SetText(current, max, scale.x);

        if (scaleImage)
            _image.transform.localScale = scale;
        if (lerp)
            setColor();
    }

    void SetText(float current, float max, float scale)
    {
        if (healthInPercent)
            _text.text = (int)(scale * 100f) + "%";
        else
        {
            _text.text = current + " / " + max;
        }
    }

    void setColor()
    {
        _image.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, HP);
    }
}
