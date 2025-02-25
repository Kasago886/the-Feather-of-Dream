using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NCards1 : MonoBehaviour
{
    public int cardNumber;
    public string buffName,buffNameWhenReduceCard;
    public string[] buffNameWhenHaveBuff;
   public void GetCards()
    {
        PlayerCardController cardController = GameObject.Find("CardPanel").GetComponent<PlayerCardController>();
        Player player=GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
        if (cardNumber > cardController.remainingSlotCount)
        {
            for (int i = 0; i < cardNumber-cardController.remainingSlotCount; i++)
            {
                player.AddBuff(buffName);
            }
        }
        for (int i = 0; i < cardController.remainingSlotCount; i++)
        {
            player.GenerateCard();
        }
    }
    public void ClearCard()
    {
        PlayerCardController cardController = GameObject.Find("CardPanel").GetComponent<PlayerCardController>();
        Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
        for (int i = 0;i < cardController.content.transform.childCount;i++)
        {
            player.AddBuff(buffNameWhenReduceCard);
        }
        cardController.ClearCard();
    }
    public void GetBuffWhenHaveBuff()
    {
        List<string> buffNames = new List<string>();
        Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
        foreach (var buff in player.buffList)
        {
            if (!buffName.Contains(buff.name))
            {
                buffNames.Add(buff.name);   
            }
        }
        for(int i = 0;i<buffNames.Count ; i++)
        {
            for(int j = 0;j < buffNameWhenHaveBuff.Length ; j++)
            {
                player.AddBuff(buffNameWhenHaveBuff[j]);
            }
        }
    }
    public void ChangeFollower()
    {
        if (GameObject.Find("Follwer(Clone)") != null)
        {
            Follower follower= GameObject.Find("Follwer(Clone)").GetComponent<Follower>();
            follower.buffNameAndNumbers.Add("ŒÆ√“");
            follower.buffNameAndNumbers.Add("…À∫€");
            follower.buffNameAndNumbers.Add("◊∆…À");
            follower.damage += 5;
        }
        Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
        player.cardGenerateList.Remove("∑¢±Ì∑÷œÌ");
    }
}
