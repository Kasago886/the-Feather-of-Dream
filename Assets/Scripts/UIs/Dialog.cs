using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.U2D;

public class Dialog : MonoBehaviour
{
    //头像
    public Image dialogImage;
    //文字
    public Text dialogText;
    //结束时事件
    public UnityEvent endEvent;

    //Animator
    Animator animator;

    //以行为单位的字符串列表
    string[] dialogList;
    //总行数
    int len;
    //当前行数序号
    int count;
    //是否正在说话
    bool ifsaying;
    //当前已显示文字
    string said;
    //当前需要显示的文字长度
    int saidlen;
    //计时器
    float timer;
    //下一个字的时间
    float nextTime;
    //当前行的总有效文字（已显示+未显示）
    string sayText;
    //暂停播放
    bool ifPause;

    //设置报错信息，便于检查
    string wrongtext;
    string information;
    string textFile;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        //初始化
        ifsaying = false;
        said = "";
        saidlen = 1;
        timer = 0;
        nextTime = 0.05f;
        sayText = "";
        ifPause = true;

        //初始化错误信息
        wrongtext = "Error";
        information = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!ifPause)
        {
            //按下F进行对话
            if (Input.GetKeyDown(KeyCode.F)) 
            {
                dialogFare();
            }

            //计时器
            timer += Time.deltaTime;

            //每隔nextTime秒就显示下一个字
            if (ifsaying && (timer >= nextTime))
            {
                //未显示完成时，显示下一个字
                if (saidlen <= sayText.Length)
                {
                    said = sayText.Substring(0, saidlen);
                    dialogText.text = said;

                    saidlen += 1;
                    timer = 0;
                }
                //显示完成时，准备读取下一行
                else
                {
                    ifsaying = false;
                    saidlen = 1;
                    timer = 0;
                    count += 1;
                }
            }
            //显示完成但等待中，等待5秒自动读取下一行
            if (!ifsaying && (timer >= 5f))
            {
                timer = 0;
                dialogFare();
            }
        }
        
    }

    //更新对话进度
    void dialogFare()
    {
        if (count < len)
        {
            //Debug.Log("count:"+count);

            //按顺序读取dialoglist
            //当前行未格式化文本
            string dialog = dialogList[count];

            //Debug.Log("dialog:" + dialog);

            //错误信息
            wrongtext = "DialogText '" + textFile + "' is wrong! On the " + (count + 1) + " line.";
            information = "\nInformation:\"" + dialog + "\".";

            //不为空时
            if (!(dialog.Length <= 0))
            {
                //按"`"分割字符串为ImageName和Text
                char[] sep = { '`' };
                string[] dialogL = dialog.Split(sep);
                //Debug.Log(dialogL);
                //Debug.Log(dialogL.Length);

                //判断格式是否正确
                bool ifcanread = true;
                switch (dialogL.Length)
                {
                    case 0:
                        ifcanread = false;
                        Debug.LogError(wrongtext + ": How? It's an impossible error!" + information);
                        break;

                    case 1:
                        ifcanread = false;
                        Debug.LogError(wrongtext + ": Need at least and only one \"`\" in the dialogText. But there exist 0!" + information);
                        break;

                    case 2:
                        break;

                    default:
                        ifcanread = false;
                        Debug.LogError(wrongtext + ": Please ensure there is only one \"`\" in the dialogText. There exist "+ (dialogL.Length-1).ToString() + "!" + information);
                        break;
                }

                //显示dialog
                if (ifcanread)
                {
                    string imageName = dialogL[0];
                    sayText = dialogL[1];

                    //设置头像
                    if (imageName.Length > 0)
                    {
                        if (imageName == "[clear]")
                        {
                            Sprite sprite = Resources.Load("Images/image_null", typeof(Sprite)) as Sprite;
                            dialogImage.overrideSprite = sprite;
                            Resources.UnloadUnusedAssets();

                            //Debug.Log(sprite);
                            if (sprite == null)
                            {
                                Debug.LogWarning("You just used \"[clear]\" while there's no needed sprite.\n" +
                                    "It's highly recommended that there be a transparent image named \"image_null\" in \"Asset/Resources/Images\".\n" +
                                    information);
                            }
                        }
                        else
                        {
                            Sprite sprite = Resources.Load("Images/" + imageName, typeof(Sprite)) as Sprite;
                            dialogImage.overrideSprite = sprite;
                            Resources.UnloadUnusedAssets();
                        }
                    }

                    //如果正在说，则直接显示完当前行
                    if (ifsaying)
                    {
                        //直接显示
                        dialogText.text = sayText;

                        ifsaying = false;
                        saidlen = 1;
                        timer = 0;
                    }
                    //已读完时，读取下一行
                    else
                    {
                        //清空当前行
                        dialogText.text = "";

                        ifsaying = true;
                        timer = 0;
                    }
                }

                //没有“正在说话”时准备读取下一条
                if (!ifsaying)
                {
                    count = count + 1;
                }

            }
            //为空时读取下一行
            else
            {
                count += 1;
                dialogFare();
            }

        }
        //结束
        else
        {
            //Debug.Log("NormalMode End!\ncount:"+count.ToString()+"\nlen:"+len.ToString());

            dialogEnd();
        }
    }

    public void Read(string TextFile)
    {
        try
        {
            //获取文本
            TextAsset textAsset = Resources.Load<TextAsset>("Texts/" + TextFile);
            //以换行符格式化文本
            dialogList = textAsset.text.Replace("\r", "").Split('\n');
            //释放Resource
            Resources.UnloadUnusedAssets();
        }
        catch
        {
            wrongtext = "DialogText '" + textFile + "' doesn't exist!";
            Debug.LogError(wrongtext);
        }

        //动画
        animator.SetBool("appear", true);

        //初始化
        ifsaying = false;
        said = "";
        saidlen = 1;
        timer = 0;
        sayText = "";
        len = dialogList.Length;
        count = 0;
        //报错信息记录
        textFile = TextFile;

        //开始更新
        ifPause = false;
        dialogFare();
    }

    void dialogEnd()
    {
        //动画
        animator.SetBool("appear", false);

        ifPause = true;

        //对话结束
        endEvent?.Invoke();
    }

    public void Clear()
    {
        Sprite sprite = Resources.Load("Images/image_null", typeof(Sprite)) as Sprite;
        dialogImage.overrideSprite = sprite;
        Resources.UnloadUnusedAssets();

        if (sprite == null)
        {
            Debug.LogWarning("You just used the function \"Clear()\" while there's no needed sprite.\n" +
                "It's highly recommended that there be a transparent image named \"image_null\" in \"Asset/Resources/Images\".");
        }

        dialogText.text = "";
    }
}
