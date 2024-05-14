using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using UnityEngine.Events;

public class FirebaseManager : MonoBehaviour
{
    private static FirebaseManager _instance;

    public static FirebaseManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = LazyInitFirebaseManager();
            }

            return _instance;
        }
    }

    private FirebaseAuth _auth;

    public FirebaseAuth Auth
    {
        get
        {
            if(_auth == null)
            {
                _auth = FirebaseAuth.GetAuth(App);
            }
            FirebaseAuth.DefaultInstance.SignOut();
            return _auth;
        }
    }

    private FirebaseApp _app;

    public FirebaseApp App
    {
        get
        {
            if(_app == null)
            {
                _app = GetAppSyncrhonous();
            }

            return _app;
        }
    }

    public UnityEvent OnFirebaseInitialized = new UnityEvent();

    private async void Awake()
    {
        if(_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;

            var dependencyResult = await FirebaseApp.CheckAndFixDependenciesAsync();
            if(dependencyResult == DependencyStatus.Available)
            {
                _app = FirebaseApp.DefaultInstance;
                OnFirebaseInitialized?.Invoke();
            }
            else
            {
                Debug.LogError($"Failed to initialize Firebase with {dependencyResult}");
            }

            Debug.Log(Instance);
            Debug.Log(Auth);
            Debug.Log(App);
        }
        else
        {
            Debug.LogError($"An instance of {nameof(FirebaseManager)} already exists!");
        }
    }

    private void OnDestroy()
    {
        _auth = null;
        _app = null;

        if(_instance == this)
        {
            _instance = null;
        }
    }

    private FirebaseApp GetAppSyncrhonous()
    {
        return FirebaseApp.DefaultInstance;
    }

    private static FirebaseManager LazyInitFirebaseManager()
    {
        return new FirebaseManager();
    }
}
