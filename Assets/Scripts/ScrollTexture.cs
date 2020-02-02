using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollSpeed = 10.0f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 offset = spriteRenderer.material.mainTextureOffset;
        offset.x += scrollSpeed *Time.deltaTime;
        spriteRenderer.material.mainTextureOffset = offset;
    }
}
