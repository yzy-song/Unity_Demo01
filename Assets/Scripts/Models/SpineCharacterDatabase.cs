using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class SpineCharacterDatabase
{
    public List<SpineCharacter> characters;

    private static SpineCharacterDatabase instance;
    public static SpineCharacterDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                // 从 Resources 加载 JSON 配置文件
                UnityEngine.TextAsset jsonFile = Resources.Load<UnityEngine.TextAsset>("SpineCharacters");
                instance = JsonUtility.FromJson<SpineCharacterDatabase>(jsonFile.text);
            }
            return instance;
        }
    }

    public SpineCharacter GetCharacterById(int id)
    {
        return characters.Find(character => character.id == id);
    }
}
