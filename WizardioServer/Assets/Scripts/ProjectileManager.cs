using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager instance;

    List<int> projectiles = new List<int>();
    List<int> freeProjectiles = new List<int>();
    int nextProjectileId = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exsists, destroying...");
            Destroy(this);
        }
    }

    public int SpawnProjectile()
    {
        int _id;
        if (freeProjectiles.Count > 0)
        {
            _id = freeProjectiles.First();
            projectiles.Add(_id);
            freeProjectiles.Remove(_id);
        }
        else
        {
            projectiles.Add(nextProjectileId);
            _id = nextProjectileId;
            nextProjectileId++;
        }
        return _id;
    }

    public void DestroyProjectile(int _id)
    {
        freeProjectiles.Add(_id);
        projectiles.Remove(_id);
    }
}
