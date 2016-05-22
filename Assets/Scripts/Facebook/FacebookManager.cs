using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FacebookManager : MonoBehaviour {

    // Use this for initialization
    void Awake () {
        Init ();
    }

    // Update is called once per frame
    void Update () {

    }

    void Init () {
        CallFBInit ();
    }

    public void loginFB () {
        CallFBLogin ();
    }

    public void logoutFB () {
        CallFBLogout ();
    }

    public string getAccessToken () {
        return FB.AccessToken;
    }

    //#region FB.Init() example

    private void CallFBInit () {
        FB.Init (OnInitComplete, OnHideUnity);
        //SystemInfo.
    }

    private void OnInitComplete () {
        Debug.Log ("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
    }

    private void OnHideUnity (bool isGameShown) {
        Debug.Log ("Is game showing? " + isGameShown);
    }

    //#endregion

    //#region FB.Login() example

    private void CallFBLogin () {
        FB.Login ("public_profile, email, user_friends", LoginCallback);
    }

    private void CallFBLoginForPublish () {
        // It is generally good behavior to split asking for read and publish
        // permissions rather than ask for them all at once.
        //
        // In your own game, consider postponing this call until the moment
        // you actually need it.
        FB.Login ("publish_actions", LoginCallback);
    }

    void LoginCallback (FBResult result) {
        if(result.Error != null)
            Debug.Log ("Error Response:\n" + result.Error);
        else if(!FB.IsLoggedIn) {
            Debug.Log ("Login cancelled by Player");
        } else {
            Debug.Log ("Login was successful!");
        }
    }

    private void CallFBLogout () {
        FB.Logout ();
    }
    //#endregion
}
