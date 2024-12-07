using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Player Settings")]
    public int initialPlayerHealth = 100;
    public int initialPlayerMana = 50;

    [Header("Enemy Settings")]
    public int initialEnemyHealth = 50;
    public int enemySpawnRate = 3;

    [Header("Battle Settings")]
    public float battleSpeed = 1.0f; // 控制倍速
    public bool enableReplay = true;

    [Header("Audio Settings")]
    public AudioClip loginSceneBGM;
    public AudioClip mainSceneBGM;
    public AudioClip battleSceneBGM;
}
