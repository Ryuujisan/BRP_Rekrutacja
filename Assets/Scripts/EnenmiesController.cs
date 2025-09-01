using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnenmiesController : MonoBehaviour
{
    [SerializeField]
    private int _multiplierForKillWekness = 2;

    [SerializeField]
    private List<EnemyData> AllEnemies;

    [SerializeField]
    private List<SpawnPoint> SpawnPoints;

    [SerializeField]
    private GameObject EnemyPrefab;

    [SerializeField]
    private FloatingText _floatingText;

    private int _currentEnemies;

    private int _maxEnemies = 3;

    private void Awake()
    {
        ConfigureEnemiesController();
    }

    private void Start()
    {
        SpawnEnemies();
    }

    private void OnEnable()
    {
        AttachListeners();
    }

    private void OnDisable()
    {
        DettachListeners();
    }

    private void AttachListeners()
    {
        GameEvents.EnemyKilled += EnemyKilled;
    }

    private void DettachListeners()
    {
        GameEvents.EnemyKilled -= EnemyKilled;
    }

    private void EnemyKilled(IEnemy enemy)
    {
        FreeSpawnPoint(enemy.GetEnemyPosition());
        DestroyKilledEnemy(enemy.GetEnemyObject());

        var points = enemy.KilledByWeakness
            ? enemy.Data.Points
            : enemy.Data.Points * _multiplierForKillWekness;
        GameEvents.Points.Value += points;
        var spawnPosition = enemy.GetEnemyObject().transform.position + Vector3.up * 2.0f;
        var text = Instantiate(_floatingText, spawnPosition, Quaternion.identity);
        text.Spawn(spawnPosition, $"+{points}", Color.green, enemy.KilledByWeakness);

        StartCoroutine(SpawnEnemyViaCor());
    }

    private void SpawnEnemies()
    {
        while (_currentEnemies < _maxEnemies) SpawnEnemy();
    }

    private IEnumerator SpawnEnemyViaCor()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (_currentEnemies >= _maxEnemies)
        {
            Debug.LogError("Max Enemies reached! Kil some to spawn new");
            return;
        }

        var freeSpawnPointIndex = -1;
        for (var i = 0; i < SpawnPoints.Count; i++)
        {
            if (SpawnPoints[i].IsOccupied) continue;

            freeSpawnPointIndex = i;
            break;
        }

        if (freeSpawnPointIndex == -1) return;

        SpawnPoints[freeSpawnPointIndex].IsOccupied = true;
        var enemy = Instantiate(EnemyPrefab, SpawnPoints[freeSpawnPointIndex].Position.position, Quaternion.identity,
            transform).GetComponent<SoulEnemy>();
        var spriteIndex = Random.Range(0, AllEnemies.Count);
        enemy.SetupEnemy(AllEnemies[spriteIndex], SpawnPoints[freeSpawnPointIndex]);
        _currentEnemies++;
    }

    private void DestroyKilledEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }

    private void FreeSpawnPoint(SpawnPoint spawnPoint)
    {
        for (var i = 0; i < SpawnPoints.Count; i++)
        {
            if (spawnPoint != SpawnPoints[i]) continue;

            SpawnPoints[i].IsOccupied = false;
            _currentEnemies--;
            break;
        }
    }

    private void ConfigureEnemiesController()
    {
        _maxEnemies = SpawnPoints != null ? SpawnPoints.Count : 3;
    }
}

[Serializable]
public class SpawnPoint
{
    public Transform Position;
    public bool IsOccupied;
}