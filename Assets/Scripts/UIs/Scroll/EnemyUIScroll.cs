using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EnemyUIScroll : Scroll
{
    Dictionary<Enemy, Transform> dict = new();
    public void AddEnemyUI(Enemy enemy)
    {
        if (!dict.ContainsKey(enemy))
        {
            Transform trans = Additem(itemTransform);
            dict[enemy] = trans;

            Text featherNumText = trans.Find("enemyLockHPNum").GetComponent<Text>();
            featherNumText.text = (enemy.feathers.Count + enemy.unlockedFeathers.Count).ToString();
            Text enemyName = trans.Find("enemyName").GetComponent<Text>();
            enemyName.text = enemy.enemyName;

            ImageSpriteSyner enemyHeadImageSyner = trans.Find("enemyHead").GetComponent<ImageSpriteSyner>();
            enemyHeadImageSyner.targetSpriteRenderer = enemy.spriteRenderer;

            Scroll enemyUnlockedFeatherScroll = trans.Find("enemyHPScrollView").GetComponent<Scroll>();
            foreach (Feather feather in enemy.unlockedFeathers)
            {
                HpUI hpUI = enemyUnlockedFeatherScroll.AddHp();
                hpUI.testTime = feather.lockTimer;
                hpUI.testHp = feather.health;
                hpUI.testHpMax = feather.maxHealth;
                feather.hpUI = hpUI;
            }
            enemy.hpScroll = enemyUnlockedFeatherScroll;
        }
    }

    public void RemoveEnemyUI(Enemy enemy)
    {
        if (dict.ContainsKey(enemy))
        {
            enemy.hpScroll = null;

            Transform trans = dict[enemy];
            dict.Remove(enemy);
            Destroy(trans.gameObject);
        }
    }
}
