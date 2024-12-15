using UnityEngine;
using UnityEngine.EventSystems;

public class WorldObject_UI_Interable : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject canvasAlert;

    [SerializeField] private UI_ShelvesProducts[] products;
    private bool alertIsShowing = false;
    private int maxProductsInShelves;

    private void Start()
    {
        UpdateUIProducts();
        maxProductsInShelves = products.Length;
    }

    public void UpdateUIProducts()
    {
        foreach (var product in products)
        {
            product.UpdateShelvesQuantityProduct_UI();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(canvas.activeSelf) { canvas.SetActive(false); if (alertIsShowing) { canvasAlert.SetActive(true); } }
        else
        {
            products[0].UpdateShelvesQuantityProduct_UI();

            products[1].UpdateShelvesQuantityProduct_UI();

            if (maxProductsInShelves == 3)
            {
                if (products[2].unlocked)
                {
                    products[2].UpdateShelvesQuantityProduct_UI();
                    if (products[2].emptyProduct) { alertIsShowing = true; }
                    else { alertIsShowing = false; }
                }
                if (products[0].emptyProduct || products[1].emptyProduct)
                {
                    alertIsShowing = true;
                }
                else
                {
                    alertIsShowing = false;
                }
            }
            else if (maxProductsInShelves == 4)
            {
                products[2].UpdateShelvesQuantityProduct_UI();


                if (products[3].unlocked)
                {
                    products[3].UpdateShelvesQuantityProduct_UI();
                    if (products[3].emptyProduct) { alertIsShowing = true; }
                    else { alertIsShowing = false; }
                }

                if (products[0].emptyProduct || products[1].emptyProduct || products[2].emptyProduct)
                {
                    alertIsShowing = true;
                }
                else
                {
                    alertIsShowing = false;
                }
            }
            else
            {
                if (products[0].emptyProduct || products[1].emptyProduct)
                {
                    alertIsShowing = true;
                }
                else
                {
                    alertIsShowing = false;
                }
            }
            

            if (canvasAlert.activeSelf) { canvasAlert.SetActive(false); }
            canvas.SetActive(true);
        }
    }
}
