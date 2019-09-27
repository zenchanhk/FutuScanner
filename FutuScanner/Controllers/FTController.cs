﻿using FTWrapper;
using Futu.OpenApi.Pb;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutuScanner
{
    public class FTController : IController, INotifyPropertyChanged
    {
        public event EventHandler<ConnectedEventArgs> Connected;
        public event EventHandler<DisconnectedEventArgs> Disconnected;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public string Vendor { get; } = "FT";
        public string VendorFullName { get; } = "FuTu NiuNiu";

        private ConnectionParam _pConnParam;
        public ConnectionParam ConnParam
        {
            get { return _pConnParam; }
            set
            {
                if (_pConnParam != value)
                {
                    _pConnParam = value;
                    Client = new FTClient(value.Host, value.Port);
                    DisplayName = "FT(" + value.AccName + ")";
                    OnPropertyChanged("ConnParam");
                }
            }
        }

        private string _pName;
        public string DisplayName
        {
            get { return _pName; }
            set
            {
                if (_pName != value)
                {
                    _pName = value;
                    OnPropertyChanged("DisplayName");
                }
            }
        }

        private ConnectionStatus _pConnectionStatus = new ConnectionStatus();
        public ConnectionStatus ConnectionStatus
        {
            get { return _pConnectionStatus; }
            set
            {
                if (_pConnectionStatus != value)
                {
                    _pConnectionStatus = value;
                    OnPropertyChanged("ConnectionStatus");
                }
            }
        }


        public bool IsConnected { get { return Client.IsQotConnected || Client.IsTrdConnected; } }
        public bool IsLocked { get; private set; }

        private VM mainVM;
        public FTClient Client { get; private set; }
        public FTController(VM vm)
        {
            mainVM = vm;
        }

        private void Init()
        {
            Client = new FTClient(ConnParam.Host, ConnParam.Port);
            Client.QotConnCallback.InitConnected += QotConnCallback_InitConnected;
            Client.QotConnCallback.Disconnected += QotConnCallback_Disconnected;
            Client.TrdConnCallback.InitConnected += TrdConnCallback_InitConnected;
            Client.TrdConnCallback.Disconnected += TrdConnCallback_Disconnected;
            Client.TrdCallback.UnlockTrade += TrdCallback_UnlockTrade;
        }

        private void TrdCallback_UnlockTrade(object sender, FTWrapper.Events.UnlockTradeEventArgs e)
        {           
            if (e.Result.RetType != (int)Common.RetType.RetType_Succeed)
            {
                ConnectionStatus.IsLocked = true;
            }
            else
                ConnectionStatus.IsLocked = false;

            OnPropertyChanged("ConnectionStatus");
        }

        private void TrdConnCallback_Disconnected(object sender, FTWrapper.Events.DisconnectedEventArgs e)
        {
            ConnectionStatus.IsTrdConnected = false;
            ConnectionStatus.IsLocked = true;
            OnPropertyChanged("ConnectionStatus");
            Disconnected?.Invoke(this, new DisconnectedEventArgs(this, e.Client, e.ErrCode));
        }

        private void QotConnCallback_Disconnected(object sender, FTWrapper.Events.DisconnectedEventArgs e)
        {
            ConnectionStatus.IsQotConnected = false;
            OnPropertyChanged("ConnectionStatus");
            Disconnected?.Invoke(this, new DisconnectedEventArgs(this, e.Client, e.ErrCode));
        }

        private void TrdConnCallback_InitConnected(object sender, FTWrapper.Events.InitConnectedEventArgs e)
        {
            ConnectionStatus.IsTrdConnected = true;
            ConnectionStatus.TrdConnectID = e.Client.GetConnectID();
            OnPropertyChanged("ConnectionStatus");
            Connected?.Invoke(this, new ConnectedEventArgs(this, e.Client, e.ErrCode, e.Message));
        }

        private void QotConnCallback_InitConnected(object sender, FTWrapper.Events.InitConnectedEventArgs e)
        {
            ConnectionStatus.IsQotConnected = true;
            ConnectionStatus.QotConnectID = e.Client.GetConnectID();
            OnPropertyChanged("ConnectionStatus");
            Connected?.Invoke(this, new ConnectedEventArgs(this, e.Client, e.ErrCode, e.Message));
        }

        public void Connect()
        {
            Init();
            Client.Connect();
        }

        public void Disconnect()
        {
            Client.Disconnect();
        }
    }
}
