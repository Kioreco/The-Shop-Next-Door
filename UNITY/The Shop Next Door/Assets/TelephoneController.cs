using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneController : MonoBehaviour
{
    [SerializeField] private GameObject MiniTelephone;
    [SerializeField] private GameObject LockedScreen;
    [SerializeField] private GameObject ShopApp;
    [SerializeField] private GameObject CalendarApp;

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
}
