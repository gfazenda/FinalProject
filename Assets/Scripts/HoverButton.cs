using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text title;

    private void Start()
    {
        title.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        title.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        title.gameObject.SetActive(false);
    }
}

