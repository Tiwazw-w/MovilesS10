using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;

public class UserDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text userText;

    private void Start()
    {
        CheckUser();
    }

    private void CheckUser()
    {
        if(FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;

            string userName;

            if (currentUser == null)
            {
                userName = "NULL";
            }

            userName = currentUser.Email;
            userText.text = userName;
        }
        else
        {
            userText.text = "No user";
        }
    }

    public void UpdateUserText()
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;

        string userName;

        if(currentUser == null)
        {
            userName = "NULL";
        }

        userName = currentUser.Email;
        userName = currentUser.UserId;
        userText.text = userName;
    }
}
