using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Controller
{
    public float distancePerGrid = 1f;

    [Header("-----Components-----")]
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Sprite[] sprFish;

    [Header("-----List-----")]
    [SerializeField] private List<GameObject> obstacleList = new List<GameObject>();
    [SerializeField] private List<PushAbleGameObj> pushAbleList = new List<PushAbleGameObj>();

    private bool isReadyToMove = true;
    private bool usePushAbleObjects;
    public int count;

    private Coral localCoral;

    void Start()
    {
        LoadObjList(LevelManager.Ins.level.GameObjList(), LevelManager.Ins.level.PushAbleGameObjList());
    }

    void Update()
    {
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

    public override void LoadObjList(List<GameObject> obstacleL, List<PushAbleGameObj> pushAbleL)
    {
        obstacleList.Clear();
        pushAbleList.Clear();

        obstacleList = obstacleL;
        pushAbleList = pushAbleL;
    }

    private void PuffedUp()
    {
        //Puffed-Up
        spr.sprite = sprFish[1];
        usePushAbleObjects = true;
        count = 5;
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
        count--;

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

            if (count < 0)
            {
                count = 0;
            }
            else if (count == 0)
            {
                PuffedDown();
            }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        localCoral = Cache.GetCoral(other);
        if (localCoral != null)
        {
            PuffedUp();
            localCoral.DeActiveBox();
        }

        /*
         WinBox box = Cache.GetWinBox(other);
         if (box != null)
         {
             LevelManager.Ins.isWin = true;
             UIManager.Ins.CloseUI<MainCanvas>();
             UIManager.Ins.OpenUI<WinCanvas>();
         }*/
    }
}