using System.Collections;
using System.Collections.Generic;
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

    ArchiveManager archiveManager;
    EquipmentPanelManager equipmentPanelManager;

    new private void Start()
    {
        base.Start();
        archiveManager = FindAnyObjectByType<ArchiveManager>();
        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();
        cardController = FindAnyObjectByType<PlayerCardController>();
        cardGenerateText = GameObject.FindGameObjectWithTag(Consts.CardGenerateTextTag).GetComponent<Text>();

        tenacity = archiveManager.currentArchive.playerInfo.tenacity;
        strength = archiveManager.currentArchive.playerInfo.strength;
    }

    /// <summary>
    /// Update
    /// </summary>
    public override void AIUpdate()
    {
        base.AIUpdate();

        CardGenerateUpdate();
    }

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
            int index = Random.Range(0, cardGenerateList.Count);
            string card = cardGenerateList[index];

            cardController.GetCard(card);
        }
    }

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
    /// 展示血条
    /// </summary>
    /// <param name="feather"></param>
    public override void ShowUnlockFeather(Feather feather)
    {
        base.ShowUnlockFeather(feather);
        if (hpScroll != null)
        {
            HpUI hpUI = hpScroll.AddHp();
            hpUI.testTime = feather.lockTimer;
            hpUI.testHp = feather.health;
            hpUI.testHpMax = feather.maxHealth;
            feather.hpUI = hpUI;
        }
    }

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
        }
    }
}
