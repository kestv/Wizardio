using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    int Id { get; set; }
    public float Speed { get; } = 15f;
    float Damage;
    bool Initialized = false;
    public int ShooterId { get; set; }
    Player Shooter { get; set; }
    Rigidbody Rgb;
    Vector3 Forward;
    float SpawnTime;

    // Update is called once per frame
    void Update()
    {
        if (!Initialized)
        {
            return;
        }

        if (Time.time - SpawnTime > 5f)
        {
            Destroy();
        }
        // TODO: need to deal with lag 0.02 on server/0.03 on client is a nice ratio for my pc
        Rgb.transform.Translate(Forward * Speed * Time.deltaTime);
        ServerSend.ProjectileMove(transform.position, Id);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Player>().Id != ShooterId)
        {
            Collided(other.gameObject);
        }
        else if (!other.gameObject.CompareTag("Player"))
        {
            Destroy();
        }
    }

    void Collided(GameObject _player)
    {
        bool dead = _player.GetComponent<Player>().TakeDamage(Shooter, Damage, ShooterId);
        Shooter.GetComponent<Player>().DealDamage(Damage);
        Destroy();
    }

    public void Initialize(Player _shooter, Vector3 _forward, float _damage, int _shooterId, int _id)
    {
        Shooter = _shooter;
        SpawnTime = Time.time;
        Rgb = transform.GetComponent<Rigidbody>();
        Forward = _forward;
        Damage = _damage;
        ShooterId = _shooterId;
        Id = _id;
        Initialized = true;
    }

    void Destroy()
    {
        ProjectileManager.instance.DestroyProjectile(Id);
        ServerSend.ProjectileDestroyed(Id);
        Destroy(gameObject);
        this.enabled = false;
    }
}
