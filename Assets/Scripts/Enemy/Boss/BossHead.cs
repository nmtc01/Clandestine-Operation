using UnityEngine;

public class BossHead : MonoBehaviour
{
    [SerializeField]
    private Health health = null;

    public void DamageHead(float damage)
    {
        health?.Damage(2*damage);
    }
}
