using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _fastest = 3f;
    [SerializeField] private float _longest = 6f;
    [SerializeField] private GameObject[] _spawnPrefabs;
    [SerializeField] private GameObject _spawnContainer;
    
    private bool _stopSpawning;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            int newSpawnIndex = Random.Range(0, _spawnPrefabs.Length);
            Vector3 _spawnPos = new Vector3(Random.Range(-15, 15), 30, 0);
            GameObject newSpawn = Instantiate(_spawnPrefabs[newSpawnIndex], _spawnPos, _spawnPrefabs[newSpawnIndex].transform.rotation);
            newSpawn.transform.parent = _spawnContainer.transform;
            Debug.Log("Spawned Prefab number " + newSpawnIndex);
            yield return new WaitForSeconds(Random.Range(_fastest, _longest));
        }
    }

    public void OnGameOver()
    { 
        _stopSpawning= true;
    }
}
