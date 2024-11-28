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



    public int SaveSupplyInAlmacen(string product, char type, int quantity)
    {
        int quantityInAlmacen = TiendaManager.Instance.GetAlmacenQuantityOfProduct(product, type);

        //if (isGrabbing)
        //{
        //    if (quantityInAlmacen - quantity >= 0)
        //    {
        //        TiendaManager.Instance.UpdateProductQuantity(product, type, -quantity);
        //        espacioUsado -= quantity;
        //        return quantity;
        //    }
        //    else
        //    {
        //        return quantityInAlmacen;
        //    }
        //}
        //else
        //{
        //    if (quantityInAlmacen + quantity <= maxEspacio)
        //    {
        //        TiendaManager.Instance.UpdateProductQuantity(product, type, quantity);
        //        espacioUsado += quantity;
        //        return quantity;
        //    }
        //    else
        //    {
        //        return quantityInAlmacen;
        //    }
        //}
        if (quantityInAlmacen + quantity <= maxEspacio)
        {
            TiendaManager.Instance.UpdateProductQuantity(product, type, quantity);
            espacioUsado += quantity;
            return quantity;
        }
        else
        {
            return quantityInAlmacen;
        }
    }
}
