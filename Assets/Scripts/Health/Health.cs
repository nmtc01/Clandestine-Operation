using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField]
    protected float maxHealth = 100f;
    private IHealthController healthController;
    [SerializeField]
    protected HealthUIController healthUIController = null;

    protected float currentHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        healthController = GetComponent<IHealthController>();
        healthUIController.SetMaxValue(maxHealth);
    }

    public virtual void Kill()
    {
        currentHealth = 0;

        healthController.SetIsDead(true);

        SetHealth();
    }

    public virtual void Damage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) 
            Kill();
        else
            SetHealth();
    }

    public virtual void Heal(float value)
    {
        currentHealth += value;

        if (currentHealth > maxHealth) currentHealth = maxHealth;

        SetHealth();
    }

    private void SetHealth()
    {
        healthUIController.SetValue(currentHealth);
    }
}
