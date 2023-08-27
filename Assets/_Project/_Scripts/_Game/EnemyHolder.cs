using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class EnemyHolder : MonoBehaviour
{
    public List<Enemy> EnemyList;
    [SerializeField] private PlayerTrigger _playerTrigger;
    [SerializeField] private PathCreator _pathCreator;

    private void Start()
    {
        CreateEnemyList();
        DefineScriptsOnEnemy();
        SubscribeOnEnemyDeathEvent();
    }

    private void CreateEnemyList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            EnemyList.Add(transform.GetChild(i).GetComponent<Enemy>());
        }
    }

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
            enemy.OnEnemyDeath += () => RemoveEnemyFromList(enemy);
        }
    }

    private void RemoveEnemyFromList(Enemy enemyToRemove)
    {
        if (EnemyList.Contains(enemyToRemove))
        {
            EnemyList.Remove(enemyToRemove);
        }
    }
}