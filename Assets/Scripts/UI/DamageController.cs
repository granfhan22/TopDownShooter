using UnityEngine;

public class DamageController : MonoBehaviour
{
    public static DamageController Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    public DamageUI dameprefabs;
    public LevelUp lvprefabs;

    public void CreateNumber(float value, Vector2 position)
    {
        DamageUI Damage = Instantiate(dameprefabs, position, transform.rotation, transform);
        Damage.SetValue(Mathf.RoundToInt(value));

    }
    public void CreatelvlUp(Vector2 position)
    {
        LevelUp lv = Instantiate(lvprefabs, position, transform.rotation, transform);
    }
}
