using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _powerUpCoolDown = 5f;
    [SerializeField] private float _playerSpeedMultiplier = 1.2f;
    [SerializeField] private int _powerUpID;

    [SerializeField] private ParticleSystem _collectParticles;
    [SerializeField] private ParticleSystem _trailParticles;


    private bool _trackToPlayer = false;
    private Player _player;
    private Shooting _shooting;


    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _shooting = GameObject.Find("Player").GetComponent<Shooting>(); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
        if (transform.position.y < -20f) Destroy(gameObject);
        
        if (_trackToPlayer)
        {
            Vector3 playerPos = _player.transform.position;
            transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_powerUpID == 0) _player.SpeedPowerUp(_playerSpeedMultiplier, _powerUpCoolDown);
            
            if (_powerUpID == 1) _shooting.TripleShotActive(_powerUpCoolDown);


            _trackToPlayer = true;
            transform.GetComponent<SpriteRenderer>().enabled = false;
            _collectParticles.Play();
            _trailParticles.Stop();
            Destroy(gameObject, _powerUpCoolDown);
        }
    }

}
