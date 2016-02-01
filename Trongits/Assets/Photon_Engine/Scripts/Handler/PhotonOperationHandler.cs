
using System;
using ExitGames.Client.Photon;

public abstract class PhotonOperationHandler : IPhotonOperationHandler
{
    private readonly ViewController _Controller;

    public delegate void BeforeOperationRecieved();
    public BeforeOperationRecieved beforeOperationRecieved;

    public delegate void AfterOperationRecieved();
    public AfterOperationRecieved afterOperationRecieved;

    public abstract byte Code
    {
        get;
    }

    public void HandleResponse(OperationResponse response)
    {
        if (beforeOperationRecieved != null)
        {
            beforeOperationRecieved();
        }
        OnHandleResponse(response);
        if (afterOperationRecieved != null)
        {
            afterOperationRecieved();
        }
    }

    public abstract void OnHandleResponse(OperationResponse response);
   

  
}
