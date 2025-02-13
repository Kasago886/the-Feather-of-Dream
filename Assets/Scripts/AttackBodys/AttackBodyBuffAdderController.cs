using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BuffNameAndNumber
{
    public bool effectOnPlayer;
    public bool effectOnEnemy;
    public string name;
    public List<int> number = new List<int>();
}
public class AttackBodyBuffAdderController : MonoBehaviour
{
    public List<BuffNameAndNumber> buffNameAndNumber=new List<BuffNameAndNumber>();
    [HideInInspector]
    public AttackBodyBuffAdder theAttackBody;
    private bool late;
    private void Update()
    {
        if (late && theAttackBody != null)
        {
            foreach (var buff in buffNameAndNumber)
            {
                if (buff.effectOnPlayer)
                {
                    theAttackBody.nextToPlayer.Add(new AttackBuffDict(buff.name, buff.number));
                }
                if (buff.effectOnEnemy)
                {
                    theAttackBody.nextToEnemy.Add(new AttackBuffDict(buff.name,buff.number));
                }
            }
            late = false;
        }
    }
    public void AddBuff()
    {
        if (theAttackBody != null)
        {
            foreach (var buff in buffNameAndNumber)
            {
                if (buff.effectOnPlayer)
                {
                    theAttackBody.nextToPlayer.Add(new AttackBuffDict(buff.name, buff.number));
                }
                if (buff.effectOnEnemy)
                {
                    theAttackBody.nextToEnemy.Add(new AttackBuffDict(buff.name, buff.number));
                }
            }
        }
        else
        {
            late = true;
        }
    }
}
