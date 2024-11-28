using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Supply : MonoBehaviour
{
    [Header("Producto")]
    public string producto;
    public char categoria;
    public int precio;
    public float descuentoEmpresa;
    public int rarity;

    [Header("Especificaciones Almacén")]
    public int quantityToBuy;
    public int quantityOwned;

    [Header("UI")]
    public Button buyButton;
    public TextMeshProUGUI quantityAlmacen_text;
    public TextMeshProUGUI quantityToBuy_text;
    public TextMeshProUGUI precioToBuy_text;

    private void Awake()
    {
        SetDescuentoEmpresa();

        SetQuantityOwned();
        SetQuantityToBuy();
        SetPrecio();
    }

    private void SetDescuentoEmpresa()
    {
        if (rarity == 1)
        {
            descuentoEmpresa = Random.Range(0.60f, 0.75f);
        }
        else if (rarity == 2)
        {
            descuentoEmpresa = Random.Range(0.75f, 0.85f);
        }
        else
        {
            descuentoEmpresa = Random.Range(0.90f, 0.95f);
        }
    }

    private void SetPrecio()
    {
        precio = (int)(TiendaManager.Instance.GetPrecioProductoIndividual(producto, categoria) * quantityToBuy * descuentoEmpresa);
        precioToBuy_text.SetText(precio.ToString() + " €");
    }

    private void SetQuantityOwned()
    {
        quantityOwned = TiendaManager.Instance.GetAlmacenQuantityOfProduct(producto, categoria) + TiendaManager.Instance.GetEstanteriaQuantityOfProduct(producto, categoria);
        quantityAlmacen_text.SetText(quantityOwned.ToString());
    }

    private void SetQuantityToBuy()
    {
        //Rarity 1 - Exclusive
        //Rarity 2 - Rare
        //Rarity 3 - Common

        if (rarity == 1)
        {
            quantityToBuy = Random.Range(1, 6);
            quantityToBuy_text.SetText(quantityToBuy + " items");
        }
        else if (rarity == 2)
        {
            quantityToBuy = Random.Range(6, 16);
        }
        else
        {
            quantityToBuy = Random.Range(16, 26);
        }

        quantityToBuy_text.SetText(quantityToBuy + " items");
    }

    public void Buy()
    {
        AlmacenManager.Instance.SaveSupplyInAlmacen(producto, categoria, quantityToBuy);
        //TiendaManager.Instance.UpdateProductQuantity(producto, categoria, quantityToBuy);

        SetQuantityOwned();

        //GameManager.Instance.espacioAlmacen += quantityToBuy;
        UIManager.Instance.UpdateAlmacenSpace_UI();
    }

    public bool CheckIfCanBuy()
    {
        if (TiendaManager.Instance.CheckIfCanBuyProduct(producto, categoria))
        {
            if ((AlmacenManager.Instance.espacioUsado + quantityToBuy) <= AlmacenManager.Instance.maxEspacio)
            {
                buyButton.interactable = true;
                return true;
            }
        }

        buyButton.interactable = false;
        return false;
    }

}
