using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Gate targetGate;

    public void CloseGate()
    {
        if (targetGate != null)
        {
            targetGate.CloseGate();
        }
    }

    public void OpenGate()
    {
        if (targetGate != null)
        {
            targetGate.OpenGate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Crab enemy = Cache.GetCrab(collision);
        if (enemy != null && enemy.enemyType.enemyType == EEnemy.Octopus)
        {
            OpenGate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Crab enemy = Cache.GetCrab(collision);
        if (enemy != null && enemy.enemyType.enemyType == EEnemy.Octopus)
        {
            CloseGate();
        }
    }
}
