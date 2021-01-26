using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Id;
    public string Username;

    //Control
    public CharacterController Controller;
    public float Gravity = -9.18f;
    public float DefaultMoveSpeed = 5f;
    public float MoveSpeed = 5f;
    public float JumpSpeed = 5f;
    public float InAirMovementRatio = 0.97f;
    public bool[] Inputs;
    private float yVelocity = 0f;
    private Vector3 LastPosition = Vector3.zero;
    private Quaternion LastRotation = Quaternion.identity;
    private Vector3 LastDirection = Vector3.zero;

    //Shooting
    public Transform ShootOrigin;
    public float Damage = 50f;
    public GameObject ShotObject;
    public bool IsReadyToShoot;

    //Health
    public float Health;
    public float MaxHealth = 100f;

    //Combat
    float Score = 0;
    float Bounty = 0;
    float DamageDealt = 0;
    float LastShot = 0;

    private void Start()
    {
        Gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        MoveSpeed *= Time.fixedDeltaTime;
        DefaultMoveSpeed *= Time.fixedDeltaTime;
        JumpSpeed *= Time.fixedDeltaTime;
    }
    public void Initialize(int _id, string _username)
    {
        Id = _id;
        Username = _username;
        Health = MaxHealth;
        Inputs = new bool[5];
    }

    public void FixedUpdate()
    {
        if (Health <= 0f)
        {
            return;
        }

        if (!IsReadyToShoot)
        {
            if (Time.time > LastShot + 1f)
            {
                IsReadyToShoot = true;
            }
        }

        Vector2 _inputDirection = Vector2.zero;
        if (Inputs[0])
        {
            _inputDirection.y += 1;
        }
        if (Inputs[1])
        {
            _inputDirection.y -= 1;
        }
        if (Inputs[2])
        {
            _inputDirection.x -= 1;
        }
        if (Inputs[3])
        {
            _inputDirection.x += 1;
        }

        Move(_inputDirection);
    }

    private void Move(Vector2 _inputDirection)
    {
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= MoveSpeed;

        if (Controller.isGrounded)
        {
            MoveSpeed = DefaultMoveSpeed;
            yVelocity = 0f;
            if (Inputs[4])
            {
                yVelocity = JumpSpeed;
            }
        }
        else
        {
            LastDirection *= InAirMovementRatio;
            _moveDirection = LastDirection;
        }

        LastDirection = _moveDirection;
        yVelocity += Gravity;
        _moveDirection.y = yVelocity;
        Controller.Move(_moveDirection);
        if (LastPosition != transform.position)
        {
            ServerSend.PlayerPosition(this);
            LastPosition = transform.position;
        }
        if (LastRotation != transform.rotation)
        {
            ServerSend.PlayerRotation(this);
            LastRotation = transform.rotation;
        }
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        Inputs = _inputs;
        transform.rotation = _rotation;
    }

    public void Shoot(Vector3 _pos, Vector3 _forward, Player _player, string _spellName)
    {
        if (Time.time - LastShot > 1f)
        {
            int _projectileId = ProjectileManager.instance.SpawnProjectile();
            var shot = Instantiate(ShotObject, _pos + _forward * 0.7f, Quaternion.identity);
            shot.GetComponent<ProjectileController>().Initialize(_player, _forward, Damage, Id, _projectileId);
            ServerSend.ProjectileSpawned(shot, transform, _projectileId, _spellName);
            LastShot = Time.time;
        }
    }

    public bool TakeDamage(Player _shooter, float _damage, int _shooterId)
    {
        Health -= _damage;

        if (Health <= 0)
        {
            Health = 0;
            Controller.enabled = false;
            transform.position = new Vector3(0, 25f, 0);
            ServerSend.PlayerPosition(this);
            PlayerKilled(_shooter);
            StartCoroutine(Respawn());
            return true;
        }

        ServerSend.PlayerHealth(this);
        return false;
    }

    public void DealDamage(float _amount)
    {
        DamageDealt += _amount;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        Health = MaxHealth;
        Controller.enabled = true;
        ServerSend.PlayerRespawn(this);
    }

    private void PlayerKilled(Player _shooter)
    {
        _shooter.RewardForKill(Bounty, _shooter.Id);
        ServerSend.PlayerKilled(_shooter.Id, Id);
    }

    public void RewardForKill(float _bounty, int _killerId)
    {
        Score += 10 + _bounty;
        Bounty++;
        ServerSend.ReloadScore(_killerId, Score);
    }
}
