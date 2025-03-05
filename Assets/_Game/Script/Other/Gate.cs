using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private GameObject eff;
    [SerializeField] private SpriteRenderer spr;
    
    private bool isOpen;
    public void OpenGate()
    {
        isOpen = true;
        spr.enabled = false; // Ẩn cửa khi mở
        if (eff != null )
        {
            eff.SetActive(true);
        }
    }

    public void CloseGate()
    {
        isOpen = false;
        spr.enabled = true; // Hiện cửa khi đóng
        if (eff != null)
        {
            eff.SetActive(false);
        }
    }

    public bool IsBlocked()
    {
        return !isOpen; // Nếu chưa mở thì bị chặn
    }
}
