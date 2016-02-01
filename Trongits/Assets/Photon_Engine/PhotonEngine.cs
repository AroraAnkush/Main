
using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    public PhotonPeer _Peer { get; protected set; }
    public GameState _State { get; protected set; }
    public ViewController Controller { get; set; }

    public string ServerAddress;
    public string ApplicationName;

    private static PhotonEngine _Instance;

    public void Awake()
    {
        _Instance = this;
    }

    public void Start()
    {
        DontDestroyOnLoad(this);
        _State = new Disconnected(_Instance);
        Application.runInBackground = true;
        Initialize();
    }

    public static PhotonEngine Instance { get { return _Instance; } }

    public void Initialize()
    {
        _Peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        _Peer.Connect(ServerAddress, ApplicationName);
        _State = new WaitForConnection(this);
    }

    public void Disconnect()
    {
        if(_Peer != null)
        {
            _Peer.Disconnect();
            _State = new Disconnected(this);
        }
    }

    public void Update()
    {
        _State.OnUpdate();
		Debug.Log(_State.ToString());
    }

    public void SendOp(OperationRequest operationrequest, bool sendreliable, byte channelid, bool encrypt)
    {
        _State.SendOperation(operationrequest, sendreliable, channelid, encrypt);
    }

    public static void UseExistingOrCreateNewPhotonEngine(string serveraddress, string applicationname)
    {
        GameObject TempEngine;
        PhotonEngine MyEngine;

        TempEngine = GameObject.Find("PhotonEngine");
        if(TempEngine == null)
        {
            TempEngine = new GameObject("PhotonEngine");
            TempEngine.AddComponent<PhotonEngine>();
        }

        MyEngine = TempEngine.GetComponent<PhotonEngine>();
        MyEngine.ApplicationName = applicationname;
        MyEngine.ServerAddress = serveraddress;
    }

    #region Implementation of IPhotonPeerListener

    public void DebugReturn(DebugLevel level, string message)
    {
        Controller.DebugReturn(level, message);
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        Controller.OnOperationResponse(operationResponse);
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        switch(statusCode)
        {
            case StatusCode.Connect:
                _Peer.EstablishEncryption();
                break;
            case StatusCode.Disconnect:
                
            case StatusCode.DisconnectByServer:
               
            case StatusCode.DisconnectByServerLogic:
                
            case StatusCode.DisconnectByServerUserLimit:
                
            case StatusCode.ExceptionOnConnect:
                
            case StatusCode.TimeoutDisconnect:

                Controller.OnDisconnected("" + statusCode);
                _State = new Disconnected(this);
                break;

            case StatusCode.EncryptionEstablished:
                _State = new Connected(this);
                break;
            default:
                Controller.OnUnexpectedStatusCode(statusCode);
                _State = new Disconnected(this);
                break;
        }
    }

    public void OnEvent(EventData eventData)
    {
        Controller.OnEvent(eventData);
    }

    #endregion
}

