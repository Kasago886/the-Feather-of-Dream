using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentItemChecker : MonoBehaviour
{
    public Item requireItem;
    [Header("勾选后不拥有Item时触发")]
    public bool opposite;
    public UnityEvent unityEvent;
    public bool readDialog;
    public string dialogFileName;

    EquipmentPanelManager equipmentPanelManager;
    Dialog dialog;
    // Start is called before the first frame update
    void Start()
    {
        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();
        dialog = FindAnyObjectByType<Dialog>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Func()
    {
        if (equipmentPanelManager != null)
        {
            bool has = equipmentPanelManager.HasItem(requireItem);
            if (opposite)
            {
                has = !has;
            }

            if (has)
            {
                unityEvent?.Invoke();

                if (readDialog)
                {
                    dialog.Read(dialogFileName);
                }
            }
        }
    }
}
