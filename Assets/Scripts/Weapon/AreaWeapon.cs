//using UnityEngine;

//public class AreaWeapon : MonoBehaviour
//{
//    [SerializeField] GameObject FireCircle;
//    public float WeaponDuration;
//    [SerializeField] float WeaponCooldown;
//    [SerializeField] float SpawnTimer;
    
//    private void Start()
//    {
        
//    }
//    private void Update()
//    {
//        SpawnTimer -= Time.deltaTime;
//        if (SpawnTimer <= 0)
//        {
//            SpawnTimer = WeaponCooldown;
//            Instantiate(FireCircle, transform.position, transform.rotation, transform);
//            Debug.Log("spawn" + FireCircle.transform.position);
//        }
//    }
//}
