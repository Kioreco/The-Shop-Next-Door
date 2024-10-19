using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour, IDropHandler
{
    [SerializeField] private TextMeshProUGUI text_zone;
    [SerializeField] private int dropZoneNumber;
    [SerializeField] private CalendarController calendarController;

    public void OnDrop(PointerEventData eventData)
    {
        DraggingItems objectDropped = eventData.pointerDrag.GetComponent<DraggingItems>();

        TextMeshProUGUI draggingItemText = objectDropped.activity_text;
        text_zone.SetText(draggingItemText.text);

        objectDropped.activity_on_calendar = true;
        objectDropped.ActivitySelected();

        calendarController.activities_selected[dropZoneNumber].CopyActivity(objectDropped.activity);
        print(calendarController.activities_selected[dropZoneNumber].activityName);
    }
}
