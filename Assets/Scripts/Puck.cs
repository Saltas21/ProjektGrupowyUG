using UnityEngine;

namespace Assets.Scripts
{
    public class Puck : MonoBehaviour
    {
        private Rigidbody2D _body;
        private AudioSource _audio;

        public float MaxSpeed = 30f;//Replace with your max speed

        void Start()
        {
            _body = gameObject.GetComponent<Rigidbody2D>();
            _audio = GetComponent<AudioSource>();
        }

        void FixedUpdate()
        {
            if (_body.velocity.magnitude > MaxSpeed)
            {
                _body.velocity = _body.velocity.normalized * MaxSpeed;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            _audio.volume = Mathf.Pow(collision.relativeVelocity.magnitude/10, 2);
            _audio.Play();
        }
    }
}
