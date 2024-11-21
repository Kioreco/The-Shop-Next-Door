using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TelephoneController : MonoBehaviour
{
    [Header("Telephone Parts")]
    [SerializeField] private GameObject MiniTelephone;
    [SerializeField] private TextMeshProUGUI hour_telephone_text;
    [SerializeField] private GameObject LockedScreen;
    [SerializeField] private Image LockedScreen_bg;
    [SerializeField] private Sprite LockedScreen_bg_1;
    [SerializeField] private Sprite LockedScreen_bg_2;
    [SerializeField] private GameObject ShopApp;
    [SerializeField] private GameObject CalendarApp;
    [SerializeField] private GameObject HireApp;
    [SerializeField] private GameObject LifeApp;

    [Header("Shop App")]
    [SerializeField] private ScrollRect contenedorTienda;

    [SerializeField] private bool theresClothes;
    [SerializeField] private bool theresFood;
    [SerializeField] private bool theresBooks;
    [SerializeField] private bool theresEntertainment;

    [SerializeField] private GameObject clothesContent;
    [SerializeField] private GameObject foodContent;
    [SerializeField] private GameObject booksContent;
    [SerializeField] private GameObject entertainmentContent;

    [Header("Shop App - Supplies")]
    [SerializeField] private Supply[] supplies;


    [Header("Calendar App")]
    [SerializeField] public CalendarController calendar;

    //[Header("Hire App")]
    //[SerializeField] public CalendarController calendar;

    [Header("Life App")]
    [SerializeField] private TextMeshProUGUI lifeApp_name;
    [SerializeField] public Life_RadarChart lifeRadar;

    private void Awake()
    {
        CheckAlmacenSpaceForBuying();
    }

    private void OnEnable()
    {
        MiniTelephone.SetActive(false);
        UIManager.Instance.ChangeVolumeEffects_Telephone(true);
        UIManager.Instance.vigor.vigorFill = true;
        UIManager.Instance.vigor.vigorDiminish = false;
        GameManager.Instance._player.disableMovement();
    }

    private void OnDisable()
    {
        UIManager.Instance.ChangeVolumeEffects_Telephone(false);
        GameManager.Instance._player.enableMovement(false);
        UIManager.Instance.vigor.vigorFill = false;
        UIManager.Instance.vigor.vigorDiminish = true;
    }

    public void HomeButton()
    {
        if (LockedScreen.activeSelf)
        {
            gameObject.SetActive(false);
            MiniTelephone.SetActive(true);

        }
        else
        {
            if(ShopApp.activeSelf) ShopApp.SetActive(false);
            if(CalendarApp.activeSelf) CalendarApp.SetActive(false);
            if(HireApp.activeSelf) HireApp.SetActive(false);
            if(LifeApp.activeSelf) LifeApp.SetActive(false);
            LockedScreen.SetActive(true);
        }
    }

    public void UpdateHourTelephone(string time)
    {
        hour_telephone_text.SetText(time);
    }

    public void ChangeLockedScreenBG(int player)
    {
        if(player == 1) { LockedScreen_bg.sprite = LockedScreen_bg_1; }
        if(player == 2) { LockedScreen_bg.sprite = LockedScreen_bg_2; }
    }

    public void ChangeLifeAppName(int player)
    {
        if (player == 1) { lifeApp_name.SetText("Gemma's Life"); }
        if (player == 2) { lifeApp_name.SetText("Emma's Life"); }
    }

    public void OpenApp(GameObject app)
    {
        app.SetActive(true);
        if(LockedScreen.activeSelf) LockedScreen.SetActive(false);
    }

    public void OpenShopCategory(GameObject content)
    {
        if(theresFood) { if (foodContent.activeSelf) { foodContent.SetActive(false); } }
        if(theresEntertainment) { if (entertainmentContent.activeSelf) { entertainmentContent.SetActive(false); } }
        if(theresBooks) { if (booksContent.activeSelf) { booksContent.SetActive(false); } }
        if(theresClothes) { if (clothesContent.activeSelf) { clothesContent.SetActive(false); } }
        
        content.SetActive(true);
        contenedorTienda.content = content.GetComponent<RectTransform>();

        CheckAlmacenSpaceForBuying();
    }

    public void BuySupply(Supply producto)
    {
        producto.Buy();
        GameManager.Instance.dineroJugador -= producto.precio;
        UIManager.Instance.UpdatePlayerMoney_UI();
        UIManager.Instance.UpdateNewMoney_UI(producto.precio, false);
        TiendaManager.Instance.FillStockWithSupplies(producto.producto, producto.quantityToBuy, producto.categoria);

        CheckAlmacenSpaceForBuying() ;
    }

    public void CheckAlmacenSpaceForBuying()
    {
        foreach(Supply supply in supplies)
        {
            supply.CheckIfCanBuy();
        }
    }
}
