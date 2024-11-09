using UnityEngine;
using UnityEngine.EventSystems;

public class WorldObject_UI_Interable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject canvas;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(canvas.activeSelf) { canvas.SetActive(false); }
        else { canvas.SetActive(true); }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
