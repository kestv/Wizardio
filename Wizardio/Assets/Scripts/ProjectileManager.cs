using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    public static Dictionary<int, GameObject> Projectiles = new Dictionary<int, GameObject>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exsists, destroying...");
            Destroy(this);
        }
    }

    public void SpawnProjectile(int _shooterId, string _name, Vector3 _pos, float _speed, Vector3 _forward, int _projectileId, float _playerRotation)
    {
        //Instantiates projectile and adds to projectile array by next Id
        // if so, ++ next id
        // else by the free id from free ids array
        // if so, empties free id slot

        // Not sure if this is required
        while (Projectiles.ContainsKey(_projectileId))
        {
            ClientSend.PlayerShoot(_pos, _forward, _name);
        }
        GameObject _shot = Instantiate(Resources.Load(_name) as GameObject, _pos, new Quaternion(Quaternion.identity.x, _playerRotation, Quaternion.identity.z, Quaternion.identity.w));
        _shot.GetComponent<ProjectileController>().Initialize(_shooterId, _speed, _forward);
        Projectiles.Add(_projectileId, _shot);
        
    }

    public void MoveProjectile(int _id, Vector3 _position)
    {
        Projectiles.FirstOrDefault(x => x.Key == _id).Value?.GetComponent<ProjectileController>().Move(_position);
    }
    public void DestroyProjectile(int _id)
    {
        Projectiles.FirstOrDefault(x => x.Key == _id).Value?.GetComponent<ProjectileController>().DestroyProjectile();
        Destroy(Projectiles.First(x => x.Key == _id).Value);
        Projectiles.Remove(_id);
    }
}
