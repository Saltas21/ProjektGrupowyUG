using UnityEngine;

namespace Assets
{
    public class AiPlayer : Player
    {
        public Rigidbody2D Puck;

        private readonly Vector2 _start = new Vector2(0, 3);
        private readonly Vector2 _stuckDestination = new Vector2(0, 4);

        private bool _isStuck = false;
        private Vector2 _oldPosition;
        private float _stuckTime;

        protected override void Start()
        {
            base.Start();
            MaxSpeed = 10;
        }

        // Update is called once per frame
        void Update()
        {
            if (Puck.position.y > 0 && Puck.position.y < 5)
            {
                if (_isStuck)
                {
                    if (Done)
                        _isStuck = false;
                    else
                        GoTo(_stuckDestination);
                    return;
                }
                if (Puck.velocity.y > 15 && Puck.position.x < -.6)
                {
                    GoTo(new Vector2(-1, 4.5f));
                } 
                else if (Puck.velocity.y > 10 && Puck.position.x > .6)
                {
                    GoTo(new Vector2(1, 4.5f));
                }
                /*else if (Puck.velocity.y > 10 && Puck.position.y < 2)
                {
                    var t = (Vector2)Vector3.Project(Body.position - Puck.position, Puck.velocity.normalized) +
                            Puck.position;
                    GoTo(t);
                }*/
                else if (Puck.velocity.y > -5)
                {
                    var t = Puck.position;
                    var r = (t - Body.position).normalized;
                    GoTo(t + r * 5);
                }
                UpdateState();
            }
            else
            {
                GoTo(_start);
            };
        }

        void UpdateState()
        {
            if (Vector2.Distance(_oldPosition, Body.position) < .1)
            {
                _stuckTime += Time.deltaTime;
                if (_stuckTime > .5 && !Done)
                    _isStuck = true;
            }
            else
            {
                _stuckTime = 0;
            }
            _oldPosition = Body.position;
        }
    }
}


