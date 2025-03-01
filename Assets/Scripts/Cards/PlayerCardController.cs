using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCardController : MonoBehaviour
{
    //[Header("父物体")]
    //public GameObject content;
    //[Header("全体卡牌预制体")]
    //public GameObject[] cardsList;
    //private List<GameObject> activeCards;
    //private List<int> activeCardsId,nextActiveCardId;
    //[Header("卡牌生成的位置")]
    //public RectTransform[] positions;
    //private Dictionary<string, int> nameToID;
    //private Dictionary<int, ObjectPool<Card>> idToCard;
    //private float timer,useNumber,timerUseNumber;
    //private int slotNumber;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    slotNumber = 5;
    //    nameToID = new Dictionary<string, int>();
    //    idToCard = new Dictionary<int, ObjectPool<Card>>();
    //    activeCards = new List<GameObject>();
    //    activeCardsId = new List<int>();
    //    nextActiveCardId = new List<int>();
    //    for (int i = 0; i < cardsList.Length; i++)
    //    {
    //        ObjectPool<Card> cardPool;
    //        cardPool = new ObjectPool<Card>(cardsList[i], positions);
    //        Card card = cardPool.GetFromPool();
    //        nameToID.Add(card.name,card.id);
    //        Debug.Log("Add"+card.name + card.id+"!!!");
    //        idToCard.Add(card.id, cardPool);
    //        cardPool.ReturnToPool(card);
    //    }
    //    InvokeRepeating("ReduceCard", 1.0f, 1.0f);
    //}
    ///// <summary>
    ///// 每秒自动检测是否少牌
    ///// </summary>
    //private void Update()
    //{
    //}
    ///// <summary>
    ///// 调用本函数增加玩家手牌
    ///// </summary>
    ///// <param name="id">
    ///// 想要增加的手牌的id
    ///// </param>
    //public void GetCard(int id)
    //{
    //    if(activeCards.Count<positions.Length)
    //    {
    //        activeCards.Add(idToCard[id].GetFromPool(positions[activeCards.Count]).gameObject);
    //        activeCardsId.Add(id);
    //    }
    //}
    ///// <summary>
    ///// 调用本函数增加玩家手牌
    ///// </summary>
    ///// <param name="cardName">
    ///// 想要增加的手牌的名称
    ///// </param>
    //public void GetCard(string cardName)
    //{
    //    if (nameToID.ContainsKey(cardName))
    //    {
    //        GetCard(nameToID[cardName]);
    //    }
    //}
    //private void GetCard(int id,bool b)
    //{
    //    if (activeCards.Count < positions.Length)
    //    {
    //        activeCards.Add(idToCard[id].GetFromPool(positions[activeCards.Count]).gameObject);
    //    }
    //}
    ///// <summary>
    ///// 少牌重新排序
    ///// </summary>
    //private void ReduceCard()
    //{
    //    bool clear=false;
    //    for(int i = 0;i < activeCards.Count;i++)
    //    {
    //        if(!activeCards[i].activeSelf)
    //        {
    //            clear=true;
    //            idToCard[activeCardsId[i]].ReturnToPool(activeCards[i].GetComponent<Card>());
    //            activeCardsId.RemoveAt(i);
    //            activeCards.RemoveAt(i);
    //            i--;
    //        }
    //    }
    //    //Debug.Log("Clear=" + clear);
    //    if (clear)
    //    {
    //        string names = null;
    //        useNumber++;
    //        for (int i = 0; i < activeCards.Count; i++)
    //        {
    //            if (activeCardsId[i] == 1)
    //            {
    //                names += "拔羽|";
    //            }
    //            else
    //            {
    //                names += "攻击|";
    //            }
    //            idToCard[activeCardsId[i]].ReturnToPool(activeCards[i].GetComponent<Card>());
    //        }
    //        activeCards.Clear();
    //        string ids=null;
    //        foreach (int id in activeCardsId)
    //        {
    //            if (id == 1)
    //            {
    //                ids +=  "拔羽|";
    //            }
    //            else
    //            {
    //                ids +=  "攻击|";
    //            }
    //            GetCard(id, true);
    //        }
    //    }
    //}
    ///// <summary>
    ///// 调用本函数以减少玩家手牌
    ///// </summary>
    ///// <param name="id">
    ///// 想要减少的卡牌的id
    ///// </param>
    ///// <param name="reduceNumber">
    ///// 想要减少的卡牌数量
    ///// </param>
    //public void ReduceCard(int id, int reduceNumber = 1)
    //{
    //    if (reduceNumber > 1)
    //    {
    //        List<int> n = new List<int>();
    //        for (int i = 0; i < activeCardsId.Count; i++)
    //        {
    //            if (id == activeCardsId[i])
    //            {
    //                n.Add(i);
    //            }
    //        }
    //        if (reduceNumber > n.Count)
    //        {
    //            reduceNumber = n.Count;
    //        }
    //        for (int i = 0; i < reduceNumber; i++)
    //        {
    //            activeCards[n[i]].SetActive(false);
    //        }
    //        ReduceCard();
    //    }
    //    else
    //    {
    //        for (int i = 0; i < activeCardsId.Count; i++)
    //        {
    //            if (id == activeCardsId[i])
    //            {
    //                activeCards[i].SetActive(false);
    //                ReduceCard();
    //                return;
    //            }
    //        }
    //    }
    //}
    ///// <summary>
    ///// 调用本函数以减少玩家手牌
    ///// </summary>
    ///// <param name="name">
    ///// 想要减少的卡牌的名字
    ///// </param>
    ///// <param name="reduceNumber">
    ///// 想要减少的卡牌数量
    ///// </param>
    //public void ReduceCard(string name, int reduceNumber = 1)
    //{
    //    ReduceCard(nameToID[name], reduceNumber);
    //}
    ///// <summary>
    ///// 调用本函数清空玩家手牌
    ///// </summary>
    //public void ClearCard()
    //{
    //    for (int i = 0; i < activeCards.Count; i++)
    //    {
    //        idToCard[activeCardsId[i]].ReturnToPool(activeCards[i].GetComponent<Card>());
    //    }
    //    activeCards.Clear();
    //    activeCardsId.Clear();
    //}
    //public bool GetCardOrNot()
    //{
    //    if (activeCardsId.Count < positions.Length)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    ///// <summary>
    ///// 增加卡槽
    ///// </summary>
    //public void AddPosition()
    //{
    //    slotNumber++;
    //    GameObject newSlot=new GameObject("Slot"+slotNumber);
    //    newSlot.AddComponent<RectTransform>();
    //    newSlot.transform.SetParent(content.transform);
    //    positions[positions.Length]=newSlot.GetComponent<RectTransform>();
    //    for (int i = 0; i < cardsList.Length; i++)
    //    {
    //        ObjectPool<Card> cardPool;
    //        cardPool = new ObjectPool<Card>(cardsList[i], positions[positions.Length]);
    //        Card card = cardPool.GetFromPool();
    //        nameToID.Add(card.name, card.id);
    //        idToCard.Add(card.id, cardPool);
    //        cardPool.ReturnToPool(card);
    //    }
    //}
    [Header("父物体")]
    public GameObject content;
    [Header("全体卡牌预制体")]
    public GameObject[] cardsList;
    [Header("初始生成数量")]
    public int positionNumber;
    public static bool cantUseCard;
    [HideInInspector]
    public int remainingSlotCount { get; private set; }
    private Dictionary<string, int> nameToID = new();
    private Dictionary<int, GameObject> idToCard = new();
    private RectTransform rectTransform;
    private Text slotNumberText;
    private void Start()
    {
        rectTransform = content.GetComponent<RectTransform>();
        slotNumberText = GameObject.Find("slotNumberText").GetComponent<Text>();
        for (int i = 0; i < cardsList.Length; i++)
        {
            GameObject g = Instantiate(cardsList[i]);
            Card c = g.GetComponent<Card>();
            idToCard.Add(c.id, cardsList[i]);
            nameToID.Add(c.name, c.id);
            Destroy(g);
        }
    }
    private void Update()
    {
        if (positionNumber < 1)
        {
            positionNumber = 1;
        }
        remainingSlotCount = positionNumber - content.transform.childCount;
        slotNumberText.text = positionNumber.ToString();
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    if (FindAnyObjectByType<Nepriz2>() != null)
        //    {
        //            FindAnyObjectByType<Nepriz2>().AddBuff("伤痕");
        //            FindAnyObjectByType<Nepriz2>().AddBuff("拔羽20s");
        //    }
        //    FindAnyObjectByType<Player>().tenacity += 99999;
        //    FindAnyObjectByType<Player>().strength += 10;
        //    FindAnyObjectByType<Player>().shields.Add(new Shield() { health = 99999999,timer=999999999 });
        //    FindAnyObjectByType<Player>().AddBuff("艾莉之剑");
        //}
    }

    /// <summary>
    /// 获取稀有度
    /// </summary>
    /// <param name="cardName"></param>
    /// <returns></returns>
    public int GetRarity(string cardName)
    {
        Card card = idToCard[nameToID[cardName]].GetComponent<Card>();
        int rarity = card.rarity;
        return rarity;
    }
    /// <summary>
    /// 调用本函数增加玩家手牌
    /// </summary>
    /// <param name="cardName"></param>
    /// <returns>
    /// 返还是否成功添加
    /// </returns>
    public bool GetCard(string cardName, bool b)
    {
        if (nameToID.ContainsKey(cardName))
        {
            return GetCard(nameToID[cardName], true);
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 调用本函数增加玩家手牌
    /// </summary>
    /// <param name="id"></param>
    /// <param name="b"></param>
    /// <returns>
    /// 返还是否成功添加
    /// </returns>
    public bool GetCard(int id, bool b)
    {
        if (content.transform.childCount < positionNumber && idToCard.ContainsKey(id))
        {
            RectTransform rectTransform1 = Instantiate(idToCard[id], content.transform).GetComponent<RectTransform>();
            rectTransform1.SetParent(rectTransform);
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 调用本函数增加玩家手牌
    /// </summary>
    /// <param name="cardName">
    /// 想要增加的手牌的名称
    /// </param>
    public void GetCard(string cardName)
    {
        if (nameToID.ContainsKey(cardName))
        {
            GetCard(nameToID[cardName]);
        }
    }
    /// <summary>
    /// 调用本函数增加玩家手牌
    /// </summary>
    /// <param name="id">
    /// 想要增加的手牌的id
    /// </param>
    public void GetCard(int id)
    {
        if (content.transform.childCount < positionNumber && idToCard.ContainsKey(id))
        {
            RectTransform rectTransform1 = Instantiate(idToCard[id], content.transform).GetComponent<RectTransform>();
            rectTransform1.SetParent(rectTransform);
        }
    }
    /// <summary>
    /// 增加卡槽
    /// </summary>
    public void AddPosition(int i = 1)
    {
        positionNumber += i;
    }
    /// <summary>
    /// 减少卡槽
    /// </summary>
    /// <param name="i">
    /// 想要减少的数目
    /// </param>
    public void DelPosition(int i = 1)
    {
        int DEBUG_WHILE_COUNT = 0;

        positionNumber -= i;
        while (content.transform.childCount > positionNumber && content.transform.childCount > 0)
        {
            DEBUG_WHILE_COUNT++;
            if (DEBUG_WHILE_COUNT > 1000)
            {
                Debug.LogError("WHILE OVERUSED! YOU SHOULD CHECK IT!");
                break;
            }
            DestroyImmediate(content.transform.GetChild(content.transform.childCount - 1).gameObject);
        }
    }
    /// 调用本函数清空玩家手牌
    /// </summary>
    public void ClearCard()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 判断是否可以Add卡牌
    /// </summary>
    /// <returns></returns>
    public bool GetCardOrNot()
    {
        if (cantUseCard && content.transform.childCount < positionNumber - 1)
        {
            return true;
        }
        else if (content.transform.childCount < positionNumber && !cantUseCard)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 调用本函数以减少玩家手牌
    /// </summary>
    /// <param name="id">
    /// 想要减少的卡牌的id
    /// </param>
    /// <param name="reduceNumber">
    /// 想要减少的卡牌数量
    /// </param>
    public void ReduceCard(int id, int reduceNumber = 1)
    {
        int n = 0;
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if (content.transform.GetChild(i).GetComponent<Card>().id == id && n < reduceNumber)
            {
                n++;
                Destroy(content.transform.GetChild(i));
            }
        }
    }
    /// <summary>
    /// 调用本函数以减少玩家手牌
    /// </summary>
    /// <param name="name">
    /// 想要减少的卡牌的名字
    /// </param>
    /// <param name="reduceNumber">
    /// 想要减少的卡牌数量
    /// </param>
    public void ReduceCard(string name, int reduceNumber = 1)
    {
        ReduceCard(nameToID[name], reduceNumber);
    }
    /// <summary>
    /// 调用本函数查询玩家手上持有某id的卡的数量
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetCardNumber(int id)
    {
        int n = 0;
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Card card = content.transform.GetChild(i).GetComponent<Card>();
            if (card.id == id)
            {
                n++;
            }
        }
        return n;
    }
    /// <summary>
    /// 调用本函数查询玩家手上持有某名字的卡的数量
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int GetCardNumber(string name)
    {
        return GetCardNumber(nameToID[name]);
    }
}
