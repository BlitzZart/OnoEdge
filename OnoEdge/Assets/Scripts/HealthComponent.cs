using UnityEngine;
using System.Collections;
using System;

public class HealthComponent : MonoBehaviour, IDamageable {
    public float maxHp = 10;
    private float hp = 0;

    public float Hp {
        get {
            return hp;
        }
    }

    public void Hit(int dmg) {
        hp -= dmg;
    }

    // Use this for initialization
    void Start () {
        hp = maxHp;
	}
}
