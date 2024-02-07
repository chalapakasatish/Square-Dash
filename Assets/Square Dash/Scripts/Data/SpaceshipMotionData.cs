
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Spaceship Motion Data", menuName = "Scriptable Objects/Spaceship Motion Data", order = 1)]
public class SpaceshipMotionData : ScriptableObject
{
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float SpaceshipAcceleration { get; private set; }
}
