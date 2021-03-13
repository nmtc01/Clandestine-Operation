using UnityEngine;

public class EnemyHealthUI : HealthUIController
{
    private void FixedUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
