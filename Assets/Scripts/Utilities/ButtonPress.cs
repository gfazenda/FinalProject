using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
        [SerializeField]
        [Tooltip("How long must pointer be down on this object to trigger a long press")]
        private float holdTime = 1f;



    public SkillInfo skillPanel;
    public string skillName;
    // Remove all comment tags (except this one) to handle the onClick event!
        private bool held = false;
        public UnityEvent onClick = new UnityEvent();

        public UnityEvent onLongPress = new UnityEvent();

        public void OnPointerDown(PointerEventData eventData)
        {
            held = false;
            Invoke("OnLongPress", holdTime);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            CancelInvoke("OnLongPress");

            if (!held){
                onClick.Invoke();
                skillPanel.DisablePanel();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CancelInvoke("OnLongPress");
        }

        private void OnLongPress()
        {
            held = true;
            //onLongPress.Invoke();
            skillPanel.ShowSkillDetails(skillName);
            Debug.Log("heres the info");
        }
}
