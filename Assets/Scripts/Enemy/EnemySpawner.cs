using System.Collections.Generic;
using Topdown.movement;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class wave
    {
        public GameObject[] EnemyPrefabs;
        public float SpawnTimer;
        public float SpawnInterval;
        public int EnemyPerWave;
        public int EnemySpawned ;
    }
    public List<wave> Waves;
    public int WaveCount ;
    private float x;
    private float y;
    public GameObject Boss;

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameActive)
        {
            Waves[WaveCount].SpawnTimer += Time.deltaTime;
            if (Waves[WaveCount].SpawnTimer > Waves[WaveCount].SpawnInterval)
            {
                SpawnEnemy();
                Waves[WaveCount].SpawnTimer = 0;
            }
            if (Waves[WaveCount].EnemySpawned >= Waves[WaveCount].EnemyPerWave)
            {
                WaveCount++;
            }
            if (WaveCount >= Waves.Count)
            {
                WaveCount = 0;
            }
            
        }
    }

    public void SpawnEnemy()
    {
        x = Random.Range(-44, 12);
        y = Random.Range(-4, 14);
        Vector2 SpawnPositon = new Vector2(x, y);
        int EnemyId = Random.Range(0, Waves[WaveCount].EnemyPrefabs.Length);
        Instantiate(Waves[WaveCount].EnemyPrefabs[EnemyId],SpawnPositon,transform.rotation);
        Waves[WaveCount].EnemySpawned++;
        if (WaveCount == 5)
        {
            Instantiate(Boss, SpawnPositon, transform.rotation);
            WaveCount = 0;
            Boss = null;
        }

    }  
}
