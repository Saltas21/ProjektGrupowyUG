using UnityEngine;

namespace Assets
{
    public class Player : MonoBehaviour
    {
        private Vector3 _initPosition;
        private bool _active = true;
        private Vector2 _target;
        private IPlayerController _controller;
		public NetworkView networkView;

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                if (!_active)
                    _target = _initPosition;
            }
        }

        public Rigidbody2D Body { get; set; }

        public IPlayerController Controller
        {
            get { return _controller; }
            set
            {
                _controller = value;
                _controller.Init(this);
            }
        }

        public bool Done { get; set; }

        public float MaxSpeed = 25;
        public Bounds Bounds;

        // Use this for initialization
        void Start()
        {
            Body = gameObject.GetComponent<Rigidbody2D>();
            _initPosition = gameObject.transform.position;
            _target = _initPosition;
        }

        void Update()
        {
            if (Controller != null)
                    Controller.OnUpdate();
        }

        void FixedUpdate()
        {
            Vector2 position = gameObject.transform.position;
            var distance = Vector2.Distance(position, _target);
            var dir = (_target - position).normalized;

            var speed = Active ? MaxSpeed : 3;
            Body.velocity = dir * speed * Mathf.Clamp(distance, 0, 1);

            if (Vector2.Distance(_target, Body.position) < .1)
            {
                Done = true;
            }
        }

        public void GoTo(Vector2 target)
        {
            if (!_active) return;
            Done = false;
            _target = Bounds.ClosestPoint(target);
        }
    }
}
