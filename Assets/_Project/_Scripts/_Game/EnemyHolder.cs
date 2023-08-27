using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PathCreation;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHolder : MonoBehaviour
{
    public List<Enemy> EnemyList;
    public List<EnemyWave> EnemyWaveList = new List<EnemyWave>();
    [SerializeField] private float _timeForNextWaveStart;
    [SerializeField] private Slider _enemyWaveSlider;
    [SerializeField] private PlayerTrigger _playerTrigger;
    [SerializeField] private TextMeshProUGUI _currentWaveText;
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private StartFightController _startFightController;
    [SerializeField] private CaptureEnemyCastle _captureEnemyCastle;
    [SerializeField] private CameraController _cameraController;
    [ReadOnly] public int CurrentWaveNumber;
    [ReadOnly] public int AllWaveCount;
    private int _killedEnemyCountInCurrentWave;

    private void Start()
    {
        //CreateEnemyList();
        DefineScriptsOnEnemy();
        SubscribeOnEnemyDeathEvent();
        SetStartingWaveCount();
        PrintWaveCountText();
        SetStartingWaveSlider();
    }

    // private void CreateEnemyList()
    // {
    //     for (int i = 0; i < transform.childCount; i++)
    //     {
    //         EnemyList.Add(transform.GetChild(i).GetComponent<Enemy>());
    //     }
    // }

    private void DefineScriptsOnEnemy()
    {
        foreach (var enemy in EnemyList)
        {
            enemy.PathCreatorScript = _pathCreator;
            enemy.PlayerTriggerScript = _playerTrigger;
        }
    }

    private void SubscribeOnEnemyDeathEvent()
    {
        foreach (var enemy in EnemyList)
        {
            enemy.ShootableHealth.OnDeath += IncreaseWaveSlider;
            enemy.ShootableHealth.OnDeath += () => RemoveEnemyFromList(enemy);
            enemy.ShootableHealth.OnDeath += () => RemoveEnemyFromWaveList(enemy);
        }
    }

    private void RemoveEnemyFromList(Enemy enemyToRemove)
    {
        if (EnemyList.Contains(enemyToRemove))
        {
            EnemyList.Remove(enemyToRemove);
        }

        if (EnemyList.Count == 0)
        {
            _startFightController.gameObject.SetActive(false);
            _captureEnemyCastle.gameObject.SetActive(true);
            StartCoroutine(_cameraController.SetCaptureCastleVirtualCamera());
        }
    }

    private void RemoveEnemyFromWaveList(Enemy enemyToRemove)
    {
        if (EnemyWaveList[CurrentWaveNumber].EnemiesInWave.Contains(enemyToRemove))
        {
            EnemyWaveList[CurrentWaveNumber].EnemiesInWave.Remove(enemyToRemove);
        }

        if (EnemyWaveList[CurrentWaveNumber].EnemiesInWave.Count == 0)
        {
            StartCoroutine(IncreaseCurrentWave());
            PrintWaveCountText();
        }
    }

    private void IncreaseWaveSlider()
    {
        _killedEnemyCountInCurrentWave++;
        _enemyWaveSlider.DOValue(_killedEnemyCountInCurrentWave, 0.25f);
    }

    private void SetStartingWaveCount()
    {
        CurrentWaveNumber = 0;
        AllWaveCount = EnemyWaveList.Count;
    }

    private void SetStartingWaveSlider()
    {
        _killedEnemyCountInCurrentWave = 0;
        _enemyWaveSlider.maxValue = EnemyWaveList[CurrentWaveNumber].EnemiesInWave.Count;
        _enemyWaveSlider.DOValue(_killedEnemyCountInCurrentWave, 0.25f);
    }

    private void PrintWaveCountText()
    {
        _currentWaveText.text = (1 + CurrentWaveNumber) + "/" + AllWaveCount; // The reason we're adding 1 here is because lists start from 0.
    }

    private IEnumerator IncreaseCurrentWave()
    {
        if (CurrentWaveNumber >= AllWaveCount)
        {
            yield break;
        }
        CurrentWaveNumber++;
        SetStartingWaveSlider();
        yield return new WaitForSeconds(_timeForNextWaveStart);
        _startFightController.StartFight();
    }

    public void StopAllEnemies()
    {
        foreach (var enemy in EnemyList)
        {
            enemy.Stop();
        }
    }
}