using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotSpeed;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private Collider _collider;
    [SerializeField] private MeshRenderer _mesh;
    private GameObject _spawnContainer;
    public int damage = 1;
    private Rigidbody _rb;
    private Vector3 _rotation;

    void Start()
    {
        _rb= GetComponent<Rigidbody>();
        _rb.AddForce(Vector3.down * Random.Range(_movementSpeed, _movementSpeed * 2), ForceMode.Impulse);
        _rotation = new Vector3(Random.Range(-_rotSpeed, _rotSpeed), Random.Range(-_rotSpeed, _rotSpeed), Random.Range(-_rotSpeed, _rotSpeed));
        _spawnContainer = GameObject.Find("AsteroidContainer");
    }

    void Update()
    {
        HandleMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            _explosion.Play();
            if (transform.localScale.x > .25f)
            {
                transform.localScale = transform.localScale / 2;
                _rb.mass = _rb.mass / 1.5f;
                GameObject newSpawn = Instantiate(_asteroid);
                newSpawn.transform.parent = _spawnContainer.transform;
            }
            else
            {
                _mesh.enabled = false;
                _collider.enabled = false;
                Destroy(gameObject, 5f);
            } 
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.GetComponent<Player>().Damage(damage);
            Destroy(gameObject);
        }
    }
    void HandleMovement()
    {
        transform.Rotate(_rotation);

        //Bounds
        if (transform.position.x > 30 ||
            transform.position.x < -30 ||
            transform.position.y > 30 ||
            transform.position.y < -30) Destroy(gameObject);
    }
}
