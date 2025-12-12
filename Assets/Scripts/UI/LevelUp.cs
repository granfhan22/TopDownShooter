using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1f);
    }
    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * 3;
    }
}
