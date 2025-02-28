using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHealth : MonoBehaviour
{
    public Text healthText;
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
            float ratio;
            if (feather.maxHealth == 0)
            {
                ratio = 1.0f;
            }
            else
            {
                ratio = feather.health / feather.maxHealth;
                if (ratio > 1.0f)
                {
                    ratio = 1.0f;
                }
                else if (ratio < 0.0f)
                {
                    ratio = 0;
                }
            }
            health.transform.localScale = new Vector3(ratio, 1, 1);
            if (feather.health == feather.maxHealth)
            {
                healthText.text = null;
            }
            else
            {
                healthText.text=feather.health.ToString();
            }
        }
    }
}
