using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int id;

    public PlayerMovement player;

    [SerializeField] private Transform obstacleHolder;
    [SerializeField] private Transform allowedHolder;
    [SerializeField] private Transform pushAbleHolder;
    [SerializeField] private Transform gateHolder;
    [SerializeField] private Transform enemyHolder;

    private List<GameObject> obstacleList = new List<GameObject>();
    private List<GameObject> allowedObjectsWhenSmall = new List<GameObject>();
    private List<PushAbleGameObj> pushAbleList = new List<PushAbleGameObj>();
    [HideInInspector] public List<Gate> gateList = new List<Gate>();
    [HideInInspector] public List<LarnternFish> enemyList = new List<LarnternFish>();

    private void Start()
    {
        LoadList();
    }

    private void Update()
    {
        if (!LevelManager.Ins.isWin) return;

        if (id == LevelManager.Ins.curMapID &&
            !LevelManager.Ins.mapSO.mapList[LevelManager.Ins.curMapID].isWon)
        {
            LevelManager.Ins.mapSO.mapList[LevelManager.Ins.curMapID].isWon = true;
            SaveWinState(LevelManager.Ins.curMapID);
            Debug.Log("Map " + LevelManager.Ins.curMapID + " is won.");
            LevelManager.Ins.curMap++;
        }

        SetCurMap();
    }

    public List<GameObject> GameObjList() => obstacleList;
    public List<GameObject> AllowedObjList() => allowedObjectsWhenSmall;
    public List<PushAbleGameObj> PushAbleGameObjList() => pushAbleList;
    public List<Gate> GateList() => gateList;
    public List<LarnternFish> EnemyList() => enemyList;

    private void SetCurMap()
    {
        PlayerPrefs.SetInt("CurrentMap", LevelManager.Ins.curMapID);
        PlayerPrefs.Save();
    }

    private void SaveWinState(int mapIndex)
    {
        string key = "MapWin_" + mapIndex;
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
        LevelManager.Ins.mapSO.LoadWinStates();
    }

    public void LoadList()
    {
        obstacleList.Clear();
        allowedObjectsWhenSmall.Clear();
        pushAbleList.Clear();
        gateList.Clear();
        enemyList.Clear();

        LoadObjectsFromHolder(obstacleHolder, obstacleList);
        LoadObjectsFromHolder(allowedHolder, allowedObjectsWhenSmall);
        LoadComponentsFromHolder(pushAbleHolder, pushAbleList);
        LoadComponentsFromHolder(gateHolder, gateList);
        LoadComponentsFromHolder(enemyHolder, enemyList);
    }

    private void LoadObjectsFromHolder(Transform holder, List<GameObject> list)
    {
        for (int i = 0; i < holder.childCount; i++)
        {
            GameObject obj = holder.GetChild(i).gameObject;
            if (obj != null)
            {
                list.Add(obj);
            }
        }
    }

    private void LoadComponentsFromHolder<T>(Transform holder, List<T> list) where T : Component
    {
        for (int i = 0; i < holder.childCount; i++)
        {
            T component = holder.GetChild(i).GetComponent<T>();
            if (component != null)
            {
                list.Add(component);
            }
        }
    }
}
