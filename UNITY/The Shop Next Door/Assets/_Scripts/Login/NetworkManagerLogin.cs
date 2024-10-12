using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerLogin : MonoBehaviour
{
    public void CreateUser(string userName, string email, string pass, Action<Response> response)
    {
        StartCoroutine(CO_CreateUser(userName, email, pass, response));
    }

    private IEnumerator CO_CreateUser(string userName, string email, string pass, Action<Response> response)
    {
        WWWForm form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("email", email);
        form.AddField("pass", pass);

        //WWW w = new WWW("http://localhost/TheShopNextDoor/createUser.php", form);

        //yield return w;

        //response(JsonUtility.FromJson<Response>(w.text));

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/TheShopNextDoor/createUser.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error en la solicitud: " + www.error);
                response(new Response { done = false, msg = www.error });
            }
            else
            {
                Debug.Log("Respuesta recibida: " + www.downloadHandler.text);
                Response jsonResponse = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                response(jsonResponse);
            }
        }
    }

}

[Serializable]
public class Response
{
    public bool done = false;
    public string msg = "";
}

