using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Enemy> wildEnemies;

    public Enemy GetRandomWildEnemy()
    {
        var wildEnemy = wildEnemies[Random.Range(0, wildEnemies.Count)];
        wildEnemy.Init();
        return wildEnemy;
    }
}
