using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShelvesProducts : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Estanteria shelve;

    [SerializeField] private string productName;

    //[SerializeField] private Image bubbleBackground;

    [Header("Quantity UI")]
    [SerializeField] private Image quantityBackground;
    [SerializeField] private GameObject quantityOverlay;
    [SerializeField] private TextMeshProUGUI quantity_text;

    [Header("Sprites")]
    [SerializeField] private Sprite normalBackground;
    [SerializeField] private Sprite normalOverlay;
    [SerializeField] private Sprite normalPress;
    [SerializeField] private Sprite alertBackground;
    [SerializeField] private Sprite alertOverlay;
    [SerializeField] private Sprite alertPress;

    private bool spriteChanged = false;

    private Color colorGreen = new Color(0.05f, 0.78f, 0.29f);
    private Color colorOrange = new Color(0.78f, 0.78f, 0.78f);
    private Color colorRed = new Color(0.85f, 0.06f, 0.32f);
    private Color colorDarkRed = new Color(0.55f, 0.0f, 0.18f);
    private Color colorDarkBlue = new Color(0.14f, 0.45f, 0.51f);

    [HideInInspector] public bool emptyProduct = false; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowQuantities();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        quantityOverlay.SetActive(false);
    }

    public void RestockProduct_Button()
    {
        TiendaManager.Instance.player.disableMovement();
        TiendaManager.Instance.player.WalkToPosition(shelve.transform.position, false);
        shelve.isRestocking = true;
        shelve.productRestocking = productName;
        shelve.product_UI = this;
        shelve.canvasInteractable.SetActive(false);
        if (shelve.playerIsHere)
        {
            shelve.AccionReponerProducto();
        }
    }

    private void ShowQuantities()
    {
        UpdateShelvesQuantityProduct_UI();
        quantityOverlay.SetActive(true);
    }

    public void UpdateShelvesQuantityProduct_UI()
    {
        int quantity = shelve.CheckQuantityProduct(productName);
        if (quantity > 0)
        {
            if (spriteChanged)
            {
                gameObject.GetComponent<Image>().sprite = normalBackground;
                var spriteState = gameObject.GetComponent<Button>().spriteState;

                spriteState.selectedSprite = normalPress;
                spriteState.highlightedSprite = normalPress;
                spriteState.pressedSprite = normalPress;
                quantityOverlay.GetComponent<Image>().sprite = normalOverlay;

                gameObject.GetComponent<Button>().spriteState = spriteState;

                quantity_text.color = colorDarkBlue;

                spriteChanged = false;
            }
            quantity_text.SetText(quantity.ToString() + "/" + shelve.maxSpacePerProduct);
        }
        else
        {
            spriteChanged = true;

            gameObject.GetComponent<Image>().sprite = alertBackground;
            var spriteState = gameObject.GetComponent<Button>().spriteState;

            spriteState.selectedSprite = alertPress;
            spriteState.highlightedSprite = alertPress;
            spriteState.pressedSprite = alertPress;
            quantityOverlay.GetComponent<Image>().sprite = alertOverlay;

            gameObject.GetComponent<Button>().spriteState = spriteState;

            quantity_text.color = colorDarkRed;

            quantity_text.SetText(quantity.ToString() + "/" + shelve.maxSpacePerProduct);

            emptyProduct = true;
            if (shelve.canvasInteractable.activeSelf) { shelve.canvasAlert.SetActive(false); }
            else shelve.canvasAlert.SetActive(true);

            UIManager.Instance.alertTelephone.SetActive(true);
            
        }
    }

    public void UpdateQuantityFillImage()
    {
        quantityBackground.fillAmount = shelve.CheckQuantityProduct(productName) / shelve.maxSpacePerProduct;

        if (quantityBackground.fillAmount >= 0.5f)
        {
            quantityBackground.color = colorGreen;
        }
        else if (quantityBackground.fillAmount < 0.5f && quantityBackground.fillAmount > 0.2)
        {
            quantityBackground.color = colorOrange;
        }
        else
        {
            quantityBackground.color = colorRed;
        }
    }

    
    
}
