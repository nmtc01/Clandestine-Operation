using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    [SerializeField]
    private Health health = null;

    public void KillEnemy()
    {
        health?.Kill();
    }
}
