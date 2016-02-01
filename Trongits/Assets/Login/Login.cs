using UnityEngine;
using System;
using System.Collections;

public class Login : View
{

    public string ServerAddress;
    public string ApplicationName;

    public bool loggingIn = false;

    public UIInput usernameobj;
    public UIInput passwordobj;
    public UIInput emailobj;
    public UIInput confirmpasswordobj;
    public string UserName;
    public string Email;
    public string Password;
    public string ConfirmPassword;

    public override void Awake()
    {

    }
    // Use this for initialization

    void Start()
    {

       
		_Controller = new LoginController(this);
		PhotonEngine.UseExistingOrCreateNewPhotonEngine(ServerAddress, ApplicationName);


    }

    public void StartLogin(UIInput username, UIInput password)
    {
        string[] arglist = new string[2];
        arglist[0] = username.value;
        arglist[1] = password.value;
        if (Application.srcValue.Split(new[] { "?" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
        {
            arglist = Application.srcValue.Split(new[] { "?" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new[] { "," },
                                                                                               StringSplitOptions.RemoveEmptyEntries);
        }
        if (arglist.Length == 2)
        {
            _Controller.SendLogin(arglist[0], arglist[1]);
            loggingIn = true;

        }
        UIManager.Instance.playername.text = arglist[0];
        print("logged in as " + arglist[0] + "and password is " + arglist[1]);
    }


    public void Register()
    {
        UserName = usernameobj.value;
        Password = passwordobj.value;
        Email = emailobj.value;
        ConfirmPassword = confirmpasswordobj.value;

        if (Password != ConfirmPassword)
        {
            Debug.LogError("password field are not same");
        }
        else
        {
            _Controller.Register(UserName, Email, Password);
        }
    }

    private LoginController _Controller;
    public override IViewController Controller
    {
        get
        {
            return (IViewController)_Controller;
        }

        protected set
        {
            _Controller = value as LoginController;
        }
    }
}
