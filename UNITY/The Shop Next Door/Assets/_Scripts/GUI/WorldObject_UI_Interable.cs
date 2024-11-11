using UnityEngine;
using UnityEngine.EventSystems;

public class WorldObject_UI_Interable : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private UI_ShelvesProducts[] products;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(canvas.activeSelf) { canvas.SetActive(false); }
        else
        {
            products[0].UpdateQuantityFillImage();
            products[0].UpdateShelvesQuantityProduct_UI();

            products[1].UpdateQuantityFillImage();
            products[1].UpdateShelvesQuantityProduct_UI();

            products[2].UpdateQuantityFillImage();
            products[2].UpdateShelvesQuantityProduct_UI();

            canvas.SetActive(true); 
        }
    }
}
