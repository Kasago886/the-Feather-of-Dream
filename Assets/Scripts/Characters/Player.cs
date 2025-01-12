using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : Character
{
    ArchiveManager archiveManager;
    EquipmentPanelManager equipmentPanelManager;
    public Scroll hpScroll;

    public List<Card> cardGenerateList;
    public float cardGenerateCooldown;
    public float cardGenerateCooldownTimer = 0;
    public Text cardGenerateText;

    new private void Start()
    {
        base.Start();
        archiveManager = FindAnyObjectByType<ArchiveManager>();
        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();

        tenacity = archiveManager.currentArchive.playerInfo.tenacity;
        strength = archiveManager.currentArchive.playerInfo.strength;
    }

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
            cardGenerateCooldownTimer = cardGenerateCooldown;

            GenerateCard();
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

    public override void ShowUnlockFeather(Feather feather)
    {
        base.ShowUnlockFeather(feather);

        HpUI hpUI = hpScroll.AddHp();

        hpUI.testTime = feather.lockTimer;
        hpUI.testHp = feather.health;
        hpUI.testHpMax = feather.maxHealth;
        
        feather.hpUI = hpUI;
    }

}
