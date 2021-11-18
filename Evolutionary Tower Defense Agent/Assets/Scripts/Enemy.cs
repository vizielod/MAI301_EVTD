using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{

    [SerializeField] float HitPoints { get; set; }

    public Enemy(float hitPoints)
    {
        this.HitPoints = hitPoints;
    }

    public void GetDamaged(float damage)
    {
        HitPoints -= damage;
    }

    public void GetHealed(float damage)
    {
        HitPoints += damage;
    }

    public float GetHitPoints()
    {
        return HitPoints;
    }
}
