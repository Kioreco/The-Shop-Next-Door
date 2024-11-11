using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShelvesProducts : MonoBehaviour, IDeselectHandler
{
    [SerializeField] private Estanteria shelve;

    [SerializeField] private string productName;

    //[SerializeField] private Image bubbleBackground;

    [SerializeField] private Image quantityBackground;
    [SerializeField] private GameObject quantityOverlay;
    [SerializeField] private TextMeshProUGUI quantity_text;

    //[SerializeField] private TextMeshProUGUI upgrade_text;
    //[SerializeField] private GameObject upgrade_button;

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

    public void OnDeselect(BaseEventData eventData)
    {
        quantityOverlay.SetActive(false);
    }


    public void ShowQuantities()
    {
        quantityOverlay.SetActive(true);
        UpdateShelvesQuantityProduct_UI();
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
            quantity_text.SetText(quantity.ToString() + "/" + shelve.CheckMaxQuantityProduct(productName).ToString());
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
        }
    }

    public void UpdateQuantityFillImage()
    {
        quantityBackground.fillAmount = shelve.CheckQuantityProduct(productName) / shelve.CheckMaxQuantityProduct(productName);

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
