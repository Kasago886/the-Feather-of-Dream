using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nepriz2 : Enemy
{
    public List<int> mediocreNumber;
    private int listNumber;
    private void Start()
    {
        base.Start();
        mediocreNumber = new List<int>();
        injuryEvent.AddListener(ChangeBuff);
    }
    private void Update()
    {
        base.Update();
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
        listNumber = buffList.Count;
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
}
