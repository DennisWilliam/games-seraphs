using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn Instance { get; private set; }

    [SerializeField] EnemySO[] enemies;
    [SerializeField] GameObject enemy_prefab;
    GameObject enemy_parent;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy_parent = GameObject.Find("ENEMIES");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextWave(int wave)
    {
        int enemyCount = (int)Mathf.Pow(wave, 3f);
        int enemyDifficulty = wave;

        for (int i=0; i < enemyCount; i++)
        {
            Vector3 spwanPos = new Vector3(
                Random.Range(-10, 10), 
                Random.Range(6, 8)
                );
            int enemyIndex = Random.Range(0, enemies.Length);
            GameObject enemy = Instantiate(enemy_prefab, spwanPos, new Quaternion(), enemy_parent.transform);
            EnemyAI enemy_script = enemy.GetComponent<EnemyAI>();
            enemy_script.enemySO = enemies[enemyIndex];
        }
    }
}
