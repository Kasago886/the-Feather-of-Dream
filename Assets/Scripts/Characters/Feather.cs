using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather
{
    public float health;
    public float maxHealth;
    public float lockTimer = 0;
}

public class DefautFeather : Feather
{
    public DefautFeather(float hp = 100)
    {
        maxHealth = hp;
        health = maxHealth;
    }
}
