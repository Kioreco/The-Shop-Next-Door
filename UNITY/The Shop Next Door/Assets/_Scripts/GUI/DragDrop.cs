using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DragDrop : MonoBehaviour, IDragAndDropHandler
{
    [SerializeField] private InputAction objectClicked;
    [SerializeField] private Camera mainCamera;
    private float dragSpeed = .1f;
    private Vector2 velocity = Vector2.zero;


    private void OnEnable()
    {
        objectClicked.Enable();
        objectClicked.performed += ObjectSelected;
    }
    private void OnDisable()
    {
        objectClicked.performed -= ObjectSelected;
        objectClicked.Disable();
    }

    private void ObjectSelected(InputAction.CallbackContext context)
    {
        PerformDragAndDrop();
    }

    public DragAndDropVisualMode dragAndDropVisualMode => throw new System.NotImplementedException();

    public bool AcceptsDragAndDrop()
    {
        throw new System.NotImplementedException();
    }

    public void PerformDragAndDrop()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateDragAndDrop()
    {
        throw new System.NotImplementedException();
    }

    public void DrawDragAndDropPreview()
    {
        throw new System.NotImplementedException();
    }

    public void ExitDragAndDrop()
    {
        throw new System.NotImplementedException();
    }
}
