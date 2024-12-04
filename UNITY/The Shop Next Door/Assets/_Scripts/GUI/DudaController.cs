using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class DudaController : MonoBehaviour
{
    private bool isAvailable;
    private bool isGoodProduct;
    private bool isGossipTalk;
    private bool isComplaint;
    private bool isFlirting;

    public string dudaText;
    public string dudaAnswerYes;
    public string dudaAnswerNo;
    

    //1. Duda de stock (se que no te gusta la idea paula, pero por meter diferentes tipos)
    //2. Duda de si el producto sale bien
    //3. Duda de chachara/cotillear(un texto y que quizás tengas que decir aham y no se que?)
    //4. Duda de queja de basura o falta de productos
    //4. Duda de ligar(si da tiempo a controlarlas


    public void CreateDuda()
    {
        int tipoDuda = 0;

        if (VidaPersonalManager.Instance.hasPartner) { tipoDuda = Random.Range(0, 4); }
        else { tipoDuda = Random.Range(0, 5); }

        switch (tipoDuda)
        {
            case 0:
                CreateProductAvailableDuda();
                break;
            case 1:
                CreateQualityProductDuda();
                break;
            case 2:
                CreateGossipDuda();
                break;
            case 3:
                CreateComplaintDuda();
                break;
            case 4:
                CreateFlirtingDuda();
                break;
        }
    }

    private string[] productAvailableTexts = 
    {
        "Excuse me, superstar!\r\nDo you happen to have <i>this</i>?\r\n\r\nMy life kinda depends on it.",
        "Hey there, champion of retail!\r\nI'm looking for something like <i>this</i>.\r\n\r\nAm I in the right shop?",
        "Hi hi hi, amazing human!\r\nAny chance you've got <i>this</i>?\r\n\r\nI don't want to go to The Shop Next Door...",
        "Greetings, lady!\r\nI need something that looks like <i>this</i>.\r\n\r\nPlease tell me it’s in stock!",
        "Hey you, my savior!\r\nDo you have <i>this</i>?\r\n\r\nIt's my last hope before I go to The Shop Next Door!",
        "Hello...\r\nPlease, tell me you have <i>this</i>lying around?\r\n\r\nMy wallet is ready!",
        "Pardon me!\r\nI’m hunting for <i>this</i>.\r\n\r\nDo you have it or should I go to The Shop Next Door?",
        "Hey hey hey beautiful lady,\r\ndo you have this product that is something like <i>this</i>?\r\n\r\nI've been searching and I can't seem to find it..."
    };

    private string[] productAvailableYesAnswersTexts =
    {
        "Yes, of course!\r\nIt's right here...",
        "Of course!\r\nWhy wouldn’t we? It’s the Beyoncé of products, after all",
        "Yes, indeed! It’s over there, pretending to be humble but loving the attention",
        "Oh, for sure!\r\nWe stocked it just so you could ask this very question",
        "Absolutely!\r\nIt’s right here, being as extra as you are"
    };
    private string[] productAvailableNoAnswersTexts =
    {
        "No, sorry...\r\nBut come back soon!",
        "Sadly, no!\r\nBut if I had it, I’d probably keep it for myself—it's that good.",
        "Oh no, sorry!\r\nIt vanished into the void...",
        "Unfortunately, no!\r\nBut I promise I’ll cry about it with you later",
        "Not this time, unfortunately!"
    };


    private void CreateProductAvailableDuda()
    {
        dudaText = productAvailableTexts[Random.Range(0, productAvailableTexts.Length)];
        dudaAnswerYes = productAvailableYesAnswersTexts[Random.Range(0, productAvailableYesAnswersTexts.Length)];
        dudaAnswerNo = productAvailableNoAnswersTexts[Random.Range(0, productAvailableNoAnswersTexts.Length)];
    }

    private void CreateQualityProductDuda()
    {
        dudaText = productAvailableTexts[Random.Range(0, productAvailableTexts.Length)];
        dudaAnswerYes = productAvailableYesAnswersTexts[Random.Range(0, productAvailableYesAnswersTexts.Length)];
        dudaAnswerNo = productAvailableNoAnswersTexts[Random.Range(0, productAvailableNoAnswersTexts.Length)];
    }
    private void CreateGossipDuda()
    {
        dudaText = productAvailableTexts[Random.Range(0, productAvailableTexts.Length)];
        dudaAnswerYes = productAvailableYesAnswersTexts[Random.Range(0, productAvailableYesAnswersTexts.Length)];
        dudaAnswerNo = productAvailableNoAnswersTexts[Random.Range(0, productAvailableNoAnswersTexts.Length)];
    }

    private void CreateComplaintDuda()
    {
        dudaText = productAvailableTexts[Random.Range(0, productAvailableTexts.Length)];
        dudaAnswerYes = productAvailableYesAnswersTexts[Random.Range(0, productAvailableYesAnswersTexts.Length)];
        dudaAnswerNo = productAvailableNoAnswersTexts[Random.Range(0, productAvailableNoAnswersTexts.Length)];
    }
    private void CreateFlirtingDuda()
    {
        dudaText = productAvailableTexts[Random.Range(0, productAvailableTexts.Length)];
        dudaAnswerYes = productAvailableYesAnswersTexts[Random.Range(0, productAvailableYesAnswersTexts.Length)];
        dudaAnswerNo = productAvailableNoAnswersTexts[Random.Range(0, productAvailableNoAnswersTexts.Length)];
    }
}
