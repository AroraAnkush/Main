using ExitGames.Client.Photon;



    public interface IGameState
    {
        void OnUpdate();
        void SendOperation(OperationRequest request, bool reliable, byte Channelid, bool encrypt);

    }
