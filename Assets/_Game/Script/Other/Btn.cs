using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour
{
    [SerializeField] private Gate targetGate;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Sprite[] sprImg;
    
    public void CloseGate()
    {
        spr.sprite = sprImg[1];
        if (targetGate != null)
        {
            targetGate.CloseGate();
        }
    }

    public void OpenGate()
    {
        spr.sprite = sprImg[0];
        if (targetGate != null)
        {
            targetGate.OpenGate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = Cache.GetEnemy(collision);
        if (enemy != null)
        {
            OpenGate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = Cache.GetEnemy(collision);
        if (enemy != null)
        {
            CloseGate();
        }
    }
}
