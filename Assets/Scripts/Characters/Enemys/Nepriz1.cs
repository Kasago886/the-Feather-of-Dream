using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nepriz1 : Enemy
{
    private List<string> debuffContain = new List<string> { "…À∫€", "”«”Ù", "±¿¿£", "∑≤”π", "ŒÆ√“",
        "¥‡»ı", "≥Ÿª∫", "ƒ˝÷ÿ", "÷–∂æ", "◊∆…À", "“Ï≥£¥‡»ı","◊∆…À¥‡»ı","…À∫€¥‡»ı" };
    private int ignoreNumber;
    NormalProperity N=new NormalProperity();
    private int number;
    private void Start()
    {
        base.Start();
        if(FindAnyObjectByType<PlayerCardController>() != null)
        {
            number=FindAnyObjectByType<PlayerCardController>().positionNumber;
        }
        if (GameObject.FindGameObjectWithTag(Consts.PlayerTag) != null)
        {
            Player player= GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
            N.abnormalityResistance=player.abnormalityResistance;
            N.burnResistance=player.burnResistance;
            N.traumaResistance=player.traumaResistance;
            N.tenacity=player.tenacity;
            N.strength=player.strength;
        }
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
        if (FindAnyObjectByType<EnemyUIScroll>() != null)
        {
            FindAnyObjectByType<EnemyUIScroll>().RemoveEnemyUI(GetComponent<Enemy>());
        }
        Destroy(gameObject);
    }
    public void ChangeState()
    {
        GameObject enemy = Instantiate(Resources.Load<GameObject>("AttackBodys/Nepriz/Dr.Nepriz1 2"), new Vector3(transform.position.x, transform.position.y + 5, 0), Quaternion.identity);
        enemy.transform.SetParent(GameObject.Find("Enemys").transform);
        Nepriz2 n = enemy.GetComponent<Nepriz2>();
        n.N.abnormalityResistance = N.abnormalityResistance;
        n.N.burnResistance = N.burnResistance;
        n.N.traumaResistance = N.traumaResistance;
        n.N.tenacity = N.tenacity;
        n.N.strength = N.strength;
        n.number=number;
    }
    private void OnDestroy()
    {
        if (GameObject.Find("Dr.NeprizTemporaryEnemyHealthUi") != null)
        {
            Destroy(GameObject.Find("Dr.NeprizTemporaryEnemyHealthUi"));
        }
    }
}
