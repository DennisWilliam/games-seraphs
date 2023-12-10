using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName ="Scriptable Objects/Enemy")]
public class EnemySO : ScriptableObject
{
    public Sprite sprite; 
    public float height;
    public float distance;
    public float healthMax;
    public float cooldown;

    public GameObject bullet_prefab;
}
