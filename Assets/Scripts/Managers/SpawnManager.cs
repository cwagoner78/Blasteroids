using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _fastest = 3f;
    [SerializeField] private float _longest = 6f;
    [SerializeField] private float _startingNukeTimer = 60;
    private float _nukeTimer;
    [SerializeField] private GameObject[] _spawnPrefabs;
    [SerializeField] private GameObject _spawnContainer;

    private Player _player;
    private Shooting _shooting;
    private GameManager _gameManager;

    private int _currentWave;
    private bool _waveStarted = false;
    private bool _stopSpawning = false;

    void Start()
    {
        _player = FindObjectOfType<Player>();
        if (_player == null) Debug.LogError("_player is NULL");
        
        _shooting = FindObjectOfType<Shooting>();
        if (_shooting == null) Debug.LogError("_shooting is NULL");

        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null) Debug.LogError("_gameManager is NULL");

        _nukeTimer = _startingNukeTimer;
    }

    private void Update()
    {
        if (!_gameManager.gamePaused && _waveStarted) UpdateNukeTimer();
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

    void UpdateNukeTimer()
    { 
        if (!_shooting.hasNuke) _nukeTimer -= Time.deltaTime;
        if (_shooting.hasNuke) _nukeTimer = _startingNukeTimer;
        if (_nukeTimer < 0) _nukeTimer = 0;
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
                if (_player.currentBoost == _player.maxBoost && newSpawnIndex == 0) newSpawnIndex++;
                if (_shooting.hasTripleShot && newSpawnIndex == 1) newSpawnIndex++;
                if (_player.shieldHealth == 3 && newSpawnIndex == 2) newSpawnIndex++;
                if (_player.lives == 3 && newSpawnIndex == 3) newSpawnIndex++;
                if (_shooting.ammoCount == 0) newSpawnIndex = 4;
                if (_nukeTimer < 1 && !_shooting.hasNuke) newSpawnIndex = 5;
                if (_nukeTimer > 1 && newSpawnIndex == 5) newSpawnIndex = 4;
            }

            GameObject newSpawn = Instantiate(_spawnPrefabs[newSpawnIndex], _spawnPos, _spawnPrefabs[newSpawnIndex].transform.rotation);
            newSpawn.transform.parent = _spawnContainer.transform;

            yield return new WaitForSeconds(Random.Range(_fastest, _longest));
        }
    }

    public void SetCurrentWave(int wave)
    {
        _currentWave = wave;
    }

    public void OnGameOver()
    {
        _stopSpawning = true;
    }
}
