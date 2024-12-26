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
    public DefautFeather()
    {
        health = 100;
        maxHealth = 100;
    }
}
