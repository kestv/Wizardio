using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();
        // int _sessionId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].transform.position = _position;
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _health = _packet.ReadFloat();

        GameManager.players[_id].SetHealth(_health);
    }

    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();

        GameManager.players[_id].Respawn();
    }

    public static void ProjectileSpawned(Packet _packet)
    {
        int _shooterId = _packet.ReadInt();
        string _name = _packet.ReadString();
        Vector3 _playerPos = _packet.ReadVector3();
        float _speed = _packet.ReadFloat();
        Vector3 _forward = _packet.ReadVector3();
        int _projectileId = _packet.ReadInt();
        float _playerRotation = _packet.ReadFloat();

        ProjectileManager.Instance.SpawnProjectile(_shooterId, _name, _playerPos, _speed, _forward, _projectileId, _playerRotation);
        Utils.StartCooldown(_name, 0.8f);
    }

    public static void ProjectileMove(Packet _packet)
    {
        Vector3 _pos = _packet.ReadVector3();
        int _id = _packet.ReadInt();
        ProjectileManager.Instance.MoveProjectile(_id, _pos);
    }

    public static void ProjectileDestroyed(Packet _packet)
    {
        int _id = _packet.ReadInt();
        ProjectileManager.Instance.DestroyProjectile(_id);
    }

    public static void PlayerKilled(Packet _packet)
    {
        int _shooterId = _packet.ReadInt();
        int _playerId = _packet.ReadInt();

        GameManager.players[_playerId].Die();
    }

    public static void ReloadScore(Packet _packet)
    {
        int _shooterId = _packet.ReadInt();
        float _score = _packet.ReadFloat();

        GameManager.players[_shooterId].ReloadScore(_score);
    }

    public static void LoginFailed(Packet _packet)
    {
        string _message = _packet.ReadString();
        UIManager.Instance.ConnectionHadErrors(_message);
    }
}
