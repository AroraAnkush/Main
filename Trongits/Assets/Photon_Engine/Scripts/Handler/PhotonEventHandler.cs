
using System;
using ExitGames.Client.Photon;

public abstract class PhotonEventHandler : IPhotonEventHandler
{
    private readonly ViewController _Controller;

    public delegate void BeforeEventRecieved();
    public BeforeEventRecieved beforeEventRecieved;

    public delegate void AfterEventRecieved();
    public AfterEventRecieved afterEventRecieved;
    protected PhotonEventHandler(ViewController controller)
    {
        _Controller = controller;
    }

    public abstract byte code
    {
        get;
    }

    public void HandleEvent(EventData eventdata)
    {
        if(beforeEventRecieved != null)
        {
            beforeEventRecieved();
        }
        OnHandleEvent(eventdata);
        if(afterEventRecieved != null)
        {
            afterEventRecieved();
        }
    }

    public abstract void OnHandleEvent(EventData eventdata);
   
}

