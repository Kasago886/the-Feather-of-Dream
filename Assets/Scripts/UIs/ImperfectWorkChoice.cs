using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class NormalProperity
{
    public bool effectOnPlayer;
    public bool effectOnEnemy;
    public float tenacity;
    public float strength;
    public float abnormalityResistance;
    public float burnResistance;
    public float traumaResistance;
    public float unlockHealth;
    public float lockHealth;
    public float tenacity1;
    public float strength1;
    public float abnormalityResistance1;
    public float burnResistance1;
    public float traumaResistance1;
    public float unlockHealth1;
    public float lockHealth1;
    public List<ChangeTimer> attackTimerList;
    public List<ChangeTimer> skillTimerList;
}
[Serializable]
public class ChangeTimer
{
    public int skillNumber;
    public float time;
}

public class ImperfectWorkChoice : MonoBehaviour
{
    public Material gb1;
    public string descrabption;
    public List<BuffNameAndNumber> buffNameAndNumbers;
    public List<NormalProperity> normalProperity;
    [HideInInspector] public float timeNumber;
    private Player player;
    private Nepriz2 nepriz2;
    private Image image;
    private Animator animator;
    private bool b,b1;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        animator = GetComponentInParent<Animator>();
        if (GameObject.FindGameObjectWithTag(Consts.PlayerTag) != null)
        {
            player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
        }
        if (GameObject.Find("Dr.Nepriz1 2(Clone)") != null)
        {
            nepriz2 = GameObject.Find("Dr.Nepriz1 2(Clone)").GetComponent<Nepriz2>();
        }
        animator.SetBool("appear",true);
        animator.speed = 1 / Time.timeScale;
    }
    void Update()
    {
        if (b && !b1)
        {
            AddDescrabption();
            b1 = true;
        }
    }
    public void AddBuff()
    {
        foreach (var buff in buffNameAndNumbers)
        {
            if (buff.effectOnPlayer && player != null)
            {
                foreach (var number in buff.number)
                {
                    for (int i = 0; i < number; i++)
                    {
                        player.AddBuff(buff.name);
                    }
                }
            }
            if (buff.effectOnEnemy && nepriz2 != null)
            {
                foreach (var number in buff.number)
                {
                    for (int i = 0; i < number; i++)
                    {
                        nepriz2.AddBuff(buff.name);
                    }
                }
            }
        }
        b = true;
    }
    public void AddProperity()
    {
        foreach (var normalProperity in normalProperity)
        {
            if (normalProperity.effectOnPlayer && player != null)
            {
                player.strength += normalProperity.strength;
                player.strength *= (normalProperity.strength1 + 1);
                player.tenacity += normalProperity.tenacity;
                player.tenacity *= (normalProperity.tenacity1 + 1);
                player.abnormalityResistance += normalProperity.abnormalityResistance;
                player.abnormalityResistance *= (normalProperity.abnormalityResistance1 + 1);
                player.burnResistance += normalProperity.burnResistance;
                player.burnResistance *= (normalProperity.burnResistance1 + 1);
                player.traumaResistance += normalProperity.traumaResistance;
                player.traumaResistance *= (normalProperity.traumaResistance1 + 1);
                if (player.unlockedFeathers.Count > 0)
                {
                    player.unlockedFeathers[0].health += normalProperity.unlockHealth;
                    player.unlockedFeathers[0].health *= (normalProperity.unlockHealth1 + 1);
                }
                if (player.feathers.Count > 0)
                {
                    player.feathers[0].health += normalProperity.lockHealth;
                    player.feathers[0].health *= (normalProperity.lockHealth1 + 1);
                }
            }
            if (normalProperity.effectOnEnemy && nepriz2 != null)
            {
                nepriz2.strength += normalProperity.strength;
                nepriz2.strength *= (normalProperity.strength1 + 1);
                nepriz2.tenacity += normalProperity.tenacity;
                nepriz2.tenacity *= (normalProperity.tenacity1 + 1);
                nepriz2.abnormalityResistance += normalProperity.abnormalityResistance;
                nepriz2.abnormalityResistance *= (normalProperity.abnormalityResistance1 + 1);
                nepriz2.burnResistance += normalProperity.burnResistance;
                nepriz2.burnResistance *= (normalProperity.burnResistance1 + 1);
                nepriz2.traumaResistance += normalProperity.traumaResistance;
                nepriz2.traumaResistance *= (normalProperity.traumaResistance1 + 1);
                if (nepriz2.unlockedFeathers.Count > 0)
                {
                    nepriz2.unlockedFeathers[0].health += normalProperity.unlockHealth;
                    nepriz2.unlockedFeathers[0].health *= (normalProperity.unlockHealth1 + 1);
                }
                if (nepriz2.feathers.Count > 0)
                {
                    nepriz2.feathers[0].health += normalProperity.lockHealth;
                    nepriz2.feathers[0].health *= (normalProperity.lockHealth1 + 1);
                }
                for (int i = 0; i < normalProperity.attackTimerList.Count; i++)
                {
                    nepriz2.attackCardLineList[normalProperity.attackTimerList[i].skillNumber].cards[0].timer += normalProperity.attackTimerList[i].time;
                }
                for (int i = 0; i < normalProperity.skillTimerList.Count; i++)
                {
                    nepriz2.effectCardLineList[normalProperity.skillTimerList[i].skillNumber].cards[0].timer += normalProperity.skillTimerList[i].time;
                }
            }
        }
        b=true;
    }
    public void Enter()
    {
        image.material = gb1;
    }
    public void Exit()
    {
        image.material = null;
    }
    void AddDescrabption()
    {
        Time.timeScale = timeNumber;
        gameObject.transform.parent.gameObject.SetActive(false);
        BuffScroll buffScroll = GameObject.Find("BuffScrollView").GetComponent<BuffScroll>();
        for (int i = 0; i < buffScroll.buffImformation.Count; i++)
        {
            if (buffScroll.buffImformation[i].name == "è¦´ÃÖ®×÷")
            {
                buffScroll.buffImformation[i].description += descrabption+"\n";
            }
        }
        buffScroll.AddGameObject();
    }
}
