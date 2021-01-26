using Assets.Scripts.Api.Controllers;
using Assets.Scripts.Api.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client
{
    public static int DataBufferSize = 4096;

    public int Id;
    public TCP Tcp;
    public UDP Udp;
    public Player Player;
    Vector3 PosToSpawn = new Vector3(0, 1f, -5f);
    public Client(int _clientId)
    {
        Id = _clientId;
        Tcp = new TCP(Id);
        Udp = new UDP(Id);
    }

    public class TCP
    {
        public TcpClient Socket;

        private readonly int Id;
        private NetworkStream Stream;
        private Packet ReceivedData;
        private byte[] ReceiveBuffer;

        public TCP(int _id)
        {
            Id = _id;
        }

        public void Connect(TcpClient _socket)
        {
            Socket = _socket;
            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;

            Stream = Socket.GetStream();

            ReceivedData = new Packet();
            ReceiveBuffer = new byte[DataBufferSize];
            Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);

            //TODO: Create game session and add client to it
            ServerSend.Welcome(Id, "Connected to the game session");
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (Socket != null)
                {
                    Stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to player {Id} via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = Stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Server.clients[Id].Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(ReceiveBuffer, _data, _byteLength);

                ReceivedData.Reset(HandleData(_data));
                Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error receiving TCP data: {_ex}");
                Server.clients[Id].Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            ReceivedData.SetBytes(_data);

            if (ReceivedData.UnreadLength() >= 4)
            {
                _packetLength = ReceivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= ReceivedData.UnreadLength())
            {
                byte[] _packetBytes = ReceivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        Server.packetHandlers[_packetId](Id, _packet);
                    }
                });

                _packetLength = 0;
                if (ReceivedData.UnreadLength() >= 4)
                {
                    _packetLength = ReceivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        public void Disconnect()
        {
            Socket.Close();
            Stream = null;
            ReceivedData = null;
            ReceiveBuffer = null;
            Socket = null;
        }
    }

    public class UDP
    {
        public IPEndPoint EndPoint;

        private int Id;

        public UDP(int _id)
        {
            Id = _id;
        }

        public void Connect(IPEndPoint _endPoint)
        {
            EndPoint = _endPoint;
        }

        public void SendData(Packet _packet)
        {
            Server.SendUDPData(EndPoint, _packet);
        }

        public void HandleData(Packet _packetData)
        {
            int _packetLength = _packetData.ReadInt();
            byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_packetBytes))
                {
                    int _packetId = _packet.ReadInt();
                    Server.packetHandlers[_packetId](Id, _packet);
                }
            });
        }

        public void Disconnect()
        {
            EndPoint = null;
        }
    }

    private void Disconnect()
    {
        Debug.Log($"{Tcp.Socket.Client.RemoteEndPoint} has disconnected");
        ThreadManager.ExecuteOnMainThread(() =>
        {
            UnityEngine.Object.Destroy(Player.gameObject);
            Player = null;

        });
        Tcp.Disconnect();
        Udp.Disconnect();

        ServerSend.PlayerDisconnected(Id);
    }

    public void SendIntoGame(string _playerName)
    {
        Player = NetworkManager.instance.InstantiatePlayer(PosToSpawn);
   
        Player.Initialize(Id, _playerName);

        foreach (var _client in Server.clients.Values)
        {
            if (_client.Player != null)
            {
                if (_client.Id != Id)
                {
                    ServerSend.SpawnPlayer(Id, _client.Player);
                }
            }
        }

        foreach (var _client in Server.clients.Values)
        {
            if (_client.Player != null)
            {
                ServerSend.SpawnPlayer(_client.Id, Player);
            }
        }
    }

    public void LoginFailed(string _message)
    {
        ServerSend.LoginFailed(Id, _message);
    }
}
