
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Game/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int attackPower;
    public int manaCost;
    public Sprite cardArt;
}
