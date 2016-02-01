
using ExitGames.Client.Photon;

public class Connected : GameState
    {

        public Connected(PhotonEngine engine) : base(engine)
        { }

        public override void OnUpdate()
        {
            _engine._Peer.Service();
        }

        public override void SendOperation(OperationRequest request, bool reliable, byte Channelid, bool encrypt)
        {
            _engine._Peer.OpCustom(request, reliable, Channelid, encrypt);
        }
    }
