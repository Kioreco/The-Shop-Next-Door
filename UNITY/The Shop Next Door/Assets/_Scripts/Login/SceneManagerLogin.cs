using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerLogin : MonoBehaviour
{
    [Header("Login")] 
    [SerializeField] private TMP_InputField _logPasswordInput = null;
    [SerializeField] private TMP_InputField _logUserInput = null;
    [SerializeField] private TextMeshProUGUI _logResultReq = null;

    [Header("Register")]
    [SerializeField] private TMP_InputField _userNameInput = null;
    [SerializeField] private TMP_InputField _emailInput = null;
    [SerializeField] private TMP_InputField _password = null;
    [SerializeField] private TMP_InputField _rePassword = null;
    [SerializeField] private TextMeshProUGUI _resultReq = null;

    private NetworkManagerLogin _networkManager = null;
    private UIManager _UIM;

    private void Awake()
    {
        _networkManager = GameObject.FindObjectOfType<@NetworkManagerLogin>();
        _UIM = UIManager.Instance;
    }

    public void SubmitLogin()
    {
        if (_logUserInput.text == "" || _logPasswordInput.text == "")
        {
            _logResultReq.text = "Some fields are empty, fill in all";
            return;
        }

        _logResultReq.text = "Processing...";

        _networkManager.CheckUser(_logUserInput.text, _logPasswordInput.text,
            delegate (Response response)
            {
                _logResultReq.text = response.msg;
                ////////_UIM.ChangeScene("2 - Matchmaking");
            });
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
