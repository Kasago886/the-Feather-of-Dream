using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Player : Character
{
    public Text featherNumText;

    public List<string> cardGenerateList;
    public float cardGenerateCooldown;
    public float cardGenerateCooldownTimer = 0;

    public PlayerCardController cardController;

    [HideInInspector] public Text cardGenerateText;
    [HideInInspector] public bool isSprinting = false;
    [HideInInspector] public int baseTenacity;
    [HideInInspector] public int baseStrength;
    [HideInInspector] public bool setUped = false;

    float healthRecoverTimer = 0;
    float healthRecoverCooldown = 10;
    float healthRecoverSpeed = 1;

    ArchiveManager archiveManager;
    EquipmentPanelManager equipmentPanelManager;
    InputListener inputListener;
    [HideInInspector] public PlayerController playerController;

    static public Dictionary<int,List<int>> level_maxExp_tenacity_strength = new Dictionary<int, List<int>>
    {
        {0,new List<int> {20,0,0}},
        {1,new List<int> {20,10,10}},
        {2,new List<int> {20,20,20}},
        {3,new List<int> {20,30,30}},
        {4,new List<int> {20,40,40}},
    };

    new private void Start()
    {
        base.Start();
        inputListener = FindAnyObjectByType<InputListener>();
        playerController = GetComponent<PlayerController>();

        if (!setUped)
            SetUp();
    }

    /// <summary>
    /// 初始化（被EquipmentPanelManager引用，保证在其之前初始化，方便计算Item附加的属性）
    /// </summary>
    public void SetUp()
    {
        archiveManager = FindAnyObjectByType<ArchiveManager>();
        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();
        cardController = FindAnyObjectByType<PlayerCardController>();
        cardGenerateText = GameObject.FindGameObjectWithTag(Consts.CardGenerateTextTag).GetComponent<Text>();

        List<int> levelInfo = level_maxExp_tenacity_strength[archiveManager.currentArchive.playerInfo.level];
        archiveManager.currentArchive.playerInfo.maxExp = levelInfo[0];
        baseTenacity = levelInfo[1];
        baseStrength = levelInfo[2];

        tenacity = baseTenacity;
        strength = baseStrength;

        featherNumText.text = (archiveManager.currentArchive.equipedFeather.items.Length).ToString();

        setUped = true;
    }

    /// <summary>
    /// Update
    /// </summary>
    public override void AIUpdate()
    {
        base.AIUpdate();

        CardGenerateUpdate();
        HealthRecoverUpdate();

        //Debug.Log("unlock feathers:" + unlockedFeathers.Count.ToString() + "\nfeathers:" + feathers.Count.ToString());
    }

    /// <summary>
    /// 攻击动画
    /// </summary>
    public override void OnAttack()
    {
        base.OnAttack();

        if (attackBodyObjList.Count > 0)
        {
            animator.Play("Attack");
        }
    }

    #region Card
    /// <summary>
    /// 更新卡牌生成
    /// </summary>
    public void CardGenerateUpdate()
    {
        if (cardGenerateCooldownTimer <= 0)
        {
            //Debug.Log("cardController.GetCardOrNot()="+cardController.GetCardOrNot());
            if (cardController.GetCardOrNot())
            {
                cardGenerateCooldownTimer = cardGenerateCooldown;
                GenerateCard();
            }
        }
        else
        {
            cardGenerateCooldownTimer -= Time.deltaTime;
            if (cardGenerateCooldownTimer < 0) { cardGenerateCooldownTimer = 0; }
        }      
            cardGenerateText.text = ((int)cardGenerateCooldownTimer).ToString() + "s";
    }
    /// <summary>
    /// 生成卡牌
    /// </summary>
    public void GenerateCard()
    {
        if (cardGenerateList.Count > 0 && cardController.GetCardOrNot())
        {
            /// 根据稀有度设置权重范围
            /// 权重份数 = 6 - 稀有度
            /// 例：A,B,C,D的权重为1,2,3,4
            /// 则对应数据为：
            /// i | cardNameWithEndIndex | index (Range)
            ///   | Key        | Value   | 
            ///   | endIndexes |         |
            /// 0   5            A          0 -  4
            /// 1   9            B          5 -  8
            /// 2   12           C          9 - 11
            /// 3   14           D         12 - 13
            Dictionary<int, string> cardNameWithEndindex = new();
            int endIndex = 0;
            foreach (string cardName in cardGenerateList)
            {
                endIndex += 6 - cardController.GetRarity(cardName);
                cardNameWithEndindex[endIndex] = cardName;
            }
            List<int> endIndexes = cardNameWithEndindex.Keys.ToList();

            int index = Random.Range(0, endIndexes[^1]);
            for (int i = 0; i < endIndexes.Count; i++)
            {
                endIndex = endIndexes[i];
                if (endIndex > index)
                {
                    string cardName = cardNameWithEndindex[endIndex];
                    cardController.GetCard(cardName);
                    break;
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// 添加梦
    /// </summary>
    /// <param name="num"></param>
    public void AddDream(int num)
    {
        archiveManager.currentArchive.playerInfo.dream += num;

        equipmentPanelManager.SetUpPlayerInfo();
    }

    /// <summary>
    /// 添加经验
    /// </summary>
    /// <param name="exp"></param>
    public void AddExp(int exp)
    {
        bool upgrade = false;

        archiveManager.currentArchive.playerInfo.currentExp += exp;
        while (archiveManager.currentArchive.playerInfo.currentExp >= archiveManager.currentArchive.playerInfo.maxExp)
        {
            archiveManager.currentArchive.playerInfo.currentExp -= archiveManager.currentArchive.playerInfo.maxExp;
            archiveManager.currentArchive.playerInfo.level += 1;

            List<int> levelInfo = level_maxExp_tenacity_strength[archiveManager.currentArchive.playerInfo.level];
            archiveManager.currentArchive.playerInfo.maxExp = levelInfo[0];

            upgrade = true;
        }

        //升级后重新计算属性
        if (upgrade)
        {
            //原附加值
            float addTenacity = tenacity - baseTenacity;
            float addStrength = strength - baseStrength;

            List<int> levelInfo = level_maxExp_tenacity_strength[archiveManager.currentArchive.playerInfo.level];
            baseTenacity = levelInfo[1];
            baseStrength = levelInfo[2];

            archiveManager.currentArchive.playerInfo.tenacity = baseTenacity;
            archiveManager.currentArchive.playerInfo.strength = baseStrength;

            tenacity = baseTenacity + addTenacity;
            strength = baseStrength + addStrength;
        }

        equipmentPanelManager.SetUpPlayerInfo();
    }

    #region feather
    /// <summary>
    /// 添加羽
    /// </summary>
    /// <param name="feather"></param>
    public override void AddFeather(Feather feather)
    {
        base.AddFeather(feather);
        Debug.Log("add feather:" + feather.GetType());
        archiveManager.currentArchive.playerInfo.feather = feathers.Count + unlockedFeathers.Count;
    }
    /// <summary>
    /// 移除羽
    /// </summary>
    /// <param name="feather"></param>
    public override void RemoveFeather(Feather feather)
    {
        base.RemoveFeather(feather);
        archiveManager.currentArchive.playerInfo.feather = feathers.Count + unlockedFeathers.Count;
    }

    /// <summary>
    /// 展示血条
    /// </summary>
    /// <param name="feather"></param>
    public override void ShowUnlockFeather(Feather feather)
    {
        base.ShowUnlockFeather(feather);
        if (hpScroll != null)
        {
            hpScroll.ClearAllContent();

            //Debug.Log(unlockedFeathers.Count);
            foreach (var item in unlockedFeathers)
            {
                HpUI hpUI = hpScroll.AddHp();
                hpUI.testTime = item.lockTimer;
                hpUI.testHp = item.health;
                hpUI.testHpMax = item.maxHealth;
                item.hpUI = hpUI;
                hpUI.targetFeather = item;
            }
        }
    }
    #endregion

    /// <summary>
    /// 受伤后更新羽数量
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="attackTrans"></param>
    public override void TakeDamage(float damage, Transform attackTrans = null)
    {
        if (!isSprinting)
        {
            base.TakeDamage(damage, attackTrans);

            featherNumText.text = (feathers.Count + unlockedFeathers.Count).ToString();
            healthRecoverTimer = healthRecoverCooldown;
        }
    }

    /// <summary>
    /// 回血
    /// </summary>
    void HealthRecoverUpdate()
    {
        healthRecoverTimer -= Time.deltaTime;
        //一段时间不受伤时回血
        if (healthRecoverTimer < 0)
        {
            //找到血量不满的羽
            Feather recoverFeather = null;
            if (feathers.Count > 0)
            {
                foreach (Feather feather in feathers)
                {
                    if (feather.health < feather.maxHealth)
                    {
                        recoverFeather = feather;
                        break;
                    }
                }

            }
            if (recoverFeather == null && unlockedFeathers.Count > 0)
            {
                foreach (Feather feather in unlockedFeathers)
                {
                    if (feather.health < feather.maxHealth)
                    {
                        recoverFeather = feather;
                        break;
                    }
                }
            }

            //回血
            if (recoverFeather != null)
            {
                recoverFeather.health += healthRecoverSpeed * Time.deltaTime;
                if (recoverFeather.health > recoverFeather.maxHealth)
                {
                    recoverFeather.health = recoverFeather.maxHealth;
                }
            }
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public override void OnDeath()
    {
        base.OnDeath();

        playerController.OnDie();

        animator.SetBool(Consts.IsDeadAnimatorArgument, true);
    }

    /// <summary>
    /// 死亡界面
    /// </summary>
    public void DeathPanel()
    {
        inputListener.OnDeath();
    }
}
