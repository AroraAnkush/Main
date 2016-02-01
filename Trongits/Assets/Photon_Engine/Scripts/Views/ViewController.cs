using System;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;

    public class ViewController : IViewController
    {

        private readonly View _controlledview;
        private readonly byte _SubOperationCode;
        public View Controlledview { get { return _controlledview; } }

        private readonly Dictionary<byte, IPhotonOperationHandler> _Operationhandlers = new Dictionary<byte, IPhotonOperationHandler>();
        private readonly Dictionary<byte, IPhotonEventHandler> _EventHandler = new Dictionary<byte, IPhotonEventHandler>();
   

    public ViewController(View controlledview, byte suboperationcode = 0)
        {
            _controlledview = controlledview;
        _SubOperationCode = suboperationcode;
            if(PhotonEngine.Instance == null)
            {
                Application.LoadLevel(0);
            }
            else
            {
                PhotonEngine.Instance.Controller = this;
            }
        }


        public Dictionary<byte, IPhotonOperationHandler> Operationhandler { get { return _Operationhandlers; } }
        public Dictionary<byte, IPhotonEventHandler> EventHandler { get { return _EventHandler; } }
        #region Implementation of IviewController
        
        
        public bool isConnected
        {
            get
            {
                return PhotonEngine.Instance._State is Connected;
            }
        }

        public void ApplicationQuit()
        {
            PhotonEngine.Instance.Disconnect();
        }

        public void Connect()
        {
           if(!isConnected)
            {
                PhotonEngine.Instance.Initialize();
            }
        }

        public void DebugReturn(DebugLevel level, string message)
        {
            _controlledview.LogDebug(string.Format("{0}  {1}", level, message));
        }

        public void OnDisconnected(string message)
        {

        }

        public void OnEvent(EventData eventdata)
        {
            IPhotonEventHandler handler;
            if (EventHandler.TryGetValue(eventdata.Code, out handler))
            {
                handler.HandleEvent(eventdata);
            }
            else
            {
                OnUnexpectedEvent(eventdata);
            }
        }

        public void OnOperationResponse(OperationResponse response)
        {
            IPhotonOperationHandler operationhandler;
            if(response.Parameters.ContainsKey(_SubOperationCode) && _Operationhandlers.TryGetValue(Convert.ToByte(response[_SubOperationCode]), out operationhandler))
            {
                operationhandler.HandleResponse(response);
            }
            else
            {
                OnUnexpectedOperationResponse(response);
            }
        }

        public void OnUnexpectedEvent(EventData eventdata)
        {
            _controlledview.LogError(String.Format("Unexpected event {0}", eventdata.Code));
        }

        public void OnUnexpectedOperationResponse(OperationResponse operationresponse)
        {
            _controlledview.LogError(String.Format("Unexpected Operation error {0} , {1}", operationresponse.ReturnCode, operationresponse.OperationCode));

        }

        public void OnUnexpectedStatusCode(StatusCode statuscode)
        {
            _controlledview.LogError(String.Format("Unexpected status {0}", statuscode));

        }

        public void SendOperation(OperationRequest request, bool sendReliable, byte ChannelId, bool encrypt)
        {
            PhotonEngine.Instance.SendOp(request, sendReliable, ChannelId, encrypt);
        }

        #endregion
    }

