using UnityEngine;

public class EnteredBossOffice : MonoBehaviour
{
    [SerializeField]
    private GameObject secondaryCamera = null;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet") && !TimerCountDown.IsCounting())
        {
            secondaryCamera.SetActive(true);
        }
    }
}
