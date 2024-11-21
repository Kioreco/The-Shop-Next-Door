using UnityEngine;
using UnityEngine.EventSystems;

public class WorldObject_UI_Interable : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject canvasAlert;

    [SerializeField] private UI_ShelvesProducts[] products;
    private bool alertIsShowing = false;

    private void Start()
    {
        foreach (var product in products) 
        { 
            product.UpdateQuantityFillImage();
            product.UpdateShelvesQuantityProduct_UI();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(canvas.activeSelf) { canvas.SetActive(false); if (alertIsShowing) { canvasAlert.SetActive(true); } }
        else
        {
            products[0].UpdateQuantityFillImage();
            products[0].UpdateShelvesQuantityProduct_UI();

            products[1].UpdateQuantityFillImage();
            products[1].UpdateShelvesQuantityProduct_UI();

            products[2].UpdateQuantityFillImage();
            products[2].UpdateShelvesQuantityProduct_UI();

            if (products[0].emptyProduct || products[1].emptyProduct || products[2].emptyProduct)
            {
                alertIsShowing = true;
            }
            else
            {
                alertIsShowing = false;
            }

            if (canvasAlert.activeSelf) { canvasAlert.SetActive(false); }
            canvas.SetActive(true);
        }
    }
}
