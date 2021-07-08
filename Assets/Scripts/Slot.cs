using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Slot : MonoBehaviour, IDropHandler
{

    public MathManager mathmanager;

    public int id;
    public bool correct = false;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item Dropped");
        

        if (eventData.pointerDrag != null)
        {

            if (eventData.pointerDrag.GetComponent<DragDrop>().id == id)
            {
                Debug.Log("Correct");
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
                correct = true;
                mathmanager.CorrectMathAnswer();
                eventData.pointerDrag.GetComponent<DragDrop>().ResetPosition();
            }
            else
            {
                Debug.Log("InCorrect");
                mathmanager.IncorrectMathAnswer();
                eventData.pointerDrag.GetComponent<DragDrop>().ResetPosition();
                
            }
        }
    }


}
