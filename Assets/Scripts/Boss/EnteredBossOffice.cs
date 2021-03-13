using UnityEngine;

public class EnteredBossOffice : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet") && !TimerCountDown.IsCounting())
        {
        }
    }
}
