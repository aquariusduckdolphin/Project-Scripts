using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{

    public float damage { get; }

    void TakeDamage(float damage);

}
