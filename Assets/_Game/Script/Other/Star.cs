using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private BoxCollider2D box;
 
    public void Eat()
    {
        anim.Play(CacheString.TAG_EATSTAR);
        box.enabled = false;
    }
}
