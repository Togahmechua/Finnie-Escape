using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "MapSO", menuName = "ScriptableObjects/MapSO", order = 1)]
public class MapSO : ScriptableObject
{
    public List<MapDetails> mapList = new List<MapDetails>();

    [System.Serializable]
    public class MapDetails
    {
        public int levelID;
        public AssetReferenceGameObject levelPrefab;
        public bool isWon;
    }

    public void LoadWinStates()
    {
        for (int i = 0; i < mapList.Count; i++)
        {
            string key = "MapWin_" + i;
            mapList[i].isWon = PlayerPrefs.GetInt(key, 0) == 1;
        }
    }
}
