using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BuffNameAndNumber
{
    public bool effectOnPlayer;
    public bool effectOnEnemy;
    public string name;
    public List<int> number;
}
public class AttackBodyBuffAdderController : MonoBehaviour
{
    public List<BuffNameAndNumber> buffNameAndNumber=new List<BuffNameAndNumber>();
    public string charactorName;
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
                    List<int> number = new List<int>();
                    for(int i = 0; i < buff.number.Count; i++)
                    {
                        number.Add(buff.number[i]);
                    }
                    theAttackBody.nextToPlayer.Add(new AttackBuffDict(buff.name,number));
                }
                if (buff.effectOnEnemy)
                {
                    List<int> number = new List<int>();
                    for (int i = 0; i < buff.number.Count; i++)
                    {
                        number.Add(buff.number[i]);
                    }
                    theAttackBody.nextToEnemy.Add(new AttackBuffDict(buff.name,buff.number));
                }
            }
            late = false;
        }
        if(GameObject.Find(charactorName + "AttackBody(Clone)")!=null&&GameObject.Find(charactorName+"AttackBody(Clone)").GetComponent<AttackBodyBuffAdder>()!=null)
        {
            theAttackBody = GameObject.Find(charactorName + "AttackBody(Clone)").GetComponent<AttackBodyBuffAdder>();
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
                    List<int> number = new List<int>();
                    for (int i = 0; i < buff.number.Count; i++)
                    {
                        number.Add(buff.number[i]);
                    }
                    theAttackBody.nextToPlayer.Add(new AttackBuffDict(buff.name, buff.number));
                }
                if (buff.effectOnEnemy)
                {
                    List<int> number = new List<int>();
                    for (int i = 0; i < buff.number.Count; i++)
                    {
                        number.Add(buff.number[i]);
                    }
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
