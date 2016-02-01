
using ExitGames.Client.Photon;
using UnityEngine;
using System.Collections.Generic;
using ComplexServerCommon;



public class LoginController : ViewController
{
    public LoginController(View ControlledView, byte SubOperationCode = 0) : base(ControlledView, SubOperationCode)
    {
    }

    public void SendLogin(string username, string password)
    {
        Debug.Log("username==> " + username + "password==>" + password);
        SendOperation(new OperationRequest(), true, 0, false);
    }

    public void Register(string username, string email, string password)
    {

        var param = new Dictionary<byte, object>()
        {
            { (byte)ClientParameterCode.UserName, username},
            { (byte)ClientParameterCode.Email, email},
            { (byte)ClientParameterCode.Password, password },
            {(byte)ClientParameterCode.SubOperationCode, (int)MessageSubCode.Register }
        };
        SendOperation(new OperationRequest() { OperationCode = (byte)ClientOperationCode.Login, Parameters = param }, true, 0, false);



    }
}

