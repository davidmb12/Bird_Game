using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Levels")]
public class LevelScriptableObject : ScriptableObject
{
    [SerializeField]
    public Transform levelObject;
    [SerializeField]
    public float levelSpeed;
}
