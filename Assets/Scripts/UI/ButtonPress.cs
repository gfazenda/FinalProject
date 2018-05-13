using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField]
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    private float holdTime = 1f;

    // Remove all comment tags (except this one) to handle the onClick event!
    private bool held = false;
    public UnityEvent onClick = new UnityEvent();

    public UnityEvent onLongPress = new UnityEvent();
    public UnityEvent onPressReleased = new UnityEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Interactable())
        {
            Debug.Log("wehwerkjhewrkwerk");
            held = false;
            Invoke("OnLongPress", holdTime);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");

        if (!held && Interactable()) {
            onClick.Invoke();
            //skillPanel.DisablePanel();
        }else
        {
            OnPressReleased();
        }
    }

    bool Interactable()
    {
        return this.GetComponent<Button>().IsInteractable();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");
    }

    private void OnPressReleased()
    {
        onPressReleased.Invoke();
        held = false;
    }

        private void OnLongPress()
        {
            held = true;
            onLongPress.Invoke();
           // skillPanel.ShowSkillDetails(skillName);
          //  Debug.Log("heres the info");
        }
}
