using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.WSA;
using static UnityEditor.Experimental.GraphView.GraphView;
[System.Serializable]
public class Buff
{
    //基础信息
    public string name;
    public string description;
    public Sprite sprite;
    //作用对象
    public Character target = null;
    //持续时间
    public float timer = 0;
    //永久存在
    public bool isPermanent = false;

    /// <summary>
    /// 初始化（某些参数对特定类型的buff无效）
    /// </summary>
    /// <param name="target">作用对象</param>
    /// <param name="timer">持续时间</param>
    /// <param name="isPermanent">是否永久存在</param>
    public virtual void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        this.target = target;
        this.timer = timer;
        this.isPermanent = isPermanent;
        this.name = "无名buff";
        this.description = "没有任何效果";
        this.sprite = Resources.Load<Sprite>("BuffIcon/testIcon");
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnExit()
    {

    }
}

#region 装备残羽buff
/// <summary>
/// 装备buff基类
/// </summary>
public class EquipmentBuff : Buff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.isPermanent = true;
    }
}

public class TestEquipmentBuff : EquipmentBuff
{
    bool isUpdated = false;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("TestEquipmentBuff is added!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!isUpdated)
        {
            Debug.Log("TestEquipmentBuff is updated succesfully!");
            isUpdated = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("TestEquipmentBuff is Removed!");
    }
}
public class HunterEquipmentBuff : EquipmentBuff
{
    Player player;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
    }

    public override void OnEnter()
    {
        player = target.GetComponent<Player>();
        base.OnEnter();
        player.cardGenerateList.Add("狂猎之枪");
        player.cardGenerateList.Add("噬血匕首");
        player.cardGenerateList.Add("起动");
        player.cardGenerateList.Add("猎杀");
        player.strength += 10;


    }
    public override void OnUpdate()
    {
    }
    public override void OnExit()
    {
        base.OnExit();
        player.strength -= 10;
        player.cardGenerateList.Remove("狂猎之枪");
        player.cardGenerateList.Remove("噬血匕首");
        player.cardGenerateList.Remove("起动");
        player.cardGenerateList.Remove("猎杀");

    }
}
public class TinWoodmanEquipmentBuff : EquipmentBuff
{
    Player player;

    public override void OnEnter()
    {
        base.OnEnter();
        player = target.GetComponent<Player>();
        player.tenacity += 10;

        player.cardGenerateList.Add("铁心");

        //Debug.Log("add");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        player = target.GetComponent<Player>();
        player.tenacity -= 10;

        player.cardGenerateList.Remove("铁心");
    }
}
public class DwarfsEquipmentBuff : EquipmentBuff
{
    Player player;

    public override void OnEnter()
    {
        base.OnEnter();
        player = target.GetComponent<Player>();
        player.tenacity += 5;

        player.cardGenerateList.Add("手巧");

        //Debug.Log("add");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        player = target.GetComponent<Player>();
        player.tenacity -= 5;

        player.cardGenerateList.Remove("手巧");
    }
}
public class FragPrinceEquipmentBuff : EquipmentBuff
{
    Player player;

    public override void OnEnter()
    {
        base.OnEnter();
        player = target.GetComponent<Player>();
        player.tenacity += 5;
        player.strength += 5;

        player.cardGenerateList.Add("护卫");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        player = target.GetComponent<Player>();
        player.tenacity -= 5;
        player.strength -= 5;

        player.cardGenerateList.Remove("护卫");
    }
}
public class ContaminatedCloneEquipmentBuff : EquipmentBuff
{
    Player player;
    PlayerController controller;

    public override void OnEnter()
    {
        base.OnEnter();
        player = target.GetComponent<Player>();
        controller = target.GetComponent<PlayerController>();
        controller.walkSpeed += 1;
        player.cardGenerateList.Add("污浊");

        //Debug.Log("add");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        player = target.GetComponent<Player>();
        controller = target.GetComponent<PlayerController>();
        player.tenacity -= 5;
        controller.walkSpeed -= 1;
        player.cardGenerateList.Remove("污浊");
    }
}
public class EnslavedDwarfsEquipmentBuff : EquipmentBuff
{
    Player player;

    public override void OnEnter()
    {
        base.OnEnter();
        player = target.GetComponent<Player>();
        player.tenacity += 5;

        player.cardGenerateList.Add("破链反击");
        player.cardGenerateList.Add("枷锁怒涛");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        player = target.GetComponent<Player>();
        player.tenacity -= 5;

        player.cardGenerateList.Remove("破链反击");
        player.cardGenerateList.Remove("枷锁怒涛");
    }
}
public class BurningDocumentsEquipmentBuff : EquipmentBuff
{
    Player player;
    private float timer1;
    public override void OnEnter()
    {
        base.OnEnter();
        player = target.GetComponent<Player>();
        player.tenacity += 15;
        player.burnResistance += 2;
        player.traumaResistance -= 2;

        player.cardGenerateList.Add("悲剧");
        player.cardGenerateList.Add("真相");
        player.cardGenerateList.Add("否认");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        timer += Time.deltaTime;
        if (timer > 12)
        {
            timer = 0;
            player.AddBuff("灼伤");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        player.tenacity -= 15;
        player.burnResistance -= 2;
        player.traumaResistance += 2;
        player.cardGenerateList.Remove("悲剧");
        player.cardGenerateList.Remove("真相");
        player.cardGenerateList.Remove("否认");
    }
}
public class NeprizEquipmentBuff : EquipmentBuff
{
    Player player;
    private PlayerCardController playerCardController;
    public override void OnEnter()
    {
        base.OnEnter();
        player = target.GetComponent<Player>();
        player.tenacity += 20;
        player.strength += 10;
        if (GameObject.Find("CardPanel") != null)
        {
            playerCardController = GameObject.Find("CardPanel").GetComponent<PlayerCardController>();
            playerCardController.positionNumber--;
        }
        player.cardGenerateList.Add("痴愚者");
        player.cardGenerateList.Add("蒙昧者");
        player.cardGenerateList.Add("自欺者");
        player.cardGenerateList.Add("释然者");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        player.tenacity -= 20;
        player.strength -= 10;
        playerCardController.positionNumber++;
        player.cardGenerateList.Remove("痴愚者");
        player.cardGenerateList.Remove("蒙昧者");
        player.cardGenerateList.Remove("自欺者");
        player.cardGenerateList.Remove("释然者");
    }
}
public class TheMisunderstoodWerewolfEquipmentBuff : EquipmentBuff
{
    Player player;
    private PlayerCardController playerCardController;
    public override void OnEnter()
    {
        //误解 成见    倒施 罪人
        base.OnEnter();
        player = target.GetComponent<Player>();
        player.tenacity += 10;
        player.strength += 5;
        player.abnormalityResistance += 10;
        player.traumaResistance += 2;
        player.cardGenerateList.Add("误解");
        player.cardGenerateList.Add("成见");
        player.cardGenerateList.Add("倒施");
        player.cardGenerateList.Add("罪人");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        player.tenacity -= 10;
        player.strength -= 5;
        player.abnormalityResistance -= 10;
        player.traumaResistance -= 2;
        player.cardGenerateList.Remove("误解");
        player.cardGenerateList.Remove("成见");
        player.cardGenerateList.Remove("倒施");
        player.cardGenerateList.Remove("罪人");
    }
}
#endregion

#region 装备羽buff
/// <summary>
/// 装备羽buff基类
/// </summary>
public class EquipmentFeatherBuff : EquipmentBuff
{
    public EquipmentFeather feather = null;

