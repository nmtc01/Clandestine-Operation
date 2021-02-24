using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    private IHealthController healthController;

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthController = GetComponent<IHealthController>();
    }

    public void Kill()
    {
        currentHealth = 0;

        healthController.SetIsDead(true);
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0) Kill();
    }

    public void Heal(float value)
    {
        currentHealth += value;

        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
}
