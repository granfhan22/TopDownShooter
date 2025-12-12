using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private TextMeshProUGUI DamageText;
    private void Start()
    {
        Speed = Random.Range(0.1f, 1.5f);
        Destroy(gameObject, 1f);
    }
    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * Speed;
    }
    public void SetValue(int value)
    {
        DamageText.text = value.ToString();
    }
}
