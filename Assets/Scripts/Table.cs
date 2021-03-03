using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField]
    private PlayerGun gun = null;
    [SerializeField]
    private GameObject key = null;

    private bool canGrabGun = false;
    private bool hasObject = true;

    private void Update()
    {
        if(hasObject && canGrabGun && Input.GetButtonDown("Interact"))
        {
            PlayerShoot playerShoot = Player.GetInstance().GetComponent<PlayerShoot>();

            playerShoot.SetNewGun(gun);

            key.SetActive(false);
            hasObject = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(hasObject && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            canGrabGun = true;
            key.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasObject && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            canGrabGun = false;
            key.SetActive(false);
        }
    }
}