    public override void OnEnter()
    {
        base.OnEnter();

        //Debug.Log(target);
        //Debug.Log(feather);
        target.AddFeather(feather);
    }

    public override void OnExit()
    {
        base.OnExit();

        target.RemoveFeather(feather);
    }
}
public class TestEquipmentFeatherBuff : EquipmentFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.feather = new TestEquipmentFeather();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("testEquipmentFeatherBuff added!");
    }

    public override void OnExit()
    {
        base.OnExit();

        Debug.Log("testEquipmentFeatherBuff removed!");
    }
}

public class BurningDocumentsFeatherBuff : EquipmentFeatherBuff
{
    Player player;
    private int oriNumber;
    private int oriCount;
    int attackbodyOriNum;
    int attackbodyNewNum;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.feather = new EllieEquipmentFeather();
        this.player = target as Player;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //档案 机能    档案 情感    档案 体质    档案 疾病    档案 免疫
        //档案 适应    档案 温度    档案 饮食    档案 交际    档案 遗传
        //档案 排异    档案 反馈    档案 表征    档案 死亡    档案 机密
        player.cardGenerateList.Add("档案 机能");
        player.cardGenerateList.Add("档案 情感");
        player.cardGenerateList.Add("档案 体质");
        player.cardGenerateList.Add("档案 疾病");
        player.cardGenerateList.Add("档案 免疫");
        player.cardGenerateList.Add("档案 适应");
        player.cardGenerateList.Add("档案 温度");
        player.cardGenerateList.Add("档案 饮食");
        player.cardGenerateList.Add("档案 交际");
        player.cardGenerateList.Add("档案 遗传");
        player.cardGenerateList.Add("档案 排异");
        player.cardGenerateList.Add("档案 反馈");
        player.cardGenerateList.Add("档案 表征");
        player.cardGenerateList.Add("档案 死亡");
        player.cardGenerateList.Add("档案 机密");
        player.tenacity += 15;
        player.abnormalityResistance += 10;
        player.burnResistance += 4;
        attackbodyOriNum = target.attackBodyObjList.Count;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        //纷飞之页
        if (player.buffList.Count !=oriCount)
        {
            List<string> list = new List<string>();
            foreach (var buff in player.buffList)
            {
                if (list.Count == 0)
                {
                    list.Add(buff.name);
                }
                else if(!list.Contains(buff.name))
                {
                    list.Add(buff.name);
                }
            }
            player.strength += list.Count - oriNumber;
            oriNumber=list.Count;
            oriCount=player.buffList.Count;
        }
        //蚕食之火
        attackbodyNewNum = target.attackBodyObjList.Count;
        if (attackbodyNewNum > attackbodyOriNum)
        {
            target.attackBodyObjList[attackbodyNewNum - 1].GetComponent<AttackBody>().buffNames.Add("灼伤");
            if (Random.Range(0, 10) >= 8)
            {
                player.AddBuff("灼伤");
            }
        }
        if (attackbodyNewNum < attackbodyOriNum)
        {
            attackbodyOriNum = attackbodyNewNum;
        }
    }
    public override void OnExit()
    {
        base.OnExit();

        player.cardGenerateList.Remove("档案 机能");
        player.cardGenerateList.Remove("档案 情感");
        player.cardGenerateList.Remove("档案 体质");
        player.cardGenerateList.Remove("档案 疾病");
        player.cardGenerateList.Remove("档案 免疫");
        player.cardGenerateList.Remove("档案 适应");
        player.cardGenerateList.Remove("档案 温度");
        player.cardGenerateList.Remove("档案 饮食");
        player.cardGenerateList.Remove("档案 交际");
        player.cardGenerateList.Remove("档案 遗传");
        player.cardGenerateList.Remove("档案 排异");
        player.cardGenerateList.Remove("档案 反馈");
        player.cardGenerateList.Remove("档案 表征");
        player.cardGenerateList.Remove("档案 死亡");
        player.cardGenerateList.Remove("档案 机密");
        player.tenacity -= 15;
        player.abnormalityResistance -= 10;
        player.burnResistance -= 4;
    }
}
public class AddFollower : MonoBehaviour
{
    public GameObject follower;
    public void Add()
    {
        follower=Instantiate(Resources.Load<GameObject>("AttackBodys / Nepriz / Follwer.prefab"),new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y,0), Quaternion.identity);
    }
    public void Del()
    {
        if (follower != null)
        {
            Destroy(follower);
        }
    }
}
public class NeprizFeatherBuff : EquipmentFeatherBuff 
{
    Player player;
    private PlayerCardController cardController;
    private float health;
    private float timer1, timer2;
    private float featherCount;
    private List<string> debuffContain = new List<string> { "伤痕", "忧郁", "崩溃", "凡庸", "萎靡",
        "脆弱", "迟缓", "凝重", "中毒", "灼伤", "异常脆弱","灼伤脆弱","伤痕脆弱" };
    private AddFollower follower;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.feather = new EllieEquipmentFeather();
        this.player = target as Player;
        cardController = GameObject.Find("CardPanel").GetComponent<PlayerCardController>();
        target.gameObject.AddComponent<AddFollower>();
        follower=target.GetComponent<AddFollower>();
    }

    public override void OnEnter()
    {
        //选题定向 文献回顾    设计规划 获取批准    收集数据
         // 数据分析    得出结论 撰写报告    发表分享
        base.OnEnter();
        player.cardGenerateList.Add("选题定向");
        player.cardGenerateList.Add("文献回顾");
        player.cardGenerateList.Add("设计规划");
        player.cardGenerateList.Add("获取批准");
        player.cardGenerateList.Add("收集数据");
        player.cardGenerateList.Add("数据分析");
        player.cardGenerateList.Add("得出结论");
        player.cardGenerateList.Add("撰写报告");
        player.cardGenerateList.Add("发表分享");
        player.tenacity += 30;
        player.strength += 20;
        player.abnormalityResistance += 10;
        cardController.AddPosition();
        follower.Add();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;
        //心防
        if (timer1 > 5)
        {
            timer1 = 0;
            float health0 = 0;
            if (featherCount == player.feathers.Count)
            {
                float debuffNumber = 0;
                for (int i = 0; i < player.feathers.Count; i++)
                {
                    health0 += player.feathers[i].health;
                }
                if (health - health0 > 0)
                {
                    foreach (var buff in player.buffList)
                    {
                        if (debuffContain.Contains(buff.name))
                        {
                            debuffNumber++;
                        }
                    }
                }
                player.shields.Add(new Shield() { health = health0 * debuffNumber / 10, timer = 999999999 });
            }
            featherCount = player.feathers.Count;
            health = health0;
        }
        //自扰
        if (timer2 > 60)
        {
            for (int i = 0; i < 3; i++)
            {
                int n = Random.Range(0, 30);
                switch (n)
                {
                    case 0: player.AddBuff("治愈Ⅲ型"); break;
                    case 1: player.AddBuff("治愈Ⅱ型"); break;
                    case 2: player.AddBuff("治愈Ⅰ型"); break;
                    case 3: player.AddBuff("灼伤");i--; break;
                    case 4: player.AddBuff("正义"); break;
                    case 5: player.AddBuff("中毒"); i--; break;
                    case 6: player.AddBuff("凝重"); i--; break;
                    case 7: player.AddBuff("迟缓"); i--; break;
                    case 8: player.AddBuff("脆弱"); i--; break;
                    case 9: player.AddBuff("萎靡"); i--; break;
                    case 10: player.AddBuff("卓越"); break;
                    case 11: player.AddBuff("坚定"); break;
                    case 12: player.AddBuff("轻健"); break;
                    case 13: player.AddBuff("神速"); break;
                    case 14: player.AddBuff("坚韧"); break;
                    case 15: player.AddBuff("振奋"); break;
                    case 16: player.AddBuff("博学"); break;
                    case 17: player.AddBuff("才华"); break;
                    case 18: player.AddBuff("凡庸"); i--; break;
                    case 19: player.AddBuff("崩溃"); i--; break;
                    case 20: player.AddBuff("忧郁"); i--; break;
                    case 21: player.AddBuff("惊惶"); i--; break;
                    case 22: player.AddBuff("伤痕"); i--; break;
                    case 23: player.AddBuff("麻木"); break;
                    case 24: player.AddBuff("异常抵抗"); break;
                    case 25: player.AddBuff("异常脆弱"); i--; break;
                    case 26: player.AddBuff("灼伤抵抗"); break;
                    case 27: player.AddBuff("灼伤脆弱"); i--; break;
                    case 28: player.AddBuff("伤痕抵抗"); break;
                    case 29: player.AddBuff("伤痕脆弱"); i--; break;
                }
            }
            timer2 = 0;
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        player.cardGenerateList.Remove("选题定向");
        player.cardGenerateList.Remove("文献回顾");
        player.cardGenerateList.Remove("设计规划");
        player.cardGenerateList.Remove("获取批准");
        player.cardGenerateList.Remove("收集数据");
        player.cardGenerateList.Remove("数据分析");
        player.cardGenerateList.Remove("得出结论");
        player.cardGenerateList.Remove("撰写报告");
        if (player.cardGenerateList.Contains("发表分享"))
        {
            player.cardGenerateList.Remove("发表分享");
        }
        player.tenacity -= 30;
        player.strength -= 20;
        player.abnormalityResistance -= 10;
        cardController.DelPosition();
        follower.Del();
    }
}

