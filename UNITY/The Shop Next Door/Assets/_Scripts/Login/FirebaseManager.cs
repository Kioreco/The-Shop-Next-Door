using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using Firebase;
//using Firebase.Auth;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    //[Header("Firebase")]
    ////[SerializeField] private DependencyStatus _dependencyStatus;
    ////private FirebaseAuth _auth;
    ////private FirebaseUser _user;

    //[Space]
    //[Header("Login")]
    //[SerializeField] private TMP_InputField _emailLoginField;
    //[SerializeField] private TMP_InputField _passwordLogin;
    //[SerializeField] private TextMeshProUGUI _resultReqLogin = null;

    //[Space]
    //[Header("Registration")]
    //[SerializeField] private TMP_InputField _userNameRegister;
    //[SerializeField] private TMP_InputField _emailRegister;
    //[SerializeField] private TMP_InputField _passwordRegister;
    //[SerializeField] private TMP_InputField _rePasswordRegister;
    //[SerializeField] private TextMeshProUGUI _resultReqRegister = null;

    private void Awake()
    {
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //{
        //    _dependencyStatus = task.Result;

        //    if (_dependencyStatus == DependencyStatus.Available)
        //    {
        //        InitializeFirebase();
        //    }
        //    else
        //    {
        //        Debug.LogError("Could not resolve all firebase dependencies: " + _dependencyStatus);
        //    }
        //});
    }

    //void InitializeFirebase()
    //{
    //    _auth = FirebaseAuth.DefaultInstance;

    //    _auth.StateChanged += AuthStateChanged;
    //    AuthStateChanged(this, null);
    //}

    //void AuthStateChanged(object sender, System.EventArgs eventArgs)
    //{
    //    if (_auth.CurrentUser != _user)
    //    {
    //        bool signedIn = _user != _auth.CurrentUser && _auth.CurrentUser != null;

    //        if (!signedIn && _user != null)
    //        {
    //            Debug.Log("Signed out " + _user.UserId);
    //        }

    //        _user = _auth.CurrentUser;

    //        if (signedIn)
    //        {
    //            Debug.Log("Signed in " + _user.UserId);
    //        }
    //    }
    //}

    //public void Login()
    //{
    //    StartCoroutine(LoginAsync(_emailLoginField.text, _passwordLogin.text));
    //}

    //private IEnumerator LoginAsync(string email, string password)
    //{
    //    var loginTask = _auth.SignInWithEmailAndPasswordAsync(email, password);

    //    yield return new WaitUntil(() => loginTask.IsCompleted);

    //    if (loginTask.Exception != null)
    //    {
    //        //Debug.LogError(loginTask.Exception);

    //        FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
    //        AuthError authError = (AuthError)firebaseException.ErrorCode;


    //        string failedMessage = "Login Failed! Because ";

    //        switch (authError)
    //        {
    //            case AuthError.InvalidEmail:
    //                failedMessage += "Email is invalid";
    //                _resultReqLogin.text = "Email is invalid";
    //                break;
    //            case AuthError.WrongPassword:
    //                failedMessage += "Wrong Password";
    //                _resultReqLogin.text = "Wrong Password";
    //                break;
    //            case AuthError.MissingEmail:
    //                failedMessage += "Email is missing";
    //                _resultReqLogin.text = "Email is missing";
    //                break;
    //            case AuthError.MissingPassword:
    //                failedMessage += "Password is missing";
    //                _resultReqLogin.text = "Password is missing";
    //                break;
    //            default:
    //                failedMessage = "Login Failed";
    //                _resultReqLogin.text = "Login Failed";
    //                break;
    //        }

    //        //Debug.Log(failedMessage);
    //    }
    //    else
    //    {
    //        _user = loginTask.Result.User;

    //        Debug.LogFormat("{0} You Are Successfully Logged In", _user.DisplayName);
    //        _resultReqLogin.text = "Hello again " + _user.DisplayName;
    //    }
    //}

    //public void Register()
    //{
    //    StartCoroutine(RegisterAsync(_userNameRegister.text, _emailRegister.text, _passwordRegister.text, _rePasswordRegister.text));
    //}

    //private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    //{
    //    if (name == "")
    //    {
    //        //Debug.LogError("User Name is empty");
    //        _resultReqRegister.text = "User Name is empty";
    //    }
    //    else if (email == "")
    //    {
    //        //Debug.LogError("email field is empty");
    //        _resultReqRegister.text = "email field is empty";
    //    }
    //    else if (_passwordRegister.text != _rePasswordRegister.text)
    //    {
    //        //Debug.LogError("Password does not match");
    //        _resultReqRegister.text = "Password does not match";
    //    }
    //    else
    //    {
    //        var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);

    //        yield return new WaitUntil(() => registerTask.IsCompleted);

    //        if (registerTask.Exception != null)
    //        {
    //            //Debug.LogError(registerTask.Exception);

    //            FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
    //            AuthError authError = (AuthError)firebaseException.ErrorCode;

    //            string failedMessage = "Registration Failed! Because ";
    //            switch (authError)
    //            {
    //                case AuthError.InvalidEmail:
    //                    failedMessage += "Email is invalid";
    //                    _resultReqRegister.text = "Email is invalid";
    //                    break;
    //                case AuthError.WrongPassword:
    //                    failedMessage += "Wrong Password";
    //                    _resultReqRegister.text = "Wrong Password";
    //                    break;
    //                case AuthError.MissingEmail:
    //                    failedMessage += "Email is missing";
    //                    _resultReqRegister.text = "Email is missing";
    //                    break;
    //                case AuthError.MissingPassword:
    //                    failedMessage += "Password is missing";
    //                    _resultReqRegister.text = "Password is missing";
    //                    break;
    //                default:
    //                    failedMessage = "Registration Failed";
    //                    _resultReqRegister.text = "Registration Failed";
    //                    break;
    //            }

    //            //Debug.Log(failedMessage);
    //        }
    //        else
    //        {
    //            _user = registerTask.Result.User;

    //            UserProfile userProfile = new UserProfile { DisplayName = name };

    //            var updateProfileTask = _user.UpdateUserProfileAsync(userProfile);

    //            yield return new WaitUntil(() => updateProfileTask.IsCompleted);

    //            if (updateProfileTask.Exception != null)
    //            {
    //                _user.DeleteAsync();

    //                Debug.LogError(updateProfileTask.Exception);

    //                FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
    //                AuthError authError = (AuthError)firebaseException.ErrorCode;


    //                string failedMessage = "Profile update Failed! Because ";
    //                switch (authError)
    //                {
    //                    case AuthError.InvalidEmail:
    //                        failedMessage += "Email is invalid";
    //                        break;
    //                    case AuthError.WrongPassword:
    //                        failedMessage += "Wrong Password";
    //                        break;
    //                    case AuthError.MissingEmail:
    //                        failedMessage += "Email is missing";
    //                        break;
    //                    case AuthError.MissingPassword:
    //                        failedMessage += "Password is missing";
    //                        break;
    //                    default:
    //                        failedMessage = "Profile update Failed";
    //                        break;
    //                }

    //                //Debug.Log(failedMessage);
    //            }
    //            else
    //            {
    //                Debug.Log("Welcome " + _user.DisplayName + "!");
    //                _resultReqRegister.text = "Welcome " + _user.DisplayName + "!";
    //            }
    //        }
    //    }
    //}
}
