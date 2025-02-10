using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    public Transform health;
    public Feather feather;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (feather != null)
        {
            float ratio = feather.health / feather.maxHealth;
            health.transform.localScale = new Vector3(ratio, 1, 1);
        }
    }
}
