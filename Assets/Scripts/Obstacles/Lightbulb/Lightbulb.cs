using UnityEngine;

public class Lightbulb : MonoBehaviour
{
    [SerializeField]
    private GameObject crackedLightbulb = null;
    [SerializeField]
    private Vector3 initialPosition = Vector3.zero;
    [SerializeField]
    private float lightbulbDamage = 10f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private GameObject newCrackedLb = null;

    private void OnCollisionEnter(Collision collision)
    {
        newCrackedLb = Instantiate(crackedLightbulb, transform.position, transform.rotation);
        gameObject.SetActive(false);

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.GetInstanceHealth().Damage(lightbulbDamage);
        }

        LightbulbController.GetInstance().ShatteredLightbulb(this);
    }

    public void Spawn()
    {
        gameObject.transform.localPosition = initialPosition;
        gameObject.SetActive(true);
        rb.velocity = Vector3.down * 10;
        rb.angularVelocity = Vector3.zero;
        Destroy(newCrackedLb);
    }
}
