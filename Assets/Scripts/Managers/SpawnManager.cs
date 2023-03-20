using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _fastest = 3f;
    [SerializeField] private float _longest = 6f;
    [SerializeField] private GameObject[] _spawnPrefabs;
    [SerializeField] private GameObject _spawnContainer;

    private Player _player;
    private Shooting _shooting;

    private bool _waveStarted = false;
    private bool _stopSpawning = false;

    void Start()
    {
        _player = FindObjectOfType<Player>();
        if (_player == null) Debug.LogError("_player is NULL");
        
        _shooting = FindObjectOfType<Shooting>();
        if (_shooting == null) Debug.LogError("_shooting is NULL");

    }

    public void StartSpawning()
    {
        if (!_waveStarted)
        {
            StartCoroutine(SpawnRoutine());
            _waveStarted = true;
        } 
        else return;
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawning == false)
        {
            int newSpawnIndex = Random.Range(0, _spawnPrefabs.Length);
            Vector3 _spawnPos = new Vector3(Random.Range(-15, 15), 25, 0);

            if (gameObject.CompareTag("PowerUpSpawner"))
            {
                if (_player.shieldsActive && newSpawnIndex == 2) newSpawnIndex--;
                if (_shooting.hasTripleShot && newSpawnIndex == 1) newSpawnIndex--;
            }

            GameObject newSpawn = Instantiate(_spawnPrefabs[newSpawnIndex], _spawnPos, _spawnPrefabs[newSpawnIndex].transform.rotation);
            newSpawn.transform.parent = _spawnContainer.transform;

            yield return new WaitForSeconds(Random.Range(_fastest, _longest));
        }
    }

    public void OnGameOver()
    {
        _stopSpawning = true;
    }
}
