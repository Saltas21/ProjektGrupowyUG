using UnityEngine;

namespace Assets
{
    public class Puck : MonoBehaviour
    {
        private Rigidbody2D _body;

        public float MaxSpeed = 30f;//Replace with your max speed

        void Start()
        {
            _body = gameObject.GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
			if(Network.isServer)
            if (_body.velocity.magnitude > MaxSpeed)
            {
                _body.velocity = _body.velocity.normalized * MaxSpeed;
            }
        }
    }
}
