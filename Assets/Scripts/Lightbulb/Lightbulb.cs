using UnityEngine;

public class Lightbulb : MonoBehaviour
{
    [SerializeField]
    private GameObject crackedLightbulb = null;
    [SerializeField]
    private Vector3 initialPosition = Vector3.zero;
    [SerializeField]
    private float lightbulbDamage = 10f;

    private LightbulbController lightbulbController = null;
    private GameObject newCrackedLb = null;

    private void OnCollisionEnter(Collision collision)
    {
        newCrackedLb = Instantiate(crackedLightbulb, transform.position, transform.rotation);
        gameObject.SetActive(false);

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Health health = Player.GetInstance().GetComponent<Health>();
            health.Damage(lightbulbDamage);
        }

        lightbulbController.ShatteredLightbulb();
    }

    public void SetController(LightbulbController controller)
    {
        lightbulbController = controller;
    }

    public void Spawn()
    {
        gameObject.transform.localPosition = initialPosition;
        gameObject.SetActive(true);
        Destroy(newCrackedLb);
    }
}