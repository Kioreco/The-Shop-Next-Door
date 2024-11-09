using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_ShelvesProducts : MonoBehaviour
{
    [SerializeField] private Estanteria shelve;

    [SerializeField] private string productName;

    [SerializeField] private Image quantityBackground;
    [SerializeField] private TextMeshProUGUI quantity_text;

    [SerializeField] private TextMeshProUGUI upgrade_text;
    [SerializeField] private GameObject upgrade_button;

    [SerializeField] private Sprite normalBackground;
    [SerializeField] private Sprite alertBackground;

    private void UpdateShelvesQuantityProduct_UI()
    {
        int quantity = shelve.CheckQuantityProduct(productName);
        if (quantity > 0)
        {
            quantity_text.SetText(quantity + "/" + shelve.CheckMaxQuantityProduct(productName));
        }
    }
}
