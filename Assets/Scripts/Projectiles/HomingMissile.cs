using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HomingMissile : MonoBehaviour
{
    [SerializeField] private float _aliveTimer = 3f;
    [SerializeField] private float _launchForce = 10f;
    [SerializeField] private float _speed = 0.05f;
    [SerializeField] bool _isLeftMissile;
    [SerializeField] bool _isRightMissile;

    private Rigidbody _rb;
    private Player _player;
    private GameObject[] _enemies;
    private Transform _target;
    private Vector3 _direction;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) Debug.LogError("_rb is NULL");

        _player = FindObjectOfType<Player>();
        if (_player == null) Debug.LogError("_player is NULL");

        if (_isLeftMissile) _rb.AddForce(Vector3.left * _launchForce, ForceMode.Impulse);
        if (_isRightMissile) _rb.AddForce(Vector3.right * _launchForce, ForceMode.Impulse);

        StartCoroutine(AliveTimer());
    }

    private void FixedUpdate()
    {
        //Find all enemies, get closest and set target
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        SetTarget(GetClosestEnemy(_enemies));

        //If current target is not targetable anymore, look for another target
        if (!_target.GetComponent<Enemy>().IsTargetable()) return;

        //Move towards target
        transform.LookAt(_target);
        _direction = _target.transform.position - transform.position;
        _rb.AddForce(_direction * _speed, ForceMode.Impulse);

    }

    Transform GetClosestEnemy(GameObject[] enemies)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget.transform;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    IEnumerator AliveTimer()
    {
        yield return new WaitForSeconds(_aliveTimer);
        if (transform.parent != null) Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }
}
