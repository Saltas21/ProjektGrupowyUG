using UnityEngine;

namespace Assets
{
    public class Player : MonoBehaviour
    {
        protected Rigidbody2D Body;
        private Vector2 _target;

        public bool Done { get; set; }
        public float MaxSpeed = 30;
        public Bounds Bounds;

        // Use this for initialization
        protected virtual void Start()
        {
            Body = gameObject.GetComponent<Rigidbody2D>();
            _target = gameObject.transform.position;
        }

        void FixedUpdate()
        {
            Vector2 position = gameObject.transform.position;
            var distance = Vector2.Distance(position, _target);
            var dir = (_target - position).normalized;

            Body.velocity = dir * MaxSpeed * Mathf.Clamp(distance, 0, 1);

            if (Vector2.Distance(_target, Body.position) < .1)
            {
                Done = true;
            }
        }

        public void GoTo(Vector2 target)
        {
            Done = false;
            _target = Bounds.ClosestPoint(target);
        }
    }
}
