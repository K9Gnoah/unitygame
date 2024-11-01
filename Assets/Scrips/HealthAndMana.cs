using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndMana : MonoBehaviour
{
    [SerializeField]
    Image healthBar;
    [SerializeField]
    Image manaBar;  // Reference to the mana bar UI

    [SerializeField]
    float playerMaxHealth = 100f;
    [SerializeField]
    float playerMaxMana = 100f;  // Maximum mana value
    [SerializeField]
    float manaRegenRate = 2f;    // Amount of mana to regenerate per second
    [SerializeField]
    float manaRegenOnHit = 10f;  // Amount of mana restored when hit

    private float playerHealth;
    private float playerMana;  // Current mana value
    private Image healthBarFill;
    private Image manaBarFill;  // UI fill component for the mana bar

    void Start()
    {
        healthBarFill = healthBar.GetComponentInChildren<Image>();
        manaBarFill = manaBar.GetComponentInChildren<Image>();  // Get the Image component for mana

        playerHealth = playerMaxHealth;
        playerMana = playerMaxMana;  // Initialize mana to max

        healthBarFill.fillAmount = 1f;  // Full health
        manaBarFill.fillAmount = 1f;    // Full mana

        StartCoroutine(RegenerateManaOverTime());  // Start passive mana regeneration
    }

    void Update()
    {
        if (playerHealth <= 0)
        {
            EndGame();
        }
    }

    // Method to take damage, update health bar, and restore mana when hit
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        healthBarFill.fillAmount = playerHealth / playerMaxHealth;

        if (playerHealth <= 0)
        {
            EndGame();
        }

        RestoreMana(manaRegenOnHit);  // Restore 10 mana on hit
    }

    // Method to check if the player has enough mana
    public bool HasEnoughMana(float manaCost)
    {
        return playerMana >= manaCost;
    }

    // Method to use mana for an action and update mana bar
    public void UseMana(float manaCost)
    {
        if (HasEnoughMana(manaCost))
        {
            playerMana -= manaCost;
            manaBarFill.fillAmount = playerMana / playerMaxMana;
        }
    }

    // Method to restore mana (for example, through power-ups or on hit)
    public void RestoreMana(float manaAmount)
    {
        playerMana = Mathf.Min(playerMana + manaAmount, playerMaxMana);  // Cap mana at max value
        manaBarFill.fillAmount = playerMana / playerMaxMana;
    }

    // Coroutine for passive mana regeneration over time
    IEnumerator RegenerateManaOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);  // Wait for 1 second
            RestoreMana(manaRegenRate);  // Restore 2 mana every second
        }
    }

    void EndGame()
    {
        Time.timeScale = 0;  // Stop the game when health reaches zero
    }
}
