using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AWSManager : MonoBehaviour
{
    public List<Skin> userSkins = new List<Skin>();

    [Space]
    [Header("Login")]
    [SerializeField] private TMP_InputField _usernameLogin;
    [SerializeField] private TMP_InputField _passwordLogin;
    [SerializeField] private TextMeshProUGUI _resultReqLogin = null;

    [Space]
    [Header("Registration")]
    [SerializeField] private TMP_InputField _usernameRegister;
    [SerializeField] private TMP_InputField _emailRegister;
    [SerializeField] private TMP_InputField _passwordRegister;
    [SerializeField] private TMP_InputField _rePasswordRegister;
    [SerializeField] private TextMeshProUGUI _resultReqRegister = null;

    private int idPlayer;


    public void Register()
    {
        if (_passwordRegister.text == _rePasswordRegister.text)
        {
            Application.ExternalCall("registerJs", _usernameRegister.text, _emailRegister.text, _passwordRegister.text);
        }
        else
        {
            _resultReqRegister.text = "Password does not match";
        }
    }

    public void Login()
    {
        Application.ExternalCall("loginJs", _usernameLogin.text, _passwordLogin.text);
    }

    public void GetUserId() //modifica para coger el name que está puesto como nombre de tienda = que el de user básicamente
    {
        Application.ExternalCall("getUserIdJs", _usernameLogin.text);
    }

    public void GetGems()
    {
        Application.ExternalCall("getGemsJs", _usernameLogin.text);
    }

    public void SetPlayerGems(string gems)
    {
        if (int.TryParse(gems, out int parsedGems))
        {
            //variable para almacenar las gemas = parsedGems;
            Debug.Log($"Player gems updated: {parsedGems}");
        }
        else
        {
            Debug.LogError("Failed to parse gems value from server response: " + gems);
        }
    }

    //public int GetPlayerId(string id)
    //{
    //    if (int.TryParse(id, out int parsedId))
    //    {
    //        //variable para almacenar el id = parsedGems;
    //        Debug.Log($"Player gems updated: {parsedId}");
    //        return parsedId;
    //    }
    //    else
    //    {
    //        Debug.LogError("Failed to parse id value from server response: " + id);
    //        return -1;
    //    }
    //}

    public void UpdateGems(int gems)
    {
        Application.ExternalCall("updateGemsJs", _usernameLogin.text, gems.ToString());
    }

    public void GetUserSkins(int userId)
    {
        Application.ExternalCall("getUserSkinsJs", userId.ToString());
    }

    public void BuySkin(int skinId)
    {
        Application.ExternalCall("buySkinJs", idPlayer.ToString(), skinId.ToString());
    }

    public void SetUserSkins(string jsonSkins)
    {
        try
        {
            // Muestra el JSON recibido para depuración
            Debug.Log("Raw JSON received: " + jsonSkins);

            // Deserializa el JSON en el contenedor SkinList
            SkinList skinList = JsonUtility.FromJson<SkinList>(jsonSkins);

            if (skinList != null && skinList.skins != null)
            {
                // Reemplaza la lista local con la lista deserializada
                userSkins = skinList.skins;

                // Muestra los datos de la skin para verificar que funciona
                if (userSkins.Count > 0)
                {
                    // Verifica el contenido de las skins
                    Debug.Log("Helper: " + userSkins[0]);
                    Debug.Log("Helper name: " + userSkins[0].name);
                    Debug.Log("Helper id: " + userSkins[0].id);

                    // Muestra todas las skins para verificar que esté bien
                    foreach (var skin in userSkins)
                    {
                        Debug.Log($"Skin ID: {skin.id}, Name: {skin.name}");
                    }
                }
                else
                {
                    Debug.LogError("No skins found in the list");
                }
            }
            else
            {
                Debug.LogError("Skin list is null or empty");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to parse skins JSON: {e.Message}");
        }
    }

    public void OnUserSkinsFetched(string skinsJson)
    {
        Debug.Log("Skins fetched from server: " + skinsJson);
        SetUserSkins(skinsJson);
    }

    public void OnUserIdFetched(string userId)
    {
        if (int.TryParse(userId, out int parsedUserId))
        {
            Debug.Log("User ID fetched successfully: " + parsedUserId);
            idPlayer = parsedUserId; //sino, se crea id en player o algo y se guarda ahí

            // Usa el ID para obtener las skins del usuario
            GetUserSkins(parsedUserId);
        }
        else
        {
            Debug.LogError("Failed to fetch or parse User ID: " + userId);
        }
    }

    public void OnBuySkinComplete(string response)
    {
        Debug.Log("Buy Skin Response: " + response);

        if (response == "Skin purchased successfully.")
        {
            Debug.Log("Skin purchase completed successfully.");
            //GetUserSkins(int.Parse(_usernameLogin.text));
        }
        else if (response == "Not enough gems.")
        {
            Debug.LogError("Not enough gems to purchase the skin.");
        }
        else if (response == "Skin not found.")
        {
            Debug.LogError("The skin does not exist.");
        }
        else if (response == "User not found.")
        {
            Debug.LogError("User ID is invalid.");
        }
        else
        {
            Debug.LogError("Unexpected response: " + response);
        }
    }

    public void OnJsHttpFuncComplete(string result)
    {
        Debug.Log("JS RESULT:");
        Debug.Log(result);

        if (result.StartsWith("{\"skins\":"))
        {
            SetUserSkins(result);
        }
        else if (result.StartsWith("Login successful."))
        {
            _resultReqLogin.text = "Login successful. Welcome again " + _usernameLogin.text + "!";
            GetGems();
            GetUserId();
        }

        else if (result.StartsWith("Invalid username or password."))
            _resultReqLogin.text = "Invalid username or password";

        else if (result == "User registered successfully.")
            _resultReqRegister.text = result + " Login to play";

        else if (result == "Username or email already exists.")
            _resultReqRegister.text = result;

        else if (result.StartsWith("Player gems:"))
        {
            string gemsString = result.Substring("Player gems:".Length).Trim();
            SetPlayerGems(gemsString);
        }
    }

    //public void OnJsHttpFuncComplete(string result)
    //{
    //    Debug.Log("JS RESULT:"); Debug.Log(result);
    //    if (result == "Login successful." || result == "Invalid username or password.")
    //        _resultReqLogin.text = result;
    //    else if (result == "User registered successfully." || result == "Username or email already exists.")
    //        _resultReqRegister.text = result;
    //}
}

[System.Serializable]
public class SkinList
{
    public List<Skin> skins;
}

//public static class JsonHelper
//{
//    public static T FromJson<T>(string json)
//    {
//        string newJson = "{\"Items\":" + json + "}";
//        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
//        return wrapper.Items;
//    }

//    [System.Serializable]
//    private class Wrapper<T>
//    {
//        public T Items;
//    }
//}
