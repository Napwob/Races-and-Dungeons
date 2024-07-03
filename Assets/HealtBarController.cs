using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] Sprite empty;
    [SerializeField] Sprite half;
    [SerializeField] Sprite full;

    private int healthValue;
    private SpriteRenderer[] heartRenderers;

    void Awake()
    {
        heartRenderers = new SpriteRenderer[10];
        for (int i = 0; i < 10; i++)
        {
            GameObject heartObject = new GameObject("Heart" + i);
            heartObject.transform.parent = transform;
            heartObject.transform.localPosition = new Vector3(-0.2f + 0.9f * i, 0, 0);

            SpriteRenderer heartRenderer = heartObject.AddComponent<SpriteRenderer>();
            heartRenderer.sprite = full;
            heartRenderer.sortingOrder = 5;

            heartRenderers[i] = heartRenderer;
        }
    }

    public void updateHealthBar(int value)
    {
        if (value < 0 || value > 20)
            return;

        for (int i = 0; i < heartRenderers.Length; i++)
        {
            if (value - 2 >= 0)
            {
                heartRenderers[i].sprite = full;
                value -= 2;
            }
            else if (value - 1 == 0)
            {
                heartRenderers[i].sprite = half;
                value--;
            }
            else
            {
                heartRenderers[i].sprite = empty;
            }
        }
    }
}