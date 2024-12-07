using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterWaveConfig
{
    public List<WaveInfo> waves;

    private static MonsterWaveConfig instance;
    public static MonsterWaveConfig Instance
    {
        get
        {
            if (instance == null)
            {
                // 从 Resources 加载 JSON 配置文件
                TextAsset jsonFile = Resources.Load<TextAsset>("MonsterWaveConfig");
                instance = JsonUtility.FromJson<MonsterWaveConfig>(jsonFile.text);
            }
            return instance;
        }
    }

    public WaveInfo GetWaveInfo(int waveIndex)
    {
        return waves.Find(wave => wave.waveIndex == waveIndex);
    }
}
