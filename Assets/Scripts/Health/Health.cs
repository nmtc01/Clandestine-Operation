using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    protected float maxHealth = 100f;
    private IHealthController healthController;

    protected float currentHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        healthController = GetComponent<IHealthController>();
    }

    public virtual void Kill()
    {
        currentHealth = 0;

        healthController.SetIsDead(true);
    }

    public virtual void Damage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) Kill();
    }

    public virtual void Heal(float value)
    {
        currentHealth += value;

        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
}
