using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

#region 装备buff
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
            enemy.strength += oriStrength / 10;
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
    private Player player;
    private Enemy enemy;
    private float health;
    private float attackTimer;
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);
        if (target.GetComponent<Player>() != null )
        {
            player = target.GetComponent<Player>();
            if (player.unlockedFeathers.Count > 0)
            {
                health = player.unlockedFeathers[0].health;
            }
        }
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
            if (player != null)
            {
                if ( player.unlockedFeathers.Count > 0&&health - player.unlockedFeathers[0].health >= 1)
                {
                    player.unlockedFeathers[0].health--;
                    attackTimer = 0;
                    if (Random.Range(0, 3) > 1)
                    {
                        this.timer = 0;
                    }
                }
            }
            if (enemy != null)
            {
                if (enemy.unlockedFeathers.Count > 0&&health - enemy.unlockedFeathers[0].health >= 1)
                {
                    enemy.unlockedFeathers[0].health--;
                    attackTimer = 0;
                    if (Random.Range(0, 3) > 1)
                    {
                        this.timer=0;
                    }
                }
            }
        }
        if(player != null && player.unlockedFeathers.Count > 0)
        {
            health = player.unlockedFeathers[0].health;
        }
        if(enemy != null && enemy.unlockedFeathers.Count > 0)
        {
            health = enemy.unlockedFeathers[0].health;
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
        this.timer = 0;
    }

    public override void OnEnter()
    {
        if (player != null && player.unlockedFeathers.Count > 0)
        {
            player.unlockedFeathers[0].health -= 1f;
        }
        if (enemy != null && enemy.unlockedFeathers.Count > 0)
        {
            enemy.unlockedFeathers[0].health -= 1f;
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
            player.unlockedFeathers[0].health += 1f;
        }
        if (enemy != null && enemy.unlockedFeathers.Count > 0)
        {
            enemy.unlockedFeathers[0].health += 1f;
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
        if(player != null)
        {
            player.cardGenerateCooldown += 1;
            baseColor=player.cardGenerateText.color;
            player.cardGenerateText.color= Color.red;
        }
        if (enemy != null)
        {
            foreach (EnemyCardLine line in enemy.attackCardLineList)
            {
                foreach(EnemyCardWithTimer ectw in line.cards)
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
                    ectw.timer += 1;
                }
            }
        }
        base.OnExit();
    }
}
#endregion