using UnityEngine;

namespace Topdown.movement
{
    public class Rotator : MonoBehaviour
    {
        protected void LookAt(Vector3 Target)
        {
            float LookAngle = AngleBetweenTwoPoints(transform.position, Target) + 90;

            transform.eulerAngles = new Vector3(0, 0, LookAngle);
        }

        private float AngleBetweenTwoPoints(Vector3 a , Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }
    }
}
