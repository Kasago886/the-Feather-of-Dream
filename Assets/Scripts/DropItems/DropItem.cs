using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class DropItem : MonoBehaviour
{
    [Header("关联的物品UI")]
    public Item item;

    bool added = false;

    EquipmentPanelManager equipmentPanelManager;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(0, 5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && !added)
        {
            if(collision.tag == Consts.PlayerTag)
            {
                equipmentPanelManager.AddItem(item);
                added = true;

                Destroy(gameObject);
            }
        }
    }
}
