
using ExitGames.Client.Photon;

public interface IPhotonEventHandler
{
    byte code { get; }

    void HandleEvent(EventData eventdata);
    void OnHandleEvent(EventData eventdata);
}

