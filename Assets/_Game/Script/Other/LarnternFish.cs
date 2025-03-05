using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarnternFish : MonoBehaviour
{
    [Header("-----Other-----")]
    [SerializeField] private SpriteRenderer model;

    [Header("-----Components-----")]
    [SerializeField] private Animator anim;

    [Header("-----Movement Settings-----")]
    [SerializeField] private int forwardSteps = 2;  // Số bước tiến
    [SerializeField] private int backwardSteps = 1; // Số bước lùi
    [SerializeField] private float stepSize = 1f;   // Khoảng cách mỗi bước

    private Vector3 startPos;
    private int currentStep = 0;
    private bool isMovingForward = true;

    private void Start()
    {
        startPos = transform.position;
    }

    public void Stun()
    {
        anim.Play(CacheString.TAG_STUN);
    }

    public void Move()
    {
        anim.Play(CacheString.TAG_IDLEFISH);

        if (isMovingForward)
        {
            if (currentStep < forwardSteps)
            {
                transform.position += new Vector3(stepSize, 0, 0);
                currentStep++;
            }
            else
            {
                isMovingForward = false;
            }
        }
        else
        {
            if (currentStep > -backwardSteps)
            {
                transform.position -= new Vector3(stepSize, 0, 0);
                currentStep--;
            }
            else
            {
                isMovingForward = true;
            }
        }

        FlipSprite(isMovingForward);
    }

    private void FlipSprite(bool isMovingRight)
    {
        model.flipX = !isMovingRight;
    }
}