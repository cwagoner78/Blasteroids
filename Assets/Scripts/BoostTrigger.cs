using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostTrigger : MonoBehaviour
{
    [SerializeField] private ParticleSystem _playerParticles;

    private float _inputY;

    private void Update()
    {
        _inputY = Input.GetAxisRaw("Vertical");
    }

    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (_inputY > 0.5f) _playerParticles.Play();
        else if (_inputY < 0.5f) _playerParticles.Stop();
    }
}

