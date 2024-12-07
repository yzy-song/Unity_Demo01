
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int health = 50;
    public int attack = 10;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Defeated");
    }
}
