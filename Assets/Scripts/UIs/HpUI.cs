using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    public float testTime;
    public float testHp;
    public float testHpMax;
    public float delHpSpeed = 1;//延迟血条减少速度
    public float delHpTime = 1;//延时血条减少时间间隔
    private float delHPtime = 1;
    private Transform hpBox;//血条图片
    private Transform delhpBox;//延迟血条图片
    public Text timeText;
    // Start is called before the first frame update
    private void Start()
    {
        hpBox = transform.GetChild(2);
        delhpBox = transform.GetChild(1);
    }

    // Update is called once per frame
    private void Update()
    {
        check();
        Hp_hide();
    }
    private void Hp_hide()
    {
        if (testTime > 0)
        {
            foreach (Transform image in gameObject.transform)
            {
                if (image.GetComponent<Image>() != null)
                {
                    Image _image = image.GetComponent<Image>();
                    image.GetComponent<Image>().color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
                }
                if (image.GetComponent<Text>() != null)
                {
                    Text _image = image.GetComponent<Text>();
                    image.GetComponent<Text>().color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
                }
                gameObject.SetActive(true);
                testTime -= Time.deltaTime;
            }
        }
        if (testTime <= 0)
        {
            foreach (Transform image in gameObject.transform)
            {
                if (image.GetComponent<Image>() != null)
                {
                    Image _image = image.GetComponent<Image>();
                    float alpha = Mathf.Max(0, _image.color.a - Time.deltaTime);
                    image.GetComponent<Image>().color = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
                    if (_image.GetComponent<Image>().color.a <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
                if (image.GetComponent<Text>() != null)
                {
                    Text _image = image.GetComponent<Text>();
                    float alpha = Mathf.Max(0, _image.color.a - Time.deltaTime);
                    image.GetComponent<Text>().color = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
                }
            }
        }
    }

    //检查血量变化控制血条
    private void check()
    {

        float x = testHp / testHpMax;
        timeText.text = ((int)(testTime)).ToString();
        if (delhpBox.localScale.x > x)
        {
            hpBox.localScale = new Vector3(x, 1, 1);
        }
        if ((delhpBox.localScale.x > x) && delHPtime == 1)//延迟血条与血条有数差
        {

            //延迟血条开启协程
            StartCoroutine(delHP(x));
            delHPtime = 0;

        }


        if (hpBox.localScale.x < x)
        {
            hpBox.localScale = new Vector3(x, 1, 1);
            delhpBox.localScale = new Vector3(x, 1, 1);
        }
    }
    IEnumerator delHP(float x)
    {
        yield return new WaitForSeconds(0.3f);//起初停顿时间

        while (delhpBox.localScale.x >= x)//直到延迟血条完成
        {

            yield return new WaitForSeconds(0.005f * delHpTime);//间隔0.005*delHpTime秒
            delhpBox.localScale = new Vector3(delhpBox.localScale.x - 0.001f * delHpSpeed, 1, 1);
        }
        delHPtime = 1;

    }

}