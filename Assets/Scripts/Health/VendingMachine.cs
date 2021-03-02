using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    [SerializeField]
    private GameObject key = null;
    private int range = 5;
    private float healFactor = 50f;
    private bool active = true;

    // Update is called once per frame
    void Update()
    {
        if ((Player.GetInstance().transform.position - this.transform.position).magnitude < range && active)
        {
            key.SetActive(true);
            if (Input.GetButtonDown("Heal"))
            {
                Health health = Player.GetInstance().GetComponent<Health>();
                health.Heal(healFactor);
                active = false;
            }
        }
        else key.SetActive(false);

    }
}