using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelephoneController : MonoBehaviour
{
    [Header("Telephone Parts")]
    [SerializeField] private GameObject MiniTelephone;
    [SerializeField] private GameObject LockedScreen;
    [SerializeField] private GameObject ShopApp;
    [SerializeField] private GameObject CalendarApp;

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

    [Header("Calendar App")]
    [SerializeField] public CalendarController calendar;

    private void OnEnable()
    {
        MiniTelephone.SetActive(false);
    }

    public void HomeButton()
    {
        if (LockedScreen.activeSelf)
        {
            this.gameObject.SetActive(false);
            MiniTelephone.SetActive(true);
        }
        else
        {
            if(ShopApp.activeSelf) ShopApp.SetActive(false);
            if(CalendarApp.activeSelf) CalendarApp.SetActive(false);
            LockedScreen.SetActive(true);
        }
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
    }

    public void BuySupply(Supply producto)
    {
        if (producto.CanBuySuply())
        {
            GameManager.Instance.dineroJugador -= producto.precio;
            UIManager.Instance.UpdatePlayersIngameMoney_UI();
        }
    }

}
