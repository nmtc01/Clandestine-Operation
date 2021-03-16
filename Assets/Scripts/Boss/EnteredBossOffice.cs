using UnityEngine;

public class EnteredBossOffice : MonoBehaviour
{
    [SerializeField]
    private GameObject secondaryCamera = null;
    private bool firstTime = true;
    [SerializeField]
    private GameObject spot = null;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet") && firstTime)
        {
            firstTime = false;
            secondaryCamera.SetActive(true);
            GameObject fourthWall = FourthWall.GetInstance();
            if (fourthWall) fourthWall.SetActive(true);
            GameObject player = Player.GetInstance();
            player.transform.position = new Vector3(spot.transform.position.x, player.transform.position.y, player.transform.position.z);
            PlayerControl playerControl = Player.GetInstanceControl();
            playerControl.SetInTransition(true);
            playerControl.ResetPlayerMovements();
        }
    }
}
