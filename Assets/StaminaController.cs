using UnityEngine;
using UnityEngine.UI;

public class StaminaRegeneration : MonoBehaviour
{
    [SerializeField] private float staminaMax = 100f;
    private float currentStamina;
    [SerializeField] private float staminaRegenRateInSecond = 10f;

    private float timer;

    void Start()
    {
        currentStamina = staminaMax; // начальное значение маны
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            RegenerateStamina();
            timer = 0f;
        }
    }

    void RegenerateStamina()
    {
        if (currentStamina >= staminaMax)
            return;

        currentStamina += staminaRegenRateInSecond;
    }
    
    public float getCurrentStamina() => currentStamina;
    public void addToCurrentStamina(float mana)
    {
        currentStamina += mana;
        if (currentStamina >= staminaMax)
            currentStamina = staminaMax;
    }

    public bool getFromCurrentStamina(float mana)
    {
        if (currentStamina - mana < 0)
            return false;

        currentStamina -= mana;
        return true;
    }
}