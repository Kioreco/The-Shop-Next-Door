using Assets.Scripts.MachineStates.Classes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Estanteria : MonoBehaviour
{
    public char tipoObj;
    public List<string> objetosEstanteria = new List<string>();
    public int maxSpacePerProduct = 10;

    private Dictionary<string, Producto> listaProductosEstanteria;
    [HideInInspector] public bool isRestocking = false;
    [HideInInspector] public string productRestocking;
    [HideInInspector] public UI_ShelvesProducts product_UI;

    [Header("Upgrade UI")]
    [SerializeField] private TextMeshProUGUI upgrade_text;
    [SerializeField] private GameObject upgrade_button;

    [HideInInspector] public int shelveLevel = 0;
    public int maxShelveLevels;
    public int[] upgradeCosts = new int[3] { 500, 800, 1000 };

    [SerializeField] private GameObject[] shelveLevelObjects;
    [SerializeField] public GameObject canvasInteractable;


    private void Awake()
    {
        listaProductosEstanteria = TiendaManager.Instance.getDictionaryAccType(tipoObj);
        upgrade_text.SetText(upgradeCosts[0] + " €");
    }

    public bool TieneElemento(string s)
    {
        if (objetosEstanteria.Contains(s)) return true;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.gameObject.GetComponent<ContextClienteGenerico>().setIsInColliderShelf(true);
        }

        if (other.CompareTag("Player") && isRestocking)
        {
            ReponerProductoEstanteria(productRestocking);
            //Animacion reponer
            product_UI.UpdateQuantityFillImage();
            product_UI.UpdateShelvesQuantityProduct_UI();
            productRestocking = null;
            product_UI = null;
            isRestocking = false;
            TiendaManager.Instance.player.enableMovement(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.gameObject.GetComponent<ContextClienteGenerico>().setIsInColliderShelf(false);
        }

        //if (other.CompareTag("Player"))
        //{
        //    //desactivar interfaz de acciones en la estantería
        //}
    }

    public int CheckQuantityProduct(string nombreProducto)
    {
        //print("Cantidad de producto " + TiendaManager.Instance.GetEstanteriaQuantityOfProduct(nombreProducto, tipoObj));
        return TiendaManager.Instance.GetEstanteriaQuantityOfProduct(nombreProducto, tipoObj);
    }

    private void ReponerProductoEstanteria(string nombreProducto)
    {
        listaProductosEstanteria[nombreProducto].gestionarStockEstanteriaYAlmacen(maxSpacePerProduct);
    }

    private void UpgradeShelve()
    {
        GameManager.Instance.dineroJugador -= upgradeCosts[shelveLevel];
        UIManager.Instance.UpdatePlayerMoney_UI();
        UIManager.Instance.UpdateNewMoney_UI(upgradeCosts[shelveLevel], false);

        canvasInteractable.SetActive(false);
        shelveLevelObjects[shelveLevel].SetActive(false);
        shelveLevel++;
        shelveLevelObjects[shelveLevel].SetActive(true);
        maxSpacePerProduct += 20;
    }

    public void UpgradeShelve_Button()
    {
        if (GameManager.Instance.dineroJugador - upgradeCosts[shelveLevel] > 0)
        {
            UpgradeShelve();
            upgrade_text.SetText(upgradeCosts[shelveLevel] + " €");
            if (shelveLevel + 1 == maxShelveLevels - 1)
            {
                upgrade_button.SetActive(false);
            }
        }
    }

}
