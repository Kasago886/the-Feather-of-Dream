using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ImageSpriteSyner : MonoBehaviour
{
    public SpriteRenderer targetSpriteRenderer;
    public Image targetImage;

    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        if (image != null )
        {
            image.preserveAspect = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (image != null)
        {
            if (targetSpriteRenderer != null)
            {
                image.sprite = targetSpriteRenderer.sprite;
            }
            else if (targetImage != null)
            {
                image.sprite = targetImage.sprite;
            }
        }
    }
}
