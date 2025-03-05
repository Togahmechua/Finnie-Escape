using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAbleGameObj : Controller
{
    private List<GameObject> obstacleList = new List<GameObject>();
    [SerializeField] private List<GameObject> allowedObjectsWhenSmall = new List<GameObject>();
    private List<PushAbleGameObj> pushAbleList = new List<PushAbleGameObj>();

    private bool isFalling = false;
    private float fallDelay = 0.15f; // Thời gian giữa mỗi lần rơi
    public bool canFall = false; // Biến kiểm soát có rơi hay không

    void Start()
    {
        LoadObjList(LevelManager.Ins.level.GameObjList(), LevelManager.Ins.level.AllowedObjList(), LevelManager.Ins.level.PushAbleGameObjList());
        StartCoroutine(FallLoop());
    }

    private IEnumerator FallLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(fallDelay);

            if (!Blocked(transform.position, Vector2.down))
            {
                Move(Vector2.down);
                isFalling = true;
            }
            else
            {
                if (isFalling)
                {
                    isFalling = false;
                    AlignToGrid();
                }
            }
        }
    }

    private void AlignToGrid()
    {
        transform.position = new Vector3(
            Mathf.Round(transform.position.x * 2) / 2,
            Mathf.Round(transform.position.y * 2) / 2,
            transform.position.z
        );
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

    public override bool Move(Vector2 direction)
    {
        if (Blocked(transform.position, direction))
        {
            return false;
        }
        else
        {
            transform.Translate(direction);
            return true;
        }
    }

    public override bool Blocked(Vector3 position, Vector2 direction)
    {
        Vector2 newPos = new Vector2(position.x, position.y) + direction * LevelManager.Ins.level.player.distancePerGrid;

        for (int i = obstacleList.Count - 1; i >= 0; i--)
        {
            if (obstacleList[i] == null)
            {
                obstacleList.RemoveAt(i);
                continue;
            }

            if (obstacleList[i].transform.position.x == newPos.x && obstacleList[i].transform.position.y == newPos.y)
            {
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

        for (int i = pushAbleList.Count - 1; i >= 0; i--)
        {
            if (pushAbleList[i] == null)
            {
                pushAbleList.RemoveAt(i);
                continue;
            }

            if (pushAbleList[i].transform.position.x == newPos.x && pushAbleList[i].transform.position.y == newPos.y)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 0.6f);
    }
}
