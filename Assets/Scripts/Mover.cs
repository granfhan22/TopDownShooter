// Mover.cs
using UnityEngine;

namespace Topdown.movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] protected float speed = 5f;
        [SerializeField] protected Rigidbody2D body;

        protected Vector2 currentInput;

        protected virtual void Awake()
        {
            if (body == null) body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0;
            body.freezeRotation = true;
        }

        protected virtual void FixedUpdate()
        {
            body.linearVelocity = currentInput * speed;
        }

        public void SetInput(Vector2 input) => currentInput = input;
    }
}