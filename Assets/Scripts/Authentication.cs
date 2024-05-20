using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using System.Threading.Tasks;
using UnityEngine.Events;
using TMPro;

public class Authentication : MonoBehaviour
{
    [SerializeField] private string email;
    [SerializeField] private string password;
    private AuSO _User;

    private FirebaseAuth _authReference;

    public UnityEvent OnLogInSuccesful = new();
    public UnityEvent OnLogOutSuccesful = new();
    public UnityEvent OnRegisterSuccesful = new();

    private void Awake()
    {
        _authReference = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
    }
    public void SetEmail(string user)
    {
        email = user;
    }
    public void SetPassword(string pass)
    {
        password = pass;
    }
    public void SignUp()
    {
        StartCoroutine(RegisterUser(email, password));
    }

    public void SignIn()
    {
        StartCoroutine(SignInWithEmail(email, password));
    }

    public void SignOut()
    {
        LogOut();
    }

    public void LoadNewScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StorageScene");
    }

    private IEnumerator RegisterUser(string email, string password)
    {
        Debug.Log("Registering");
        var registerTask = _authReference.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {registerTask.Exception}");
        }
        else
        {
            OnRegisterSuccesful?.Invoke();
            email = "";
            password = "";
            Debug.Log($"Succesfully registered user {registerTask.Result.User.Email}");
        }
    }

    private IEnumerator SignInWithEmail(string email, string password)
    {
        Debug.Log("Loggin In");

        var loginTask = _authReference.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogWarning($"Login failed with {loginTask.Exception}");
        }
        else
        {
            _User._correoID = loginTask.Result.User.Email;
            _User._Name = loginTask.Result.User.UserId;
            Debug.Log($"Login succeeded with {loginTask.Result.User.Email}");
            OnLogInSuccesful?.Invoke();
        }
    }

    private void LogOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        OnLogOutSuccesful?.Invoke();
    }
}
