using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _yPosBound = 15f;
    [SerializeField] private Collider _collider;
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private MeshRenderer _mesh;
    public int damage = 1;

    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime) ;
        if (transform.position.y < -_yPosBound + 10f) Destroy(gameObject); //transform.position = new Vector3(Random.Range(-_xPosBound, _xPosBound), _yPosBound, 0);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
            _explosion.Play();
            StartCoroutine(DestroyObject());
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.GetComponent<Player>().Damage(damage);

            _explosion.Play();
            StartCoroutine(DestroyObject());
        }
    }

    IEnumerator DestroyObject()
    {
        _mesh.enabled = false;
        _collider.enabled = false;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }


}

