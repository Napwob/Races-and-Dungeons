using System.Collections;
using UnityEngine;


public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // начальное значение маны
    }

    public int getCurrentHealth() => currentHealth;
    public void addToCurrentHealth(int health)
    {
        currentHealth += health;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
    }

    public void getFromCurrentHealth(int health)
    {
        if (currentHealth - health < 0)
            return;

        Debug.Log("Current Health: " + currentHealth);
        currentHealth -= health;
    }
}