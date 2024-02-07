using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Square Motion Data",menuName = "Scriptable Objects/Square Motion Data",order =0)]
public class SquareMotionData : ScriptableObject
{
    [field: SerializeField] public float MoveSpeed {  get; private set; }
    [field: SerializeField] public float JumpSpeed { get; private set; }
}
