using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LarnternFish fish = Cache.GetLarnternFish(collision);
        if (fish != null)
        {
            Debug.Log("Ded");
            fish.Die();
        }
    }
}
