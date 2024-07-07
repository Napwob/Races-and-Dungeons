using UnityEngine;
using UnityEngine.UI;

public class ManaController : MonoBehaviour
{
    [SerializeField] private float maxMana = 100f;
    private float currentMana;
    [SerializeField] private float manaRegenRateInSecond = 10f;

    private float timer;

    void Start()
    {
        currentMana = maxMana; // начальное значение маны
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            RegenerateMana();
            timer = 0f;
        }
    }

    void RegenerateMana()
    {
        if (currentMana >= maxMana)
            return;

        currentMana += manaRegenRateInSecond;
    }
    
    public float getCurrentMana() => currentMana;
    public void addToCurrentMana(float mana)
    {
        currentMana += mana;
        if (currentMana >= maxMana)
            currentMana = maxMana;
    }

    public bool getFromCurrentMana(float mana)
    {
        if (currentMana - mana < 0)
            return false;

        currentMana -= mana;
        return true;
    }
}