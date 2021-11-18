using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret
{
    [SerializeField] public float HitPoints { get; set; }
    [SerializeField] public float Range { get; set; }
    [SerializeField] public float FieldOfViewAngle { get; set; }
    [SerializeField] public float RotationSpeed { get; set; }
    [SerializeField] public float Damage { get; set; }

    public Turret(float hitPoints, float range, float fieldOfView, float rotationSpeed, float damage)
    {
        this.HitPoints = hitPoints;
        this.Range = range;
        this.FieldOfViewAngle = fieldOfView;
        this.RotationSpeed = rotationSpeed;
        this.Damage = damage;
    }
}
