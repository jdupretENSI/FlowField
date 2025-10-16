using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPool : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public int PoolSize = 20000;
    private Queue<GameObject> _availableEnemies = new Queue<GameObject>();
    private List<GameObject> _allEnemies = new List<GameObject>();
    
    
    public void Initialize()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject tempObj = Instantiate(EnemyPrefab, transform);
            tempObj.SetActive(false);
            
            _allEnemies.Add(tempObj);
            _availableEnemies.Enqueue(tempObj);
        }
    }

    public GameObject GetEnemy()
    {

        if (PoolEmpty())
        {
            Initialize();
        }
        
        GameObject enemy = _availableEnemies.Dequeue();
        enemy.SetActive(true);
        //Return game object for setting position?
        return enemy;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        _availableEnemies.Enqueue(obj);
    }
    
    public bool PoolEmpty()
    {
        return _availableEnemies.Count == 0;
    }
}