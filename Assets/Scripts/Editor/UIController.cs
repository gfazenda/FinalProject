using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

//[CustomEditor(typeof(UIController))]
public class UIController : MonoBehaviour
{
    public List<Image> buttons;
    public bool autoPopulate = true, horizontal = true, configureAtRuntime = true;
    [Tooltip("Values in screen % (1 to 100)")]
    public float sizeX, offset, sizeY;

    [Tooltip("1st button position")]
    public Vector2 startPosition;

    void Start ()
    {
        if (autoPopulate)
        {
            buttons.AddRange(this.GetComponentsInChildren<Image>());
        }

        if(configureAtRuntime)
            configureUI();
    }
	
   public void configureUI()
    {
       float mSizeX = sizeX * 0.01f; //% to 1/100
       float mOffset = offset * 0.01f; //% to 1/100
       float mSizeY = sizeY * 0.01f;//% to 1/100

        if (horizontal)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                float x = startPosition.x + (mSizeX * i) + (i > 0 ? (mOffset * i) : 0);
                buttons[i].GetComponent<RectTransform>().anchorMin = new Vector2((float)(x), startPosition.y);
                buttons[i].GetComponent<RectTransform>().anchorMax = new Vector2((float)(x + mSizeX), (float)startPosition.y + mSizeY);
            }
        }
        else
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                float y = startPosition.y + (mSizeY * i) + (i > 0 ? (mOffset * i) : 0);
                buttons[i].GetComponent<RectTransform>().anchorMin = new Vector2(startPosition.x, (float)y);
                buttons[i].GetComponent<RectTransform>().anchorMax = new Vector2((float)startPosition.x + mSizeX, (float)(y + mSizeY));
            }
        }
    }
}
