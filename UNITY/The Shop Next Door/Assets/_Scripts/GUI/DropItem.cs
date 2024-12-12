using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour, IDropHandler
{
    [SerializeField] private TextMeshProUGUI text_zone;
    [SerializeField] private int dropZoneNumber;
    [SerializeField] private CalendarController calendarController;

    private DraggingItems[] draggingItems = new DraggingItems[3];

    public void OnDrop(PointerEventData eventData)
    {
        DraggingItems objectDropped = eventData.pointerDrag.GetComponent<DraggingItems>();

        //Si esa casilla estaba vacía, se guarda la actividad
        if (draggingItems[dropZoneNumber] != null)
        {
            draggingItems[dropZoneNumber].ActivateActivity();
        }

        draggingItems[dropZoneNumber] = objectDropped;

        //Se cambia el texto
        TextMeshProUGUI draggingItemText = objectDropped.activity_text;
        text_zone.SetText(draggingItemText.text);

        //Se marca el objeto como cogido
        objectDropped.DesactiveActivity();

        //Se guarda la actividad en la lista del calendario
        calendarController.activities_selected[dropZoneNumber].CopyActivity(objectDropped.activity.activityInfo);

        calendarController.hasChosenActivities = true;
    }

    public void ResetDropZone()
    {
        text_zone.SetText("");
        draggingItems[dropZoneNumber] = null;
    }
}
