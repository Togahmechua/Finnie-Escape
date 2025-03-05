using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : Controller
{
    public float distancePerGrid = 1f;
    public bool isDed;

    [Header("-----Components-----")]
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Sprite[] sprFish;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform bubbleHolder;

    [Header("-----List-----")]
    [SerializeField] private List<GameObject> obstacleList = new List<GameObject>();
    [SerializeField] private List<GameObject> allowedObjectsWhenSmall = new List<GameObject>();
    [SerializeField] private List<PushAbleGameObj> pushAbleList = new List<PushAbleGameObj>();

    private GameObject[] bb = new GameObject[5];
    private bool isReadyToMove = true;
    private bool usePushAbleObjects;
    private int count;

    private Coral localCoral;
    

    void Start()
    {
        for (int i = 0; i < bubbleHolder.childCount; i++)
        {
            bb[i] = bubbleHolder.GetChild(i).gameObject;
            bb[i].SetActive(false);
        }

        LoadObjList(LevelManager.Ins.level.GameObjList(), LevelManager.Ins.level.AllowedObjList(), LevelManager.Ins.level.PushAbleGameObjList());
    }

    void Update()
    {
        if (isDed || LevelManager.Ins.isWin)
            return;

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput.Normalize();

        Vector2 newMoveInput = moveInput * new Vector2(distancePerGrid, distancePerGrid);

        if (newMoveInput.sqrMagnitude > 0.5)
        {
            if (isReadyToMove)
            {
                isReadyToMove = false;
                Move(newMoveInput);
            }
        }
        else
        {
            isReadyToMove = true;
        }
    }

    /*public void OnMoveButton(Vector2 direction)
    {
        if (isReadyToMove)
        {
            isReadyToMove = false;
            Move(direction);

            // Delay nhỏ để tránh spam liên tục
            StartCoroutine(ResetMoveCooldown());
        }
    }*/

    private IEnumerator ResetMoveCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        isReadyToMove = true;
    }

    public override void LoadObjList(List<GameObject> obstacleL, List<GameObject> allowedObjectsL, List<PushAbleGameObj> pushAbleL)
    {
        obstacleList.Clear();
        pushAbleList.Clear();
        allowedObjectsWhenSmall.Clear();

        allowedObjectsWhenSmall = allowedObjectsL;
        obstacleList = obstacleL;
        pushAbleList = pushAbleL;
    }

    private void PuffedUp()
    {
        //Puffed-Up
        spr.sprite = sprFish[1];
        usePushAbleObjects = true;
        count = 5;

        for (int i = 0; i < bb.Length; i++)
        {
            bb[i].SetActive(true);
        }

        if (usePushAbleObjects)
        {
            foreach (LarnternFish enemy in LevelManager.Ins.level.EnemyList())
            {
                enemy.Stun();
            }
        }
    }

    private void PuffedDown()
    {
        //Puffed-Down
        spr.sprite = sprFish[0];
        count = 0;
        usePushAbleObjects = false;
        if (localCoral != null)
        {
            localCoral.ActiveBox();
        }
    }

    public override bool Move(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > 0.1f)
        {
            spr.flipX = direction.x < 0;
        }

        if (Mathf.Abs(direction.x) < 0.5f)
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }

        direction.Normalize();

        if (Blocked(transform.position, direction))
        {
            return false;
        }
        else
        {
            transform.Translate(direction);

            // Sau khi di chuyển mới xử lý phồng / xẹp
            if (count > 0)
            {
                count--;
                if (count < bb.Length)
                {
                    bb[count].SetActive(false); // Tắt dần từng bubble
                }
            }

            if (count <= 0)
            {
                PuffedDown();
            }

            StartCoroutine(DelayedEnemyMove());

            return true;
        }
    }

    public override bool Blocked(Vector3 position, Vector2 direction)
    {
        Vector2 newPos = new Vector2(position.x, position.y) + direction * distancePerGrid;

        foreach (var obj in obstacleList)
        {
            if (obj.transform.position.x == newPos.x && obj.transform.position.y == newPos.y)
            {
                return true;
            }
        }

        foreach (var allowedObj  in allowedObjectsWhenSmall)
        {
            if (allowedObj.transform.position.x == newPos.x && allowedObj.transform.position.y == newPos.y)
            {
                if (!usePushAbleObjects && allowedObjectsWhenSmall.Contains(allowedObj))
                {
                    continue; // 0 bị chặn
                }

                return true;
            }
        }

        foreach (var gate in LevelManager.Ins.level.GateList())
        {
            if (gate.transform.position.x == newPos.x && gate.transform.position.y == newPos.y)
            {
                if (gate.IsBlocked())
                {
                    return true;
                }
            }
        }

        if (!usePushAbleObjects)
            return false;

        foreach (PushAbleGameObj objToPush in pushAbleList)
        {
            if (objToPush.transform.position.x == newPos.x && objToPush.transform.position.y == newPos.y)
            {
                if (objToPush != null && objToPush.Move(direction))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator DelayedEnemyMove()
    {
        yield return new WaitForSeconds(0.25f);

        if (!usePushAbleObjects)
        {
            foreach (LarnternFish enemy in LevelManager.Ins.level.EnemyList())
            {
                enemy.Move();
            }
        }
    }


    private IEnumerator IEDead()
    {
        anim.Play(CacheString.TAG_DEAD);
        isDed = true;
        UIManager.Ins.CloseUI<MainCanvas>();
        yield return new WaitForSeconds(1.2f);
        UIManager.Ins.OpenUI<LooseCanvas>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Coral detectedCoral = Cache.GetCoral(other);
        if (detectedCoral != null)
        {
            if (localCoral == null)
            {
                localCoral = detectedCoral;
            }
            PuffedUp();
            localCoral.DeActiveBox();
        }

        Star star = Cache.GetStar(other);
        if (star != null)
        {
            star.Eat();
        }

        LarnternFish enemy = Cache.GetEnemy(other);
        if (enemy != null)
        {
            StartCoroutine(IEDead());
        }

        Crab crab = Cache.GetCrab(other);
        if (crab != null)
        {
            StartCoroutine(IEDead());
        }

        Btn btn = Cache.GetBtn(other);
        if (btn != null)
        {
            btn.OpenGate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Btn btn = Cache.GetBtn(collision);
        if (btn != null)
        {
            btn.CloseGate();
        }
    }
}