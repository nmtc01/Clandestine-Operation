public class PlayerHealthUI : HealthUIController
{
    #region Singleton
    public static PlayerHealthUI instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion
}
