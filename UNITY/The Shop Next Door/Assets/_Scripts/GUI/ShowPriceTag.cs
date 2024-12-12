using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowPriceTag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject price;

    public void OnPointerEnter(PointerEventData eventData)
    {
        price.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        price.SetActive(false);
    }
}

