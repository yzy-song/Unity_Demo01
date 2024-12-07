using System.Collections.Generic;
using UnityEngine;

public class SpineManager : Singleton<SpineManager>
{
    private Dictionary<int, GameObject> loadedPrefabs = new Dictionary<int, GameObject>();

    public GameObject LoadSpineCharacter(int characterId)
    {
        if (!loadedPrefabs.ContainsKey(characterId))
        {
            // 获取角色信息
            SpineCharacter character = SpineCharacterDatabase.Instance.GetCharacterById(characterId);
            if (character == null)
            {
                Debug.LogError($"Character with ID {characterId} not found!");
                return null;
            }

            // 加载预制体
            GameObject prefab = Resources.Load<GameObject>(character.prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found at path: {character.prefabPath}");
                return null;
            }

            loadedPrefabs[characterId] = prefab;
        }

        // 实例化角色
        return Instantiate(loadedPrefabs[characterId]);
    }

    public void ClearCache()
    {
        loadedPrefabs.Clear();
    }
}
