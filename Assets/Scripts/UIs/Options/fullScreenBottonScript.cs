using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fullScreenBottonScript : MonoBehaviour
{
    public Text text;
    //public AudioSource music;

    float timer = 0.5f;
    bool ifTextChange = false;

    // Start is called before the first frame update
    void Start()
    {
        if (text == null)
        {
            text = GetComponentInChildren<Text>();
        }

        //是否为移动端
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            text.text = "全屏（移动端无效）";
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
            //初始化检测是否全屏
            if (Screen.fullScreen)
            {
                text.text = "全屏：开";
            }
            else
            {
                text.text = "全屏：关";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //字体乱码后计时器0.5s后激活
        if (ifTextChange)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                //恢复字体并停止声音
                if (Screen.fullScreen)
                {
                    text.GetComponent<Text>().text = "全屏：开";
                }
                else
                {
                    text.GetComponent<Text>().text = "全屏：关";
                }
                music.Stop();
                timer = 0.5f;
                ifTextChange = false;
            }
        }
        */

        //切换全屏后计时器0.2s后恢复
        if (ifTextChange)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {   
                timer = 0.2f;
                ifTextChange = false;
            }
            //切换状态文字
            if (Screen.fullScreen)
            {
                text.text = "全屏：开";
            }
            else
            {
                text.text = "全屏：关";
            }
        }
    }

    public void setFullScreen()
    {
        //防止连续点击
        if (!ifTextChange)
        {
            timer = 0.5f;
            ifTextChange = true;

            bool ifFull = !Screen.fullScreen;

            //获取分辨率
            Resolution[] resolutions = Screen.resolutions;

            //设置分辨率并全屏
            if (ifFull)
            {
                //Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
                Screen.SetResolution(1920, 1080, true);
            }
            else
            {
                Screen.SetResolution(1920, 1080, false);
            }
            Screen.fullScreen = ifFull;

            //字体乱码
            //text.GetComponent<Text>().text = "全屏：&";

            //音效
            //music.Play();
        }
    }
}
