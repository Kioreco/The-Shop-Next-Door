using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggingItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    private Vector2 initialPosition;

    [SerializeField] public TextMeshProUGUI activity_text;
    [SerializeField] private CanvasGroup canvas_group;
    [SerializeField] private Image activity_image;
    [SerializeField] public Activity activity;
    public bool activity_on_calendar = false;

    private void Awake()
    {
        initialPosition = transform.position;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        canvas_group.blocksRaycasts = false;
        gameObject.transform.localScale = gameObject.transform.localScale * 0.75f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 selectedPosition
            ))
        {
            transform.position = canvas.transform.TransformPoint(selectedPosition);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvas_group.blocksRaycasts = true;

        gameObject.transform.localScale = gameObject.transform.localScale * 1.25f;
        transform.position = initialPosition;
    }

    public void ActivitySelected()
    {
        
        if(activity_on_calendar)
        {
            activity_image.color = new Color(166, 209, 209);
        }
        else
        {
            activity_image.color = new Color(255, 255, 255);
        }
    }
}