using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float duration;
    [SerializeField] private bool shouldLoop;
    [SerializeField] private bool freezAtLastFrame;

    private int spriteIndex;
    private float timer = 0;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if ((timer += Time.deltaTime) >= (duration / sprites.Length))
        {
            timer = 0;

            spriteIndex = (spriteIndex + 1) % sprites.Length;
            image.sprite = sprites[spriteIndex];
            
            if(freezAtLastFrame && spriteIndex == sprites.Length - 1)
            {
                this.enabled = false;
            }
        }
    }
}
