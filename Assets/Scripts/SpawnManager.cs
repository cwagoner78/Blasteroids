using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _fastest = 3f;
    [SerializeField] private float _longest = 6f;
    [SerializeField] private GameObject[] _spawnPrefabs;
    [SerializeField] private GameObject _spawnContainer;
    private Vector3 _spawnPos;
    private bool _stopSpawning;

    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            int spawnIndex = Random.Range(0, _spawnPrefabs.Length);
            _spawnPos = new Vector3(Random.Range(-15, 15), 30, 0);
            GameObject newSpawn = Instantiate(_spawnPrefabs[spawnIndex], _spawnPos, _spawnPrefabs[spawnIndex].transform.rotation);
            newSpawn.transform.parent = _spawnContainer.transform;
            Debug.Log("Spawned Prefab number " + spawnIndex);
            yield return new WaitForSeconds(Random.Range(_fastest, _longest));
        }

    }

    public void OnGameOver()
    { 
        _stopSpawning= true;
    }
}
