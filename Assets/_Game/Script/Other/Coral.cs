using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coral : MonoBehaviour
{
    [SerializeField] private BoxCollider2D box;
    [SerializeField] private Animator anim;

    public void DeActiveBox()
    {
        box.enabled = false;
        anim.Play(CacheString.TAG_NOBUBBLE);
    }

    public void ActiveBox()
    {
        box.enabled = true;
        anim.Play(CacheString.TAG_IDLECORAL);
    }
}
