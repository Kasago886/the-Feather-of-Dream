using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPanelManager : MonoBehaviour
{
    public Text levelText;
    public Text expNumber;
    public RectTransform expProgress;
    public Text tenacity;
    public Text strength;
    public Text feather;
    public Text dream;
    public Transform featherEquipContent;
    public Transform brokenFeatherEquipContent;
    public Transform itemContent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePanel(Archive archive)
    {
        levelText.text = "Lv." + archive.level.ToString();
        expNumber.text = archive.currentExp.ToString() + "/" + archive.maxExp.ToString();
        expProgress.sizeDelta = new Vector2(300 * archive.currentExp/archive.maxExp, expProgress.sizeDelta.y);  
        tenacity.text = archive.tenacity.ToString();
        strength.text = archive.strength.ToString();
        feather.text = archive.feather.ToString();
        dream.text = archive.dream.ToString();

        foreach (GameObject equipedFeather in archive.equipedFeather)
        {
            GameObject instance = Instantiate(equipedFeather);
            instance.transform.SetParent(featherEquipContent,false);
        }
        foreach (GameObject equipedBrokenFeather in archive.equipedBrokenFeather)
        {
            GameObject instance = Instantiate(equipedBrokenFeather);
            instance.transform.SetParent(brokenFeatherEquipContent, false);
        }
        foreach (GameObject items in archive.items)
        {
            GameObject instance = Instantiate(items);
            instance.transform.SetParent(itemContent, false);
        }
    }
}
