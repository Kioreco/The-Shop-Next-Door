using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TelephoneController : MonoBehaviour
{
    [Header("Telephone Parts")]
    [SerializeField] public GameObject MiniTelephone;
    [SerializeField] private GameObject EmptyCloseButton;
    [SerializeField] private TextMeshProUGUI hour_telephone_text;
    [SerializeField] private GameObject LockedScreen;
    [SerializeField] private Image LockedScreen_bg;
    [SerializeField] private Sprite LockedScreen_bg_1;
    [SerializeField] private Sprite LockedScreen_bg_2;
    [SerializeField] private GameObject ShopApp;
    [SerializeField] private GameObject CalendarApp;
    [SerializeField] private GameObject HireApp;
    [SerializeField] private GameObject LifeApp;
    [SerializeField] private GameObject SettingsApp;

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
    [SerializeField] private GameObject Warehouse;
    public Sprite warehouseItemBG_base;
    public Sprite warehouseItemBG_low;


    [Header("Calendar App")]
    [SerializeField] public CalendarController calendar;

    [Header("Settings App")]
    [SerializeField] public GameObject s_MainScreen;
    [SerializeField] public GameObject s_Music;
    [SerializeField] public GameObject s_Camera;
    [SerializeField] public GameObject s_Tutorial;

    [Header("Hire App")]
    [SerializeField] public HireApp hirer;

    [Header("Life App")]
    [SerializeField] private TextMeshProUGUI lifeApp_name;
    [SerializeField] public Life_RadarChart lifeRadar;

    private void Awake()
    {
        CheckAlmacenSpaceForBuying();
    }

    private void OnEnable()
    {
        LeanTween.moveY(MiniTelephone.GetComponent<RectTransform>(), -220.0f, 0.5f).setEaseInOutBounce();
        LeanTween.moveY(gameObject.GetComponent<RectTransform>(), -250.0f, 0.5f).setEaseInOutBounce();
        //MiniTelephone.SetActive(false);
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
        AudioManager.Instance.PlaySound("CloseApp");
        if (LockedScreen.activeInHierarchy)
        {
            LeanTween.moveY(MiniTelephone.GetComponent<RectTransform>(), 40.0f, 0.5f).setEaseInOutBounce();
            LeanTween.moveY(gameObject.GetComponent<RectTransform>(), -1120.0f, 0.5f).setEaseInOutBounce();
            //gameObject.SetActive(false);
            //MiniTelephone.SetActive(true);
            StartCoroutine(WaitToAnimation(0.5f));
            CheckAlmacenSpaceForBuying();
        }
        else if (SettingsApp.activeInHierarchy)
        {
            if (s_MainScreen.activeInHierarchy) { SettingsApp.SetActive(false); LockedScreen.SetActive(true); }
            else if (s_Music.activeInHierarchy) { s_Music.SetActive(false); s_MainScreen.SetActive(true); }
            else if (s_Camera.activeInHierarchy) { s_Camera.SetActive(false); s_MainScreen.SetActive(true); }
            else if (s_Tutorial.activeInHierarchy) { s_Tutorial.SetActive(false); s_MainScreen.SetActive(true); }
        }
        else
        {
            if (ShopApp.activeInHierarchy) { ShopApp.SetActive(false); Warehouse.SetActive(false); EmptyCloseButton.SetActive(true); }
            if (CalendarApp.activeInHierarchy) { CalendarApp.SetActive(false); EmptyCloseButton.SetActive(true); }
            if (HireApp.activeInHierarchy) { HireApp.SetActive(false); }
            if (LifeApp.activeInHierarchy) { LifeApp.SetActive(false); }
            LockedScreen.SetActive(true);
        }
    }

    IEnumerator WaitToAnimation(float timeAnimation)
    {
        yield return new WaitForSeconds(timeAnimation);

        gameObject.SetActive(false);
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
        AudioManager.Instance.PlaySound("OpenApp");
        app.SetActive(true);
        if (LockedScreen.activeInHierarchy) { LockedScreen.SetActive(false); }
        if (CalendarApp.activeInHierarchy) { EmptyCloseButton.SetActive(false); }
        if (ShopApp.activeInHierarchy) { EmptyCloseButton.SetActive(false); Warehouse.SetActive(true); }
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

        CheckAlmacenSpaceForBuying() ;
    }

    public void CheckAlmacenSpaceForBuying()
    {
        foreach(Supply supply in supplies)
        {
            supply.CheckIfCanBuy();
            supply.SetQuantityOwned();
        }
    }

    public void ResetTelephone()
    {
        Warehouse.SetActive(false);
        EmptyCloseButton.SetActive(true);

        LeanTween.moveY(MiniTelephone.GetComponent<RectTransform>(), 40.0f, 0.5f).setEaseInOutBounce();
        LeanTween.moveY(gameObject.GetComponent<RectTransform>(), -1120.0f, 0.5f).setEaseInOutBounce();

        MiniTelephone.SetActive(true);
        gameObject.SetActive(false);

        GameManager.Instance._player.enableMovement(false);

        if (SettingsApp.activeInHierarchy) 
        {
            s_MainScreen.SetActive(true);
            s_Music.SetActive(false);
            s_Camera.SetActive(false);
            s_Tutorial.SetActive(false);
            SettingsApp.SetActive(false);
        }
        if (ShopApp.activeInHierarchy) { ShopApp.SetActive(false); }
        if (CalendarApp.activeInHierarchy) { CalendarApp.SetActive(false); }
        if (HireApp.activeInHierarchy) { HireApp.SetActive(false); }
        if (LifeApp.activeInHierarchy) { LifeApp.SetActive(false); }
        LockedScreen.SetActive(true);
    }
}
