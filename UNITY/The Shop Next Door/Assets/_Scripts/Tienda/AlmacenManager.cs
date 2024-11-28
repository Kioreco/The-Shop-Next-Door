using UnityEngine;

public class AlmacenManager : MonoBehaviour
{
    public readonly int maxEspacio = 100;
    public int espacioUsado = 0;
    //private Producto[] productos;

    #region Singleton
    public static AlmacenManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion



    public void SaveSupplyInAlmacen(string product, char type, int quantity)
    {
        int quantityProductInAlmacen = TiendaManager.Instance.GetAlmacenQuantityOfProduct(product, type);

        if (quantityProductInAlmacen + quantity <= maxEspacio)
        {
            TiendaManager.Instance.UpdateProductQuantity(product, type, quantity);
            espacioUsado += quantity;

            UIManager.Instance.UpdateAlmacenSpace_UI();
        }
    }
}
