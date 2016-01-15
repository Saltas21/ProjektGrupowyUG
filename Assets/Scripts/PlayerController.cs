using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : IPlayerController
    {
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
            _player.MaxSpeed = 30;
        }

        public void OnUpdate()
        {
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                Vector2 t = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _player.GoTo(t);
            }
            else if (Input.touchCount > 0)
            {
                var t = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                _player.GoTo(t);
            }
        }
    }
}

