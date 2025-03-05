using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarHolder : MonoBehaviour
{
    private bool flag;

    private void Start()
    {
        if (transform.childCount != 0)
            return;

        Debug.LogError("Missing Carrots");
    }

    private void Update()
    {
        if (transform.childCount == 0 && !flag && !LevelManager.Ins.level.player.isDed)
        {
            Win();
            flag = true;
        }
    }

    private void Win()
    {
        Debug.Log("Win");
        UIManager.Ins.CloseUI<MainCanvas>();
        LevelManager.Ins.isWin = true;
        UIManager.Ins.OpenUI<WinCanvas>();
    }
}