public class EllieEquipmentFeatherBuff : EquipmentFeatherBuff
{
    Player player;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.feather = new EllieEquipmentFeather();
        this.player = target as Player;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        player.cardGenerateList.Add("艾莉之剑");
        player.cardGenerateList.Add("腐败");
        player.cardGenerateList.Add("正义");
    }

    public override void OnExit()
    {
        base.OnExit();

        player.cardGenerateList.Remove("艾莉之剑");
        player.cardGenerateList.Remove("腐败");
        player.cardGenerateList.Remove("正义");
    }
}

#endregion

#region 攻击buff
/// <summary>
/// 攻击buff基类
/// </summary>
public class AttackBuff : Buff
{
    //攻击替身
    public GameObject attackBody = null;

    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.isPermanent = false;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        target.AddAttackBody(attackBody);
    }

    public override void OnExit()
    {
        base.OnExit();

        target.RemoveAttackBody(attackBody);
    }
}

public class TestAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 10;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/testAttackBody");
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("TestAttackBuff added!");
    }

    public override void OnExit()
    {
        base.OnExit();

        Debug.Log("TestAttackBuff removed!");
    }
}

public class TestEnemyAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 10;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/testEnemyAttackBody");
    }
}
public class ElliesSwordAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 10;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/Ellie'sSwordAttackBody");
        //Debug.Log(attackBody);
    }
}

public class PrinceSwordAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 5;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/PrinceSwordAttackBody");
        //Debug.Log(attackBody);
    }
}
public class PrinceGuardSwordAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 5;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/PrinceGuardSwordAttackBody");
        //Debug.Log(attackBody);
    }
}

public class CrazyHunterAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 5;
        if (target.GetComponent<CrazyHunter>().attackWay == true)
        {
            this.attackBody = Resources.Load<GameObject>("AttackBodys/CrazyHunter/CrazyHunterAttackBody");
            target.GetComponent<CrazyHunter>().tenacity = 10;
        }
        else
        {
            this.attackBody = Resources.Load<GameObject>("AttackBodys/CrazyHunter/CrazyHunterAttackBodyGun");
            target.GetComponent<CrazyHunter>().tenacity = -10;

        }
    }
}
public class CrazyHunterAttackBuff1 : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 1;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/CrazyHunter/CrazyHunterAttackBodyTrap");
        target.GetComponent<Enemy>().attackCooldownTimer = 0;

    }
}
public class TinWoodmanAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 6;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/TinWoodmanAttackBody");
    }
}
public class EnslavedDwarfsAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 8;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/EnslavedDwarfsAttackBody");
    }
}
public class DwarfsAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 8;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/DwarfsAttackBody");
    }
}
public class TheMisunderstoodWerewolfBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 7;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/TheMisunderstoodWerewolfAttackBody 1");
    }
}
public class HunterFeatherAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 7;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/CrazyHunter/HunterFeatherAttackBody");
    }
}
public class HunterFeatherAttackBuffGun : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 7;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/CrazyHunter/HunterFeatherAttackBodyGun");
    }
}
public class BurningDocumentsAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 12;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/BurningDocumentsAttackBody");
    }
}


public class CorruptionAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 5;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/PoisonAttackBody");
    }
}
public class ContaminatedCloneAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 8;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/ContaminatedCloneAttackBody");
        target.GetComponent<Character>().abnormalityResistance += Random.Range(-5, 15);
    }
}
public class NeprizAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 17;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/Nepriz/NeprizAttackBody");
    }
}
public class ExperimentPlayerAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 5;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/ExperimentPlayer/ExperimentPlayerAttackBody");
    }
}
public class ExperimentPlayerAttackBuff1 : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 5;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/ExperimentPlayer/ExperimentPlayerAttackBody1");
    }
}
#endregion

#region 拔羽buff
/// <summary>
/// 拔羽buff基类
/// </summary>
public class UnlockFeatherBuff : Buff
{
    //拔羽数
    public int unlockFeatherNum = 0;

    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.isPermanent = false;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        target.UnlockFeather(unlockFeatherNum, timer);
    }
}

public class TestUnlockFeatherBuff : UnlockFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 10;
        this.unlockFeatherNum = 1;
    }
}

public class UnlockFeather5sBuff : UnlockFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 5;
        this.unlockFeatherNum = 1;
    }
}

public class UnlockFeather10sBuff : UnlockFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 10;
        this.unlockFeatherNum = 1;
    }
}

public class UnlockFeather15sBuff : UnlockFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 15;
        this.unlockFeatherNum = 1;
    }
}

public class UnlockFeather20sBuff : UnlockFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 20;
        this.unlockFeatherNum = 1;
    }
}

#endregion

#region 效果buff
/// <summary>
/// 效果buff基类
/// </summary>
public class EffectBuff : Buff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.isPermanent = false;
    }
}

public class TestEffectBuff : EffectBuff
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("add TestEffectBuff!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("remove TestEffectBuff!");
    }
}

