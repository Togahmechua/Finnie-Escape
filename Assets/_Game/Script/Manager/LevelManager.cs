using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using System;

public class LevelManager : Singleton<LevelManager>
{
    public MapSO mapSO;
    [HideInInspector] public Level level;
    public int curMap;
    public bool isWin;
    public int curMapID;

    private List<AsyncOperationHandle<GameObject>> loadedLevels = new List<AsyncOperationHandle<GameObject>>();

    private void Start()
    {
        curMap = PlayerPrefs.GetInt("CurrentMap", 0);
        mapSO.LoadWinStates();
    }

    public void LoadMapByID(int id)
    {
        curMapID = id;

        if (level != null)
        {
            DespawnMap();
        }

        MapSO.MapDetails mapDetails = mapSO.mapList.Find(x => x.levelID == id);
        if (mapDetails == null || !mapDetails.levelPrefab.RuntimeKeyIsValid())
        {
            Debug.LogError("Không tìm thấy Level ID: " + id);
            return;
        }

        var handle = mapDetails.levelPrefab.InstantiateAsync(transform);
        handle.Completed += OnLevelLoaded;

        loadedLevels.Add(handle);
    }

    private void OnLevelLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            level = handle.Result.GetComponent<Level>();
            if (level == null)
            {
                Debug.LogError("Level Prefab không có component Level!");
            }
        }
        else
        {
            Debug.LogError("Load Level thất bại!");
        }
    }

    public void DespawnMap()
    {
        if (level != null)
        {
            foreach (var handle in loadedLevels)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }

            loadedLevels.Clear();
            level = null;
            isWin = false;
            Debug.Log("Đã xóa Level cũ!");
        }
    }
}
