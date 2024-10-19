using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private Vector2 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerEventData = data as PointerEventData;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            pointerEventData.position,
            canvas.worldCamera,
            out Vector2 selectedPosition
            ))
        {
            transform.position = canvas.transform.TransformPoint(selectedPosition);
        }
    }

    public void EndDragHandler(BaseEventData data)
    {
        PointerEventData pointerEventData = data as PointerEventData;

        transform.position = initialPosition;

    }
}