public class TestEnemyEffectBuff : EffectBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 5;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //Debug.Log("add TestEnemyEffectBuff!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        //Debug.Log("remove TestEnemyEffectBuff!");
    }
}
public class CrazyHunterEffectBuff : EffectBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 5;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        target.GetComponent<Enemy>().runSpeed *= 1.5f;
        target.GetComponent<Enemy>().strength = 80;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        target.GetComponent<Enemy>().runSpeed /= 1.5f;
        target.GetComponent<Enemy>().strength = 0;
    }
}
public class TinWoodmanEffectBuff : EffectBuff
{
    private float buffTimer;
    private float oriStrength;
    private float orihealth;
    private Enemy enemy;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 6;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy = target.GetComponent<Enemy>();
        if (enemy.unlockedFeathers.Count > 0)
        {
            enemy.unlockedFeathers[0].health -= enemy.unlockedFeathers[0].health / 40;
            orihealth = enemy.unlockedFeathers[0].health;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        buffTimer += Time.deltaTime;
        if (buffTimer > 2)
        {
            enemy.strength += enemy.oriStrength / 10;
            buffTimer = 0;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        if (enemy.unlockedFeathers.Count > 0 && orihealth - enemy.unlockedFeathers[0].health > 20)
        {
            if (enemy.unlockedFeathers[0].health >= 40)
            {
                enemy.unlockedFeathers[0].health -= 40;
            }
            if (enemy.unlockedFeathers.Count > 1 && enemy.unlockedFeathers[0].health < 40)
            {
                enemy.unlockedFeathers[1].health -= (40 - enemy.unlockedFeathers[0].health);
                enemy.unlockedFeathers[0].health = 0;
            }
        }
    }
}
public class EnslavedDwarfsEffectBuff : EffectBuff
{
    private Player player;
    private Enemy enemy;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Player>() != null)
        {
            player = target.GetComponent<Player>();
        }
        if (target.GetComponent<Enemy>() != null)
        {
            enemy = target.GetComponent<Enemy>();
        }
        float health = 0;
        if (enemy != null)
        {
            for (int i = 0; i < enemy.unlockedFeathers.Count; i++)
            {
                health += enemy.unlockedFeathers[i].health;
            }
            if (enemy.unlockedFeathers.Count > 0)
            {
                enemy.unlockedFeathers[0].health += health / 20;
                enemy.unlockedFeathers[0].maxHealth += health / 20;
            }
        }
        if (player != null)
        {
            for (int i = 0; i < player.unlockedFeathers.Count; i++)
            {
                health += player.unlockedFeathers[i].health;
            }
            if (player.unlockedFeathers.Count > 0)
            {
                player.unlockedFeathers[0].health += health / 20;
                player.unlockedFeathers[0].maxHealth += health / 20;
            }
        }
        this.timer = 0;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class PrincePowerEffectBuff : EffectBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 5;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        target.strength += 20;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        target.strength -= 20;
    }
}
public class DwarfsEffectBuff : EffectBuff
{
    float Num;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 5;
        //Num = target.GetComponent<Dwarfs>().DwarfsNumber;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        target.strength += 20 * Num;
        target.tenacity += 20 * Num;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        target.strength -= 20 * Num;
        target.tenacity -= 20 * Num;
    }
}
public class Trauma : EffectBuff
{
    private Character character;
    private float health;
    private float attackTimer;
    private int buffNumber;
    private bool isAddBuff;
    public float damage = 1;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
            if (character.unlockedFeathers.Count > 0)
            {
                health = character.unlockedFeathers[0].health;
            }
        }
        foreach (var buff in character.buffList)
        {
            if (buff.name == "伤痕")
            {
                buffNumber++;
            }
            if (buff.name == "裂隙")
            {
                isAddBuff = true;
            }
        }
        character.traumaNumber[1] = buffNumber;
        if (buffNumber >= 10 && character.traumaResistance < 5 && character.traumaNumber[0] < 10 && !isAddBuff)
        {
            character.AddBuff("裂隙");
        }
        character.traumaNumber[0] = buffNumber;
        this.timer = 999999;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        attackTimer += Time.deltaTime;
        if (attackTimer > 0.1f)
        {
            if (character != null)
            {
                if (character.unlockedFeathers.Count > 0 && health - character.unlockedFeathers[0].health >= 1)
                {
                    character.unlockedFeathers[0].health -= damage*Mathf.Pow(2, -character.abnormalityResistance / 100) * (1 - character.traumaResistance * 0.1f);
                    attackTimer = 0;
                    if (Random.Range(0, 3) > 1)
                    {
                        this.timer = 0;
                    }
                }
            }
        }
        if (character != null && character.unlockedFeathers.Count > 0)
        {
            health = character.unlockedFeathers[0].health;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class Terrified : EffectBuff
{
    private Player player;
    private Enemy enemy;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Player>() != null)
        {
            player = target.GetComponent<Player>();
        }
        if (target.GetComponent<Enemy>() != null)
        {
            enemy = target.GetComponent<Enemy>();
        }
        this.timer = 1;
    }

    public override void OnEnter()
    {
        if (player != null && player.unlockedFeathers.Count > 0)
        {
            player.unlockedFeathers[0].health -= 1.5f;
        }
        if (enemy != null && enemy.unlockedFeathers.Count > 0)
        {
            enemy.unlockedFeathers[0].health -= 1.5f;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (player != null && player.unlockedFeathers.Count > 0)
        {
            player.unlockedFeathers[0].health += 1.5f;
        }
        if (enemy != null && enemy.unlockedFeathers.Count > 0)
        {
            enemy.unlockedFeathers[0].health += 1.5f;
        }
        base.OnExit();
    }
}
public class Depressed : EffectBuff
{
    private Player player;
    private Enemy enemy;
    private Color baseColor;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Player>() != null)
        {
            player = target.GetComponent<Player>();
        }
        if (target.GetComponent<Enemy>() != null)
        {
            enemy = target.GetComponent<Enemy>();
        }
        this.timer = 12;
    }

    public override void OnEnter()
    {
        if (player != null)
        {
            player.cardGenerateCooldown += 1;
            baseColor = player.cardGenerateText.color;
            player.cardGenerateText.color = Color.red;
        }
        if (enemy != null)
        {
            foreach (EnemyCardLine line in enemy.attackCardLineList)
            {
                foreach (EnemyCardWithTimer ectw in line.cards)
                {
                    ectw.timer += 1;
                }
            }
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (player != null)
        {
            player.cardGenerateCooldown -= 1;
            player.cardGenerateText.color = baseColor;
        }
        if (enemy != null)
        {
            foreach (EnemyCardLine line in enemy.attackCardLineList)
            {
                foreach (EnemyCardWithTimer ectw in line.cards)
                {
                    ectw.timer -= 1;
                }
            }
        }
        base.OnExit();
    }
}
public class Crash : EffectBuff
{
    private Player player;
    private Enemy enemy;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Player>() != null)
        {
            player = target.GetComponent<Player>();
        }
        if (target.GetComponent<Enemy>() != null)
        {
            enemy = target.GetComponent<Enemy>();
        }
        this.timer = 13;
    }

    public override void OnEnter()
    {
        if (player != null)
        {
            player.strength -= player.oriStrength / 10;
            player.tenacity -= player.oriTenacity / 10;
        }
        if (enemy != null && enemy.unlockedFeathers.Count > 0)
        {
            enemy.strength -= enemy.oriStrength / 10;
            enemy.tenacity -= enemy.oriTenacity / 10;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (player != null)
        {
            player.strength += player.oriStrength / 10;
            player.tenacity += player.oriTenacity / 10;
        }
        if (enemy != null && enemy.unlockedFeathers.Count > 0)
        {
            enemy.strength += enemy.oriStrength / 10;
            enemy.tenacity += enemy.oriTenacity / 10;
        }
        base.OnExit();
    }
}
public class Mediocre : EffectBuff
{
    private PlayerCardController playerCardController;
    private Enemy enemy;
    private Color baseColor;
    private bool b = true;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (GameObject.Find("CardPanel") != null)
        {
            playerCardController = GameObject.Find("CardPanel").GetComponent<PlayerCardController>();
        }
        if (target.GetComponent<Enemy>() != null)
        {
            enemy = target.GetComponent<Enemy>();
        }
        this.timer = 60;
        if (GameObject.Find("Dr.Nepriz1 2(Clone)") != null)
        {
            this.timer = 99999999999;
            b = false;
        }
    }

    public override void OnEnter()
    {
        if (playerCardController != null)
        {
            playerCardController.DelPosition();
        }
        if (enemy != null)
        {
            foreach (EnemyCardLine line in enemy.effectCardLineList)
            {
                foreach (EnemyCardWithTimer ectw in line.cards)
                {
                    ectw.timer += 3;
                }
            }
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (b && GameObject.Find("Dr.Nepriz1 2(Clone)") != null)
        {
            this.timer = 99999999999;
            b = false;
        }
    }

    public override void OnExit()
    {
        if (playerCardController != null)
        {
            playerCardController.AddPosition();
        }
        if (enemy != null)
        {
            foreach (EnemyCardLine line in enemy.effectCardLineList)
            {
                foreach (EnemyCardWithTimer ectw in line.cards)
                {
                    ectw.timer -= 3;
                }
            }
        }
        base.OnExit();
    }
}
public class Talent : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 29.7f;
    }

    public override void OnEnter()
    {
        change = character.tenacity * 9;
        character.tenacity *= 10;
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (target.GetComponent<Player>() != null)
        {
            character.tenacity -= change;
        }
        if (target.GetComponent<Enemy>() != null)
        {
            character.tenacity /= 10;
        }
        base.OnExit();
    }
}
public class Talent1 : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 29.7f;
    }

    public override void OnEnter()
    {
        change = character.tenacity * 9;
        character.tenacity *= 10;
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class Erudite : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 29.7f;
    }

    public override void OnEnter()
    {
        change = character.strength;
        character.strength *= 2;
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (target.GetComponent<Player>() != null)
        {
            character.strength -= change;
        }
        if (target.GetComponent<Enemy>() != null)
        {
            character.strength /= 2;
        }
        base.OnExit();
    }
}
public class Erudite1 : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 29.7f;
    }

    public override void OnEnter()
    {
        change = character.strength;
        character.strength *= 2;
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class Ignore : EffectBuff
{
    private Enemy enemy;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        isPermanent = true;
        this.timer = 99999999999;
    }

    public override void OnEnter()
    {
        if (enemy != null)
        {
            foreach (EnemyCardLine line in enemy.effectCardLineList)
            {
                foreach (EnemyCardWithTimer ectw in line.cards)
                {
                    ectw.cooldown += 0.05f;
                }
            }
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class ImperfectWork : EffectBuff
{
    private Nepriz2 nepriz2;
    private Player player;
    private float healthMax;
    private ImperfectWorkUi ui;
    private bool b1, b2, b3, b4;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        isPermanent = true;
        if (GameObject.Find("Dr.Nepriz1 2(Clone)") != null)
        {
            nepriz2 = GameObject.Find("Dr.Nepriz1 2(Clone)").GetComponent<Nepriz2>();
            foreach (var item in nepriz2.feathers)
            {
                healthMax += item.health;
            }
        }
        ui = GameObject.Find("ImperfectWorkUi").GetComponent<ImperfectWorkUi>();
        ui.b = true;
        player = target.GetComponent<Player>();
        this.timer = 99999999999;
        foreach (var buff in player.buffList)
        {
            if (buff.name == "瑕疵之作")
            {
                this.timer = 0;
            }
        }
    }

    public override void OnEnter()
    {

        base.OnEnter();
    }

    public override void OnUpdate()
    {
        if (nepriz2 == null)
        {
            timer = 0;
        }
        else
        {
            float health = 0;
            foreach (var item in nepriz2.feathers)
            {
                health += item.health;
            }
            if (!b1 && health < healthMax * 4 / 5)
            {
                Time.timeScale = 0.0001f;
                Change();
                b1 = true;
                ui.b1 = true;
            }
            if (!b2 && health < healthMax * 3 / 5)
            {
                Time.timeScale = 0.0001f;
                Change();
                b2 = true;
                ui.b2 = true;
            }
            if (!b3 && health < healthMax * 2 / 5)
            {
                Time.timeScale = 0.0001f;
                Change();
                b3 = true;
                ui.b3 = true;
            }
            if (!b4 && health < healthMax * 1 / 5)
            {
                Time.timeScale = 0.0001f;
                Change();
                b4 = true;
                ui.b4 = true;
            }
        }
        base.OnUpdate();
    }

    public override void OnExit()
    {
        ui.b = false;
        base.OnExit();
    }
    private void Change()
    {
        int n = 0;
        foreach (var item in nepriz2.buffList)
        {
            if (item.name == "凡庸")
            {
                item.timer = 0;
                n++;
            }
        }
        for (int i = 0; i < n; i++)
        {
            nepriz2.AddBuff("渴求认可");
        }
    }
}
public class CraveRecognition : EffectBuff
{
    private Enemy enemy;
    private float health;
    private float attackTimer;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Enemy>() != null)
        {
            enemy = target.GetComponent<Enemy>();
            if (enemy.unlockedFeathers.Count > 0)
            {
                health = enemy.unlockedFeathers[0].health;
            }
        }
        this.timer = 999999;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        attackTimer += Time.deltaTime;
        if (attackTimer > 0.1f)
        {
            if (enemy != null)
            {
                if (enemy.unlockedFeathers.Count > 0 && health - enemy.unlockedFeathers[0].health >= 1)
                {
                    enemy.unlockedFeathers[0].health -= 0.5f;
                    attackTimer = 0;
                }
            }
        }
        if (enemy != null && enemy.unlockedFeathers.Count > 0)
        {
            health = enemy.unlockedFeathers[0].health;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class UpLifting_ : EffectBuff
{
    private int attackbodyNewNum, attackbodyOriNum;
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 999999999;
    }

    public override void OnEnter()
    {
        change = character.strength * 0.1f;
        character.strength += change;
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        attackbodyNewNum = target.attackBodyObjList.Count;
        if (attackbodyNewNum > attackbodyOriNum)
        {
            target.attackBodyObjList[attackbodyNewNum - 1].GetComponent<AttackBody>().damage += change;
            timer = 0;
        }
        if (attackbodyNewNum < attackbodyOriNum)
        {
            attackbodyOriNum = attackbodyNewNum;
        }
        base.OnUpdate();
    }

    public override void OnExit()
    {
        character.strength -= change;
        base.OnExit();
    }
}
public class UpLifting : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        change = character.strength * 0.1f;
        if (change > 10)
        {
            change = 10;
        }
        character.strength += change;
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        character.strength -= change;
        base.OnExit();
    }
}
public class Toughness : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        change = character.tenacity * 0.1f;
        if (change > 20)
        {
            change = 20;
        }
        character.tenacity += change;
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        character.tenacity -= change;
        base.OnExit();
    }
}
public class BlazingSpeed : EffectBuff
{
    private Enemy character;
    private PlayerController playerController;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Enemy>() != null)
        {
            character = target.GetComponent<Enemy>();
        }
        if (target.GetComponent<PlayerController>() != null)
        {
            playerController = target.GetComponent<PlayerController>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        if (character != null)
        {
            change = character.runSpeed * 0.1f;
            character.runSpeed += change;
        }
        if (playerController != null)
        {
            change = playerController.walkSpeed * 0.1f;
            playerController.walkSpeed += change;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (character != null)
        {
            character.runSpeed -= change;
        }
        if (playerController != null)
        {
            playerController.walkSpeed -= change;
        }
        base.OnExit();
    }
}
public class Agile : EffectBuff
{
    private Enemy character;
    private PlayerController playerController;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Enemy>() != null)
        {
            character = target.GetComponent<Enemy>();
        }
        if (target.GetComponent<PlayerController>() != null)
        {
            playerController = target.GetComponent<PlayerController>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        if (character != null)
        {
            change = character.jumpSpeed * 0.1f;
            character.jumpSpeed += change;
        }
        if (playerController != null)
        {
            change = playerController.jumpSpeed * 0.1f;
            playerController.jumpSpeed += change;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (character != null)
        {
            character.jumpSpeed -= change;
        }
        if (playerController != null)
        {
            playerController.jumpSpeed -= change;
        }
        base.OnExit();
    }
}
public class Steadfast : EffectBuff
{
    private Player player;
    private Enemy enemy;
    private Color baseColor;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Player>() != null)
        {
            player = target.GetComponent<Player>();
        }
        if (target.GetComponent<Enemy>() != null)
        {
            enemy = target.GetComponent<Enemy>();
        }
        this.timer = 12;
    }

    public override void OnEnter()
    {
        if (player != null)
        {
            player.cardGenerateCooldown -= 1;
            baseColor = player.cardGenerateText.color;
            player.cardGenerateText.color = Color.green;
        }
        if (enemy != null)
        {
            foreach (EnemyCardLine line in enemy.attackCardLineList)
            {
                foreach (EnemyCardWithTimer ectw in line.cards)
                {
                    ectw.timer -= 1;
                }
            }
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (player != null)
        {
            player.cardGenerateCooldown += 1;
            player.cardGenerateText.color = baseColor;
        }
        if (enemy != null)
        {
            foreach (EnemyCardLine line in enemy.attackCardLineList)
            {
                foreach (EnemyCardWithTimer ectw in line.cards)
                {
                    ectw.timer += 1;
                }
            }
        }
        base.OnExit();
    }
}
public class Superb : EffectBuff
{
    private PlayerCardController playerCardController;
    private Enemy enemy;
    private Color baseColor;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Player>() != null)
        {
            playerCardController = GameObject.Find("CardPanel").GetComponent<PlayerCardController>();
        }
        if (target.GetComponent<Enemy>() != null)
        {
            enemy = target.GetComponent<Enemy>();
        }
        this.timer = 60;
    }

    public override void OnEnter()
    {
        if (playerCardController != null)
        {
            playerCardController.AddPosition();
        }
        if (enemy != null)
        {
            foreach (EnemyCardLine line in enemy.effectCardLineList)
            {
                foreach (EnemyCardWithTimer ectw in line.cards)
                {
                    ectw.timer -= 3;
                }
            }
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (playerCardController != null)
        {
            playerCardController.DelPosition();
        }
        if (enemy != null)
        {
            foreach (EnemyCardLine line in enemy.effectCardLineList)
            {
                foreach (EnemyCardWithTimer ectw in line.cards)
                {
                    ectw.timer += 3;
                }
            }
        }
        base.OnExit();
    }
}
public class Lethargic : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        change = character.strength * 0.1f;
        if (change > 10)
        {
            change = 10;
        }
        character.strength -= change;
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        character.strength += change;
        base.OnExit();
    }
}
public class Fragile : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        change = character.tenacity * 0.1f;
        if (change > 20)
        {
            change = 20;
        }
        character.tenacity -= change;
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        character.tenacity += change;
        base.OnExit();
    }
}
public class Sluggish : EffectBuff
{
    private Enemy character;
    private PlayerController playerController;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Enemy>() != null)
        {
            character = target.GetComponent<Enemy>();
        }
        if (target.GetComponent<PlayerController>() != null)
        {
            playerController = target.GetComponent<PlayerController>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        if (character != null)
        {
            change = character.runSpeed * 0.1f;
            character.runSpeed -= change;
        }
        if (playerController != null)
        {
            change = playerController.walkSpeed * 0.1f;
            playerController.walkSpeed -= change;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (character != null)
        {
            character.runSpeed += change;
        }
        if (playerController != null)
        {
            playerController.walkSpeed += change;
        }
        base.OnExit();
    }
}
public class Grave : EffectBuff
{
    private Enemy character;
    private PlayerController playerController;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Enemy>() != null)
        {
            character = target.GetComponent<Enemy>();
        }
        if (target.GetComponent<PlayerController>() != null)
        {
            playerController = target.GetComponent<PlayerController>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        if (character != null)
        {
            change = character.jumpSpeed * 0.1f;
            character.jumpSpeed -= change;
        }
        if (playerController != null)
        {
            change = playerController.jumpSpeed * 0.1f;
            playerController.jumpSpeed -= change;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (character != null)
        {
            character.jumpSpeed += change;
        }
        if (playerController != null)
        {
            playerController.jumpSpeed += change;
        }
        base.OnExit();
    }
}
public class PoisonBuff : EffectBuff
{
    float bufftimer;
    Character character;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        bufftimer = 1;
        this.timer = 5.1f;
        character = target.GetComponent<Character>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (bufftimer > 0)
        {
            bufftimer -= Time.deltaTime;
        }
        else
        {
            if (character.unlockedFeathers.Count > 0)
            {
                character.unlockedFeathers[0].health -= 1f;
                bufftimer = 1;
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class JusticeBuff : EffectBuff
{
    int attackbodyOriNum;
    int attackbodyNewNum;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 9999;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        attackbodyOriNum = target.attackBodyObjList.Count;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        attackbodyNewNum = target.attackBodyObjList.Count;
        if (attackbodyNewNum > attackbodyOriNum)
        {
            target.attackBodyObjList[attackbodyNewNum - 1].GetComponent<AttackBody>().damage *= 2;
            timer = 0;
        }
        if (attackbodyNewNum < attackbodyOriNum)
        {
            attackbodyOriNum = attackbodyNewNum;
        }

    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class Scorch : EffectBuff
{
    private Character character;
    private float attackTimer;
    private int buffNumber;
    private bool isAddBuff;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 6f;
        foreach (var buff in character.buffList)
        {
            if (buff.name == "灼伤")
            {
                buff.timer += 6f;
                buffNumber++;
            }
            if (buff.name == "烈焰")
            {
                isAddBuff = true;
            }
        }
        character.burnNumber[1] = buffNumber;
        if (buffNumber >= 10 && character.burnResistance < 5 && character.burnNumber[0] < 10 && !isAddBuff)
        {
            character.AddBuff("烈焰");
        }
        character.burnNumber[0] = buffNumber;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > 0.1f && character.unlockedFeathers.Count > 0)
        {
            character.unlockedFeathers[0].health -= 0.1f * Mathf.Pow(2, -character.abnormalityResistance / 100) * (1 - character.burnResistance * 0.1f);
            attackTimer = 0;
        }
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class Burn : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 60;
    }

    public override void OnEnter()
    {
        change = character.abnormalityResistance / 2;
        character.abnormalityResistance -= change;
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        character.abnormalityResistance += change;
        base.OnExit();
    }
}
public class Heal1 : EffectBuff
{
    private Character character;
    private float healingTimer;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 12;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (healingTimer > 0.1f)
        {
            if (character.unlockedFeathers.Count > 0)
            {
                character.unlockedFeathers[0].health += 0.2f;
            }
            healingTimer = 0;
        }
        else
        {
            healingTimer += Time.deltaTime;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class Heal2 : EffectBuff
{
    private Character character;
    private float healingTimer;
    private float health;
    private int useNumber;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 30;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (character.unlockedFeathers.Count > 0 && health > character.unlockedFeathers[0].health && useNumber > 0)
        {
            timer = 0;
        }
        if (healingTimer > 0.1f)
        {
            if (character.unlockedFeathers.Count > 0)
            {
                character.unlockedFeathers[0].health += 0.3f;
                health = character.unlockedFeathers[0].health;
                useNumber++;
            }
            healingTimer = 0;
        }
        else
        {
            healingTimer += Time.deltaTime;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class Heal3 : EffectBuff
{
    private Character character;
    private float oriHealth;
    private bool first;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 30;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!first && character.unlockedFeathers.Count > 0)
        {
            first = true;
            oriHealth = character.unlockedFeathers[0].health;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        if (first && character.unlockedFeathers.Count > 0 && character.unlockedFeathers[0].health <= oriHealth)
        {
            character.unlockedFeathers[0].health += oriHealth - character.unlockedFeathers[0].health;
        }
    }
}
public class HunterFeatherEffectBuff : EffectBuff
{
    private Player player;
    private PlayerCardController cardController;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Player>() != null)
        {
            player = target.GetComponent<Player>();
        }
        this.timer = 60;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        cardController = player.cardController;
        int ran = Random.Range(1, 3);
        if (cardController.GetCardOrNot() && ran == 1)
        {
            cardController.GetCard("狂猎之枪");
        }
        else if (cardController.GetCardOrNot() && ran == 2)
        {
            cardController.GetCard("噬血匕首");
        }
        if (player.attackBodyObjList.Count >= 2)
        {
            player.cardGenerateCooldownTimer = 0;
        }
        timer = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class HunterFeatherEffectBuff1 : EffectBuff
{
    private Player player;
    private int num;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Player>() != null)
        {
            player = target.GetComponent<Player>();
        }
        this.timer = 5;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        num = player.attackBodyObjList.Count;
        player.strength += 10 * (num + 1);
        player.tenacity += 10 * (num + 1);

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        player.strength -= 10 * (num + 1);
        player.tenacity -= 10 * (num + 1);
    }
}
public class RandomBuff : EffectBuff
{
    private Character character;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
            int n = Random.Range(0, 30);
            switch (n)
            {
                case 0: character.AddBuff("治愈Ⅲ型"); break;
                case 1: character.AddBuff("治愈Ⅱ型"); break;
                case 2: character.AddBuff("治愈Ⅰ型"); break;
                case 3: character.AddBuff("灼伤"); break;
                case 4: character.AddBuff("正义"); break;
                case 5: character.AddBuff("中毒"); break;
                case 6: character.AddBuff("凝重"); break;
                case 7: character.AddBuff("迟缓"); break;
                case 8: character.AddBuff("脆弱"); break;
                case 9: character.AddBuff("萎靡"); break;
                case 10: character.AddBuff("卓越"); break;
                case 11: character.AddBuff("坚定"); break;
                case 12: character.AddBuff("轻健"); break;
                case 13: character.AddBuff("神速"); break;
                case 14: character.AddBuff("坚韧"); break;
                case 15: character.AddBuff("振奋"); break;
                case 16: character.AddBuff("博学"); break;
                case 17: character.AddBuff("才华"); break;
                case 18: character.AddBuff("凡庸"); break;
                case 19: character.AddBuff("崩溃"); break;
                case 20: character.AddBuff("忧郁"); break;
                case 21: character.AddBuff("惊惶"); break;
                case 22: character.AddBuff("伤痕"); break;
                case 23: character.AddBuff("麻木"); break;
                case 24: character.AddBuff("异常抵抗"); break;
                case 25: character.AddBuff("异常脆弱"); break;
                case 26: character.AddBuff("灼伤抵抗"); break;
                case 27: character.AddBuff("灼伤脆弱"); break;
                case 28: character.AddBuff("伤痕抵抗"); break;
                case 29: character.AddBuff("伤痕脆弱"); break;
            }
        }
        this.timer = 0;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class AbnormalResistance : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        if (character != null)
        {
            change = character.abnormalityResistance * 0.1f;
            character.abnormalityResistance += change;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (character != null)
        {
            character.abnormalityResistance -= change;
        }
        base.OnExit();
    }
}
public class AbnormalFragility : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        if (character != null)
        {
            change = character.abnormalityResistance * 0.1f;
            character.abnormalityResistance -= change;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (character != null)
        {
            character.abnormalityResistance += change;
        }
        base.OnExit();
    }
}
public class BurnResistance : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        if (character != null)
        {
            character.burnResistance++;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (character != null)
        {
            character.burnResistance--;
        }
        base.OnExit();
    }
}
public class BurnFragility : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        if (character != null)
        {
            character.burnResistance--;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (character != null)
        {
            character.burnResistance++;
        }
        base.OnExit();
    }
}
public class TraumaResistance : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        if (character != null)
        {
            character.traumaResistance++;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (character != null)
        {
            character.traumaResistance--;
        }
        base.OnExit();
    }
}
public class TraumaFragility : EffectBuff
{
    private Character character;
    private float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 16f;
    }

    public override void OnEnter()
    {
        if (character != null)
        {
            character.traumaResistance--;
        }
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        if (character != null)
        {
            character.traumaResistance++;
        }
        base.OnExit();
    }
}
public class Fissure : EffectBuff
{
    private Character character;
    private float health, attackTimer, number;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 9999999999;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (character.unlockedFeathers.Count > 0 && health - character.unlockedFeathers[0].health >= 1)
        {
            number++;
        }
        if (number >= 3 && Random.Range(3, 13) < number && character.unlockedFeathers.Count > 0)
        {
            timer = 0;
        }
        if (character.unlockedFeathers.Count > 0)
        {
            health = character.unlockedFeathers[0].health;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        int buffNumber = 0;
        foreach (var buff in character.buffList)
        {
            if (buff.name == "伤痕")
            {
                buffNumber++;
            }
        }
        character.unlockedFeathers[0].health -= buffNumber * number * Mathf.Pow(2, -character.abnormalityResistance / 100);
    }
}
public class BurningDocumentsEffectBuff1 : EffectBuff
{
    private Character character;
    private float health, attackTimer, number;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Character>() != null)
        {
            character = target.GetComponent<Character>();
        }
        this.timer = 999999999999;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        character.abnormalityResistance += 100;
        character.burnResistance += 5;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

