using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _timer = 5f;

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
            _trackToPlayer = true;
            _shooting.TripleShotActive(_timer);
            transform.GetComponent<SpriteRenderer>().enabled= false;
            GameObject.Find("TS_Collect").GetComponent<ParticleSystem>().Play();

            GameObject.Find("TS_Trail").GetComponent<ParticleSystem>().Stop();
            Destroy(gameObject, _timer);
        }
    }

}
