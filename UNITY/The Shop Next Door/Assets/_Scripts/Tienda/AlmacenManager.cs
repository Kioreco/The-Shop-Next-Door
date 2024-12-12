using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AlmacenManager : MonoBehaviour
{
    public int maxEspacio = 100;
    public int espacioUsado = 0;
    [SerializeField] private TextMeshProUGUI warehouseCapacity;
    [SerializeField] private TextMeshProUGUI priceToUpgrade_text;
    [SerializeField] private Button upgradeButton;

    private int[] priceToUpgrade;
    private int upgradeLevel;

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

    private void Start()
    {
        priceToUpgrade = new int[3] { 600, 800, 1000 };
        upgradeLevel = 0;
        priceToUpgrade_text.SetText(priceToUpgrade[upgradeLevel] + " €");
    }

    public void UpdateWarehouseCapacity_UI()
    {
        warehouseCapacity.SetText(espacioUsado + " / " + maxEspacio);
    }

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

    public void UpgradeCapacity()
    {
        if (GameManager.Instance.dineroJugador - priceToUpgrade[upgradeLevel] >= 0)
        {
            GameManager.Instance.dineroJugador -= priceToUpgrade[upgradeLevel];
            UIManager.Instance.UpdatePlayerMoney_UI();
            UIManager.Instance.UpdateNewMoney_UI(priceToUpgrade[upgradeLevel], false);

            maxEspacio += 50;
            UIManager.Instance.UpdateAlmacenSpace_UI();

            upgradeLevel++;
            if (upgradeLevel == 2)
            {
                priceToUpgrade_text.gameObject.SetActive(false);
                upgradeButton.interactable = false;
            }
            else
            {
                priceToUpgrade_text.SetText(priceToUpgrade[upgradeLevel] + " €");
            }
        }
    }
}
