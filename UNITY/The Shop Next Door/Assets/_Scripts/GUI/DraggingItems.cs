using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggingItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    private Vector2 initialPosition;
    private Vector3 initialScale;

    [SerializeField] public TextMeshProUGUI activity_text;
    [SerializeField] private CanvasGroup canvas_group;
    [SerializeField] private Image activity_image;
    [SerializeField] public Activity activity;

    private void Awake()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
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

        gameObject.transform.localScale = initialScale;
        transform.position = initialPosition;
    }


    public void ActivateActivity()
    {
        activity_image.color = new Color(1.0f, 1.0f, 1.0f);
        enabled = true;
    }

    public void DesactiveActivity()
    {
        activity_image.color = new Color(0.65f, 0.82f, 0.82f);
        transform.position = initialPosition;
        gameObject.transform.localScale = gameObject.transform.localScale = initialScale;
        canvas_group.blocksRaycasts = true;
        enabled = false;
    }
}