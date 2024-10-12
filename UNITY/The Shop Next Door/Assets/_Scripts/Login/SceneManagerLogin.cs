using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerLogin : MonoBehaviour
{
    [SerializeField] private TMP_InputField _userNameInput = null;
    [SerializeField] private TMP_InputField _emailInput = null;
    [SerializeField] private TMP_InputField _password = null;
    [SerializeField] private TMP_InputField _rePassword = null;
    [SerializeField] private TextMeshProUGUI _resultReq = null;

    private NetworkManagerLogin _networkManager = null;

    private void Awake()
    {
        _networkManager = GameObject.FindObjectOfType<@NetworkManagerLogin>();
    }

    public void SubmitRegister()
    {
        if (_userNameInput.text == "" || _emailInput.text == "" || _password.text == "" || _rePassword.text == "")
        {
            _resultReq.text = "Some fields are empty, fill in all";
            return;
        }
        if (_password.text == _rePassword.text)
        {
            _resultReq.text = "Processing...";

            _networkManager.CreateUser(_userNameInput.text, _emailInput.text, _password.text,
                delegate(Response response)
                {
                    _resultReq.text = response.msg;
                });
        }
        else
        {
            _resultReq.text = "Different passwords, enter the same one";
        }
    }
}
