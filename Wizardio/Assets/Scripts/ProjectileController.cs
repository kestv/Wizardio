using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Vector3 shotDirection { get; set; }
    public float speed { get; set; }
    int shooterId;
    float damage;
    Rigidbody rgb;
    Vector3 forward;

    public void Initialize(int _shooterId, float _speed, Vector3 _forward)
    {
        shooterId = _shooterId;
        rgb = transform.GetComponent<Rigidbody>();
        speed = _speed;
        forward = _forward;
    }

    internal void DestroyProjectile()
    {
        StartCoroutine("Explode");
    }

    public void Move(Vector3 _pos)
    {
        transform.position = _pos;
    }

    IEnumerator Explode()
    {
        var explosion = Instantiate(Resources.Load("explosion") as GameObject, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        Destroy(explosion);
    }
}
