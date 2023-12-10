using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public int wave;
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

    void Start()
    {
        enemy_parent = GameObject.Find("ENEMIES");
    }

    void Update()
    {
        if (enemy_parent.transform.childCount == 0)
        {
            wave++;
            EnemySpawn.Instance.NextWave(2);
        }
    }
}
