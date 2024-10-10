using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerLogin : MonoBehaviour
{
    [SerializeField] private InputField _userNameInput = null;
    [SerializeField] private InputField _emailInput = null;
    [SerializeField] private InputField _password = null;
    [SerializeField] private InputField _rePassword = null;
    [SerializeField] private Text _resultReq = null;

    private NetworkManagerLogin _networkManager = null;

    private void Awake()
    {
        _networkManager = GameObject.FindObjectOfType<NetworkManagerLogin>();
    }

    public void SubmitLogin()
    {
        if (_userNameInput.text == "" || _emailInput.text == "" || _password.text == "" || _rePassword.text == "")
        {
            _resultReq.text = "Algún/os campos vacíos, rellene todos";
            return;
        }
        if (_password.text == _rePassword.text)
        {
            _resultReq.text = "Procesando...";

            _networkManager.CreateUser(_userNameInput.text, _emailInput.text, _password.text,
                delegate(Response response)
                {
                    _resultReq.text = response.msg;
                });
        }
        else
        {
            _resultReq.text = "Contraseñas distintas, introduzca la misma";
        }
    }
}
