using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nepriz1 : Enemy
{
    private List<string> debuffContain = new List<string> { "…À∫€", "”«”Ù", "±¿¿£", "∑≤”π", "ŒÆ√“", 
        "¥‡»ı", "≥Ÿª∫", "ƒ˝÷ÿ", "÷–∂æ", "◊∆…À", "“Ï≥£¥‡»ı","◊∆…À¥‡»ı","…À∫€¥‡»ı" };
    private int ignoreNumber;
    private void Start()
    {
        base.Start();
    }
    public override void AddBuff(string buffName)
    {
        if (debuffContain.Contains(buffName))
        {
            if (ignoreNumber < 99)
            {
                AddBuff("≤ª±ª»œø…");
                ignoreNumber++;
            }
        }
        else
        {
            Buff buff = BuffContainer.GetBuffInstance(buffName) as Buff;
            buff.Init(this);
            buff.name = buffName;
            AddBuff(buff);
        }
    }
    public override void OnDeath()
    {
        isDead = true;
        deathEvent?.Invoke();
    }
    public void ChangeState()
    {
        Instantiate(Resources.Load<GameObject>("AttackBodys/Nepriz/Dr.Nepriz1 2"),transform);
    }
}
