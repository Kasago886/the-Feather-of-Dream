using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nepriz2 : Enemy
{
    public List<int> mediocreNumber;
    private int listNumber;
    private float skillTimer;
    public NormalProperity N = new NormalProperity();
    public int number;
    private void Start()
    {
        base.Start();
        mediocreNumber = new List<int>();
        injuryEvent.AddListener(ChangeBuff);
    }
    private void Update()
    {
        base.Update();
        skillTimer += Time.deltaTime;
        if (listNumber != buffList.Count)
        {
            mediocreNumber.Clear();
            for (int i = 0; i < buffList.Count; i++)
            {
                if (buffList[i].name == "·²Ó¹")
                {
                    mediocreNumber.Add(i);
                }
            }
        }
        if(skillTimer>20)
        {
            skillTimer = 0;
            Change();
        }
        listNumber = buffList.Count;
        if (tenacity > 10000)
        {
            tenacity = 10000;
        }
    }
    private void ChangeBuff()
    {
        if (GameObject.FindGameObjectWithTag(Consts.PlayerTag) != null)
        {
            Player player= GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
            for (int i = 0;i < player.buffList.Count;i++)
            {
                if (player.buffList[i].name == "·²Ó¹")
                {
                    player.buffList[i].timer = 0;
                    break;
                }
            }
            AddBuff("·²Ó¹");
        }
    }
    private void Change()
    {
        int n = 0;
        foreach (var item in buffList)
        {
            if (item.name == "·²Ó¹")
            {
                item.timer = 0;
                n++;
            }
        }
        for (int i = 0; i < n; i++)
        {
            AddBuff("¿ÊÇóÈÏ¿É");
        }
    }
    private void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag(Consts.PlayerTag) != null)
        {
            Player player= GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent< Player>();
            player.tenacity = N.tenacity;
            player.strength = N.strength;
            player.abnormalityResistance = N.abnormalityResistance;
            player.burnResistance = N.burnResistance;
            player.traumaResistance = N.traumaResistance;
        }
        if(GameObject.FindAnyObjectByType<PlayerCardController>() != null)
        {
            FindAnyObjectByType<PlayerCardController>().positionNumber=number;
        }
    }
}
