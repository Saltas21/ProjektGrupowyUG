using UnityEngine;

namespace Assets
{
    public class PlayerController : IPlayerController
    {
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
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
                var t = Input.GetTouch(0).position;
                _player.GoTo(t);
            }
        }
    }
}