public class IronHeartEffectBuff : EffectBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 10;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        target.tenacity += 50;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        target.tenacity -= 50;
    }
}
public class DwarfsFeatherEffectBuff : EffectBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 8;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        target.GetComponent<Player>().cardGenerateCooldown *= 0.8f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        target.GetComponent<Player>().cardGenerateCooldown /= 0.8f;
    }
}
public class ContaminatedCloneEffectBuff : EffectBuff
{
    Enemy enemy;
    float abnormalityResistance;
    float addStrength;
    float addTenacity;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        enemy = target.GetComponent<Enemy>();
        this.abnormalityResistance = enemy.abnormalityResistance;
        this.timer = 10;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        addStrength = Random.Range(-50 + abnormalityResistance, 30 + abnormalityResistance);
        addTenacity = Random.Range(-50 + abnormalityResistance, 30 + abnormalityResistance);
        enemy.strength += addStrength;
        enemy.tenacity += addTenacity;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        enemy.strength -= addStrength;
        enemy.tenacity -= addTenacity;
    }
}
public class ShieldEffectBuff : EffectBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 10;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        target.GetComponent<Player>().shields.Add(new Shield()
        {
            health = 20,
            timer = 10
        });
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class EnslavedDwarfsBrokenEffectBuff : EffectBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 8;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        target.GetComponent<Character>().strength -= 20;

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        target.GetComponent<Character>().strength += 20;
    }
}
public class EnslavedDwarfsBrokenEffectBuff1 : EffectBuff
{
    Character character;
    float bufftimer = 1;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        character = target.GetComponent<Character>();
        this.timer = 8;
    }

    public override void OnEnter()
    {
        base.OnEnter();

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (bufftimer > 0)
        {
            bufftimer -= Time.deltaTime;
        }
        else
        {
            foreach (Feather feather in character.unlockedFeathers)
            {
                feather.health -= 10;
            }
            bufftimer = 1;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class Death : EffectBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        Collider2D[] collider2Ds = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                        new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer));
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            int n1 = 0, n2 = 0, n3 = 0;
            Enemy enemy = collider2Ds[i].GetComponent<Enemy>();
            if (enemy != null)
            {
                for (int j = 0; j < enemy.buffList.Count; j++)
                {
                    if (enemy.buffList[j].name == "灼伤")
                    {
                        n1++;
                    }
                    if (enemy.buffList[j].name == "伤痕")
                    {
                        n2++;
                    }
                    if (enemy.buffList[j].name == "中毒")
                    {
                        n3++;
                    }
                }
            }
            if (n1 >= 10 || n2 >= 10 || n3 >= 5)
            {
                enemy.tenacity -= enemy.tenacity / 5;
                enemy.strength -= enemy.strength / 5;
                for (int j = 0; j < enemy.buffList.Count; j++)
                {
                    enemy.buffList[j].timer = 0;
                }
                break;
            }
        }
        this.timer = 3;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class Relieve : EffectBuff
{
    Character character;
    float change;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        character = target.GetComponent<Character>();
        this.timer = 60;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (character.tenacity > 0)
        {
            change = character.tenacity / 10;
            character.strength += change;
            character.tenacity = 0;
        }
        else
        {
            character.tenacity += 10;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        character.strength -= change;
    }
}
public class Defend : EffectBuff
{
    Character character;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        character = target.GetComponent<Character>();
        float change=0;
        character.shields.Add(new Shield() { health = character.tenacity / 20, timer = 9999999 });
        this.timer = 0;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class Rethink : EffectBuff
{
    Character character;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        character = target.GetComponent<Character>();
        if (character.tenacity < 0)
        {
            character.tenacity = -character.tenacity;
        }
        else
        {
            if(target.GetComponent<Player>() != null)
            {
                 Player player = target.GetComponent<Player>();
                player.GenerateCard();
                player.AddBuff("坚韧");
            }
        }
        this.timer = 0;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class Misunderstanding : EffectBuff
{
    Character character;
    public int grade=1;
    private int buffCount1;
    private int buffCount2;
    private bool b1, b2;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 99999999999;
        character = target.GetComponent<Character>();
        foreach (var buff in character.buffList)
        {
            if (buff.name == "误解")
            {
                ((Misunderstanding)buff).grade++;
                this.timer= 0;   
            }
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (grade > 3)
        {
            grade = 3;
        }
        if (grade >= 1)
        {
            if(character.buffList.Count !=buffCount1)
            {
                if(character.buffList.Count>buffCount1)
                {
                    for(int i = buffCount1;  i < character.buffList.Count; i++)
                    {
                        if (character.buffList[i].name == "伤痕"&& ((Trauma)character.buffList[i]).damage==1)
                        {
                            ((Trauma)character.buffList[i]).damage++;
                        }
                    }
                }
                buffCount1 = character.buffList.Count;
            }
        }
        if(grade >= 2)
        {
            if (!b1)
            {
                character.abnormalityResistance -= 20;
                b1 = true;
            }
            if (character.buffList.Count != buffCount2)
            {
                if (character.buffList.Count > buffCount2)
                {
                    for (int i = buffCount2; i < character.buffList.Count; i++)
                    {
                        if (character.buffList[i].name == "伤痕" && ((Trauma)character.buffList[i]).damage < 3)
                        {
                            ((Trauma)character.buffList[i]).damage++;
                        }
                    }
                }
                buffCount2 = character.buffList.Count;
            }
        }
        if(grade==3)
        {
            if (!b2)
            {
                character.abnormalityResistance -= character.abnormalityResistance / 4;
                character.traumaResistance -= 3;
                b2 = true;
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class DoingThingsInACompletelyWrongOrOppositeWay : EffectBuff
{
    int attackbodyOriNum;
    int attackbodyNewNum;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        this.timer = 9999;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        attackbodyOriNum = target.attackBodyObjList.Count;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        attackbodyNewNum = target.attackBodyObjList.Count;
        if (attackbodyNewNum > attackbodyOriNum)
        {
            target.attackBodyObjList[attackbodyNewNum - 1].GetComponent<AttackBody>().buffNames.Add("伤痕");
            timer = 0;
        }
        if (attackbodyNewNum < attackbodyOriNum)
        {
            attackbodyOriNum = attackbodyNewNum;
        }

    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
public class ExperimentAttackEffectBuff : EffectBuff
{
    Character character;
    Player Player;
    float oriHp;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        character = target.GetComponent<Character>();
        this.timer = 5;
        if (character.feathers.Count != 0)
        {
            oriHp = character.feathers[0].health;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (character.feathers[0].health < oriHp)
        {
            timer = 0;
        }
        if (timer <= 0.5f&&timer>0)
        {
            timer = 0;
            target.gameObject.transform.position = GameObject.Find("Player").transform.position;
        }
        foreach (GameObject attackBody in character.attackBodyObjList)
        {
            if (attackBody.name == "ExperimentPlayerAttackBody1")
            {
                attackBody.GetComponent<AttackBody>().damage += Time.deltaTime;
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
#endregion