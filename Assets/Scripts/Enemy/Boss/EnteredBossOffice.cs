using UnityEngine;

public class EnteredBossOffice : MonoBehaviour
{
    private bool firstTime = true;
    [SerializeField]
    private int spot = 30;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet") && firstTime)
        {
            firstTime = false;
            BossController.GetInstance().ActivateBossCamera();
            GameObject player = Player.GetInstance();
            player.transform.position = new Vector3(spot, player.transform.position.y, player.transform.position.z);
        }
    }
}
