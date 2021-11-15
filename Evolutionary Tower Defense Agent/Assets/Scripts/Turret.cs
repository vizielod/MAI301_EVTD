using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret
{
    [SerializeField] public float HitPoints { get; set; } = 100f;
    [SerializeField] float Range { get; set; } = 5f;
    [SerializeField] float FieldOfViewAngle { get; set; } = 90f;
    [SerializeField] float RotationSpeed { get; set; } = 10f;
    [SerializeField] float Damage { get; set; } = 10f;

}
