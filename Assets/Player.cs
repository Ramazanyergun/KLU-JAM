using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Healthbar healthbar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth= maxHealth;
        healthbar.SetmaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
    void TakeDamage(int damage)
    {
        currentHealth-= damage;

        healthbar.SetHealth(currentHealth);
    }
}
