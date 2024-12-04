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
        "Excuse me, superstar!\r\nDo you happen to have <i>this</i>?\r\n\r\nMy life kinda depends on it",
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
        "Of course!\r\nIt’s the Beyoncé of products, after all",
        "Yes, indeed!\r\nIt’s over there, somewhere",
        "Oh, for sure!\r\nWe stocked it just for you!",
        "Absolutely!\r\nIt’s right here, being as extra as you are"
    };
    private string[] productAvailableNoAnswersTexts =
    {
        "No, sorry...\r\nBut come back soon!",
        "Sadly, no!\r\nBut I’d probably keep it for myself",
        "Oh no, sorry!\r\nIt vanished into the void...",
        "Unfortunately, no!\r\nBut I’ll cry about it later",
        "Not this time, unfortunately!"
    };


    private void CreateProductAvailableDuda()
    {
        dudaText = productAvailableTexts[Random.Range(0, productAvailableTexts.Length)];
        dudaAnswerYes = productAvailableYesAnswersTexts[Random.Range(0, productAvailableYesAnswersTexts.Length)];
        dudaAnswerNo = productAvailableNoAnswersTexts[Random.Range(0, productAvailableNoAnswersTexts.Length)];
    }

    private string[] productQualityTexts =
    {
        "Be honest...\r\nWill <i>this product</i> last longer than Jess after saying <i>I love you</i> to Rory?",
        "On a scale of 1 to Beyoncé,\r\nHow good is <i>this product</i>?",
        "Quick question...\r\nDoes <i>this product</i> scream 'Mike Faist in Challengers' or just quietly whisper 'Katy Perry after Witness'?",
        "Does <i>this product</i> come pre-approved by moms everywhere,\r\nor just yours? Don't get me wrong I love Mrs. Telma...",
        "Is <i>this product</i> worth my money,\r\nor should I spend it on coffee in The Shop Next Door?",
        "Would you gift <i>this product</i> to someone you like,\r\nor to your mother?",
        "How would you rate <i>this product</i>:\r\ncowboy’s kiss or horse’s disappointment?",
        "Hi...\r\nWill <i>this product</i> be good for a try-hard swiftie?\r\nAsking for… a friend"
    };

    private string[] productQualityYesAnswersTexts =
    {
        "Definitely! Like a cowboy’s kiss moment",
        "Telma would want you to have it!",
        "That product is Beyonce level for sure!",
        "Buy one for your bestie and other for your oomfie!",
        "Best product ever... Like EVER!"
    };
    private string[] productQualityNoAnswersTexts =
    {
        "Honestly, it's on Katy Perry's level...",
        "Frankly, it's like Rory and Dean in season 4...",
        "Telma would scream at me me if she saw this...",
        "You should never buy that again... Like NEVER!",
        "I wouldn't even gift it to my enemy..."
    };

    private void CreateQualityProductDuda()
    {
        dudaText = productQualityTexts[Random.Range(0, productQualityTexts.Length)];
        dudaAnswerYes = productQualityYesAnswersTexts[Random.Range(0, productQualityYesAnswersTexts.Length)];
        dudaAnswerNo = productQualityNoAnswersTexts[Random.Range(0, productQualityNoAnswersTexts.Length)];
    }

    private string[] GossipTexts =
    {
        "Did you hear about Mariana from down the street?\r\nShe bought three of those fancy coffee machines, but I swear,\r\nshe still gets her latte from the gas station every morning.\r\nGiving major Lorelai vibes, don’t you think?",
        "So apparently, Karen’s husband got caught sneaking out at 2 a.m.\r\nShe says he was just 'feeding the raccoons.' Sure, Antonieta...\r\nSounds like the plot of a Desperate Housewives episode,\r\nminus the... you know",
        "So, the 'Challengers Monthly Rewatch' meeting last night?\r\nTotal disaster.\r\nApparently, Florencia called Estelaria’s cupcakes 'store-bought' in front of everyone.\r\nCan you imagine? I mean, she wasn’t wrong, but still!",
        "Okay, so my sister’s best friend just swore off dating apps\r\nbecause she met a guy who only communicates in Taylor Swift lyrics.\r\nI mean, that’s dedication, right?\r\nBut also maybe she should look after his 'Music Videos Watch Parties'...",
        "Mr. Gylmorian from The Bazaar Next Door has been sneaking\r\ninto his wife’s 'Twilight Analysis' class to bring her coffee\r\nbecause she says she’s too busy to stop by the diner.\r\nWhat a Luke move, right?",
        "I heard someone tried to return their blender twice\r\nbecause it didn’t 'spark joy.'\r\nSweetheart, this isn’t Marie Kondo.\r\nIt’s a blender!",
        "I saw Joselito crying in his car the other day.\r\nHe said it was because of the 'This Is Me Trying' bridge,\r\nbut honestly, I think it’s about his missing child",
        "Last night at the rodeo bar,\r\na guy ordered whiskey neat and called me 'ma’am.'\r\nI almost fainted.\r\nIs this what the wild west felt like?!",
        "So apparently, the rodeo is in town next week.\r\nDo I care about the horses? No.\r\nAm I going for the smoldering lasso tricks and Mike Fais impersonators?\r\nAbsolutely",
        "You remember Paul, the shy guy from down the street?\r\nI saw him wearing a leather vest yesterday, and I’m not okay.\r\nTurns out, quiet cowboys hit different",
        "You know those cowboy romance novels\r\nwith the overly dramatic covers?\r\nI thought they were silly until I saw Jake Gyllenhaal in Brokeback Mountain.\r\nNow I get it",
        "I swear, there’s just something about a man\r\nwho knows how to saddle a horse. It’s like,\r\n'Yes, sir, you can also saddle <i>my emotions</i>'"
    };

    private string[] GossipYesAnswersTexts =
    {
        "I knew something was up! You always have the best tea",
        "Stop it! That’s juicier than Love Island",
        "You’ve got to be kidding me! That’s wild",
        "No way! Honestly, I’m not even surprised",
        "Oh my goodness, really? Tell me everything!"
    };
    private string[] GossipNoAnswersTexts =
    {
        "I don’t know... That sounds a bit far-fetched",
        "Okay, but where’s the proof?",
        "Do I look like I’ve got time for this?",
        "Amazing. Now, can I get back to work?",
        "Do you ever think about not distracting people?"
    };

    private void CreateGossipDuda()
    {
        dudaText = GossipTexts[Random.Range(0, GossipTexts.Length)];
        dudaAnswerYes = GossipYesAnswersTexts[Random.Range(0, GossipYesAnswersTexts.Length)];
        dudaAnswerNo = GossipNoAnswersTexts[Random.Range(0, GossipNoAnswersTexts.Length)];
    }

    private string[] ComplaintTexts =
    {
        "I swear, your store’s been stuck in 2006,\r\nlike, are you even trying to keep up with trends like\r\nTaylor Swift’s new album?",
        "I was hoping this place would be more\r\nTaylor Swift-like—fun, fresh, and easy to navigate,\r\nbut nope",
        "This is the worst!\r\nI’ve seen better service at a cowboy bar\r\nin the middle of nowhere!",
        "I can't believe how slow the service is here.\r\nYou people need to get it together or\r\nI’ll be writing a review, mark my words!",
        "I don’t understand how this store\r\nis still in business with this level of incompetence.\r\nWhere is the manager? I want to speak to them right now!\r\nTelma! Where is Telma?!",
        "It’s like you all don’t even try.\r\nI’ve been to three different stores today,\r\nand this one is by far the worst followed by\r\nThe Shop Next Door",
        "This store is a joke.\r\nI don't even remember why I came...\r\nI’m leaving and never returning again.\r\nYou can count on that!",
        "Your hair is horrible!\r\nI can't even look at it.\r\nI want to talk to the manager have them force you\r\nto change that broom hair!"
    };

    private string[] ComplaintYesAnswersTexts =
    {
        "Sorry for that! I’ll fix it now",
        "I’ll handle this right away. Apologies!",
        "Telma's Daughter is on her way. Sorry!",
        "I’ll sort it out for you. Thanks for your patience",
        "Best product ever... Like EVER!"
    };
    private string[] ComplaintNoAnswersTexts =
    {
        "Ok. KAREN.",
        "You seem a little bit crazy",
        "I can change the world if you want!",
        "Go away, you are ugly",
        "Complaining doesn't suit you very well"
    };

    private void CreateComplaintDuda()
    {
        dudaText = productAvailableTexts[Random.Range(0, productAvailableTexts.Length)];
        dudaAnswerYes = productAvailableYesAnswersTexts[Random.Range(0, productAvailableYesAnswersTexts.Length)];
        dudaAnswerNo = GossipNoAnswersTexts[Random.Range(0, GossipNoAnswersTexts.Length)];
    }
    private void CreateFlirtingDuda()
    {
        dudaText = productAvailableTexts[Random.Range(0, productAvailableTexts.Length)];
        dudaAnswerYes = productAvailableYesAnswersTexts[Random.Range(0, productAvailableYesAnswersTexts.Length)];
        dudaAnswerNo = productAvailableNoAnswersTexts[Random.Range(0, productAvailableNoAnswersTexts.Length)];
    }
}
