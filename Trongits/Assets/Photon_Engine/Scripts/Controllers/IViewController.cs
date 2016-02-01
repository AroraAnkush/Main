using ExitGames.Client.Photon;


    public interface IViewController
    {
        bool isConnected { get; }
        void ApplicationQuit();
        void Connect();

        void SendOperation(OperationRequest request, bool sendReliable, byte ChannelId, bool encrypt);


        #region Implementation of IPhotonPeerListener

        void DebugReturn(DebugLevel level, string message);
        void OnOperationResponse(OperationResponse response);
        void OnEvent(EventData eventdata);

        void OnUnexpectedEvent(EventData eventdata);
        void OnUnexpectedOperationResponse(OperationResponse operationresponse);
        void OnUnexpectedStatusCode(StatusCode statuscode);

        void OnDisconnected(string message);
        #endregion
    }
