using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoDestory : MonoBehaviour
{
    public float timer = 1;
    public bool noTransition = false;

    SpriteRenderer spriteRenderer;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        if (!noTransition)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            image = GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (!noTransition)
        {
            if (timer < 1)
            {
                try
                {
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, timer);
                }
                catch
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, timer);
                }
            }
        }

        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }
}
