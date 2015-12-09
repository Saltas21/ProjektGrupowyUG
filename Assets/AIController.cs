using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    public class AiController : IPlayerController
    {
        public enum PlayingDirection
        {
            Up,
            Down
        }

        public Rigidbody2D Puck;

        public PlayingDirection Direction
        {
            get
            {
                return _m == 1 ? PlayingDirection.Down : PlayingDirection.Up;
            }
            set { _m = value == PlayingDirection.Down ? 1 : -1; }
        }

        private readonly Vector2 _start = new Vector2(0, 3);
        private readonly Vector2 _stuckDestination = new Vector2(0, 4.5f);

        private int _m = 1;
        private Player _player;
        private bool _isStuck = false;
        private Vector2 _oldPosition;
        private float _stuckTime;

        // Update is called once per frame
        public void Init(Player player)
        {
            _player = player;
            _player.MaxSpeed = 8;
        }

        public void OnUpdate()
        {
            if (!_player.Active)
                return;

            var posY = Puck.position.y * _m;
            var velY = Puck.velocity.y * _m;
            const int max = 12;

            if (posY > 0 && posY < 5)
            {
                if (_isStuck)
                {
                    if (_player.Done)
                        _isStuck = false;
                    else
                        Go(_stuckDestination);
                    return;
                }
                if (velY > max && Puck.position.x < -.2)
                {
                    Go(new Vector2(-1, 4.5f));
                }
                if (velY > max && Puck.position.x > .2)
                {
                    Go(new Vector2(1, 4.5f));
                }
                else if (velY > -5)
                {
                    var toGoal = (new Vector2(0, 5 * _m) - _player.Body.position).normalized;
                    var random = new Vector2(Random.value, Random.value).normalized;
                    var t = Puck.position + toGoal*.2f;
                    if (posY < 3)
                        t += random * .1f;
                    var r = (t - _player.Body.position).normalized;
                    Go(t + r * 5, false);
                }
                UpdateState();
            }
            else
            {
                Go(_start);
            }
        }

        private void Go(Vector2 target, bool invert = true)
        {
            target = new Vector2(target.x, target.y*(invert ? _m : 1));
            _player.GoTo(target);
        }

        void UpdateState()
        {
            if (Vector2.Distance(_oldPosition, _player.Body.position) < .1)
            {
                _stuckTime += Time.deltaTime;
                if (_stuckTime > .5 && !_player.Done)
                    _isStuck = true;
            }
            else
            {
                _stuckTime = 0;
            }
            _oldPosition = _player.Body.position;
        }
    }
}


