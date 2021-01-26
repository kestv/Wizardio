using UnityEngine;
using Assets.Scripts.Api.Controllers;
using System;
using Assets.Scripts.Api.Models;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Api.Dto;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        UsersController usersCtrl = new UsersController();
        try
        {
            Response<User> response = usersCtrl.Login(_username);
            User user = response.entity;
            GameSession session = null;
            if (!response.hasErrors)
            {
                GameSessionController gameSessionsController = new GameSessionController();
                Response<List<GameSession>> sessionResponse = gameSessionsController.GetAvailableGameSessions();
                if (!sessionResponse.hasErrors || sessionResponse.entity == null)
                {
                    var sessions = sessionResponse.entity;
                    var foundSession = sessions.FirstOrDefault(x => x.MaxPlayers < x.PlayersCount);
                    if (foundSession != null)
                    {
                        session = foundSession;
                        gameSessionsController.AddPlayerToSession(_fromClient, foundSession);
                    }
                    else
                    {
                        var newSession = new GameSession()
                        {
                            MaxPlayers = 2,
                            PlayersCount = 1,
                            Start = new DateTime()
                        };
                        GameSessionController gameSessionController = new GameSessionController();
                        session = gameSessionController.AddNewSession(newSession).entity;
                    }
                }
                if (session != null)
                {
                    user.SessionId = session.Id;
                }
                usersCtrl.UpdateUser(user);
                // Connect player to a session.
                Debug.Log(String.Format("{0} has logged in successfully", _username));
                Server.clients[_fromClient].SendIntoGame(_username);
                Debug.Log($"{Server.clients[_fromClient].Tcp.Socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            }
            else
            {
                Server.clients[_fromClient].LoginFailed(response.message);
            }
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("Error occured, when player {0} tried to log in: {1}", _username, e.Message);
            Server.clients[_fromClient].LoginFailed("Something went wrong");

        }

        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();

        Server.clients[_fromClient].Player.SetInput(_inputs, _rotation);
    }

    public static void PlayerShoot(int _fromClient, Packet _packet)
    {
        Vector3 _pos = _packet.ReadVector3();
        Vector3 _forward = _packet.ReadVector3();
        string _spellName = _packet.ReadString();

        Server.clients[_fromClient].Player.Shoot(_pos, _forward, Server.clients[_fromClient].Player, _spellName);
    }
}
