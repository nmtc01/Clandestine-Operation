using UnityEngine;

public class Armour : MonoBehaviour
{
    [SerializeField]
    protected float maxShield = 100f;

    protected float currentShield;

    private bool active = true;

    [SerializeField]
    private GameObject armour = null;

    [SerializeField]
    private EnemyController enemyController = null;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentShield = maxShield;
    }

    public void DestroyShield()
    {
        currentShield = 0;
        active = false;
        Destroy(armour);
    }

    public virtual void Damage(float damage)
    {
        currentShield -= damage;

        enemyController.AlertEnemy();

        if (currentShield <= 0) DestroyShield();
    }

    public bool IsActive()
    {
        return active;
    }
}
