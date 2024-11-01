using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab2GenEnemy : MonoBehaviour
{
    [SerializeField]
    List<GameObject> enemyPrefabs; // Danh sách prefab kẻ thù
    [SerializeField]
    float period;
    [SerializeField]
    int poolSize;
    [SerializeField]
    bool enableExtend;
    [SerializeField]
    int maxEnemiesOnScreen; // Giới hạn số lượng kẻ thù trên màn hình

    // Tỷ lệ phần trăm cho các loại enemy
    [SerializeField]
    float spawnRateTypeA = 0.2f; 
    [SerializeField]
    float spawnRateTypeB = 0.8f; 

    float time;
    List<GameObject> enemyPool;
    int currentEnemiesOnScreen;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        currentEnemiesOnScreen = 0;

        // Tạo pool
        enemyPool = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(GetRandomEnemyPrefab()); // Random kẻ thù từ danh sách
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= period && currentEnemiesOnScreen < maxEnemiesOnScreen)
        {
            GameObject newEnemy = GetFreeEnemy();
            if (newEnemy != null)
            {
                newEnemy.SetActive(true); // Kích hoạt kẻ thù mà không xác định vị trí

                currentEnemiesOnScreen++;
                time = 0;
            }
        }
    }

    // Chọn ngẫu nhiên prefab từ danh sách dựa trên tỷ lệ
    private GameObject GetRandomEnemyPrefab()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < spawnRateTypeA) 
        {
            return enemyPrefabs[0]; 
        }
        else 
        {
            return enemyPrefabs[1]; 
        }
    }

    // Lấy một kẻ thù chưa được sử dụng từ pool
    private GameObject GetFreeEnemy()
    {
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (enemyPool[i] == null)
                continue; // Skip destroyed objects

            if (!enemyPool[i].activeSelf)
                return enemyPool[i];
        }

        // Nếu pool đầy và có cho phép mở rộng
        if (enableExtend)
        {
            GameObject enemy = Instantiate(GetRandomEnemyPrefab());
            enemy.SetActive(false);
            enemyPool.Add(enemy);
            return enemy;
        }
        return null;
    }
}
