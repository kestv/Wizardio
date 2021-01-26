using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].Tcp.SendData(_packet);
    }

    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].Udp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].Tcp.SendData(_packet);
        }
    }
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].Tcp.SendData(_packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].Udp.SendData(_packet);
        }
    }
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].Udp.SendData(_packet);
            }
        }
    }

    #region Packets
    public static void Welcome(int _toClient, string _msg, int _sessionId = 0)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);
            //_packet.Write(_sessionId);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.Id);
            _packet.Write(_player.Username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.Id);
            _packet.Write(_player.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerRotation(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.Id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAll(_player.Id, _packet);
        }
    }

    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerHealth(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerHealth))
        {
            _packet.Write(_player.Id);
            _packet.Write(_player.Health);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerRespawn(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRespawned))
        {
            _packet.Write(_player.Id);

            SendTCPDataToAll(_packet);
        }
    }

    public static void ProjectileSpawned(GameObject _projectile, Transform pTransform, int _projectileId, string _projectileName)
    {
        var obj = _projectile.GetComponent<ProjectileController>();
        using (Packet _packet = new Packet((int)ServerPackets.projectileSpawned))
        {
            _packet.Write(obj.ShooterId);
            _packet.Write(_projectileName);
            _packet.Write(pTransform.position);
            _packet.Write(obj.Speed);
            _packet.Write(pTransform.forward);
            _packet.Write(_projectileId);
            _packet.Write(pTransform.rotation.y);

            SendTCPDataToAll(_packet);
        }
    }

    public static void ProjectileMove(Vector3 _pos, int _projectileId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.projectileMove))
        {
            _packet.Write(_pos);
            _packet.Write(_projectileId);

            SendUDPDataToAll(_packet);
        }
    }

    public static void ProjectileDestroyed(int _projectileId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.projectileDestroyed))
        {
            _packet.Write(_projectileId);

            SendTCPDataToAll(_packet);
        }
       
    }

    public static void PlayerKilled(int _shooterId, int _id)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerKilled))
        {
            _packet.Write(_shooterId);
            _packet.Write(_id);

            SendTCPDataToAll(_packet);
        }
    }

    public static void ReloadScore(int _shooterId, float _score)
    {
        using (Packet _packet = new Packet((int)ServerPackets.reloadScore))
        {
            _packet.Write(_shooterId);
            _packet.Write(_score);

            SendTCPData(_shooterId, _packet);
        }
    }

    public static void LoginFailed(int _toClient, string _message)
    {
        using (Packet _packet = new Packet((int)ServerPackets.loginFailed))
        {
            _packet.Write(_message);
            SendTCPData(_toClient, _packet);
        }
    }
    #endregion
}
