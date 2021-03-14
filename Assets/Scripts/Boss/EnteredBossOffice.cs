using UnityEngine;

public class EnteredBossOffice : MonoBehaviour
{
    [SerializeField]
    private GameObject secondaryCamera = null;
    private bool firstTime = true;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet") && firstTime)
        {
            firstTime = false;
            secondaryCamera.SetActive(true);
        }
    }
}
