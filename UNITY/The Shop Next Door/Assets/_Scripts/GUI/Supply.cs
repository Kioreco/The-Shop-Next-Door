using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Supply : MonoBehaviour
{
    public float precio;
    public int cuantityToBuy;
    public int cuantityOwned;
    public Button buyButton;

    public TextMeshProUGUI cuantityAlmacen_text;

    public bool CanBuySuply()
    {
        if ((GameManager.Instance.espacioAlmacen + cuantityToBuy) < GameManager.Instance.maxEspacioAlmacen)
        {
            cuantityAlmacen_text.SetText((cuantityOwned + cuantityToBuy).ToString());
            cuantityOwned += cuantityToBuy;
            GameManager.Instance.espacioAlmacen += cuantityToBuy;
            return true;
        }
        else
        {
            buyButton.interactable = false;
            return false;
        }
    }
}
