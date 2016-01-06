using UnityEngine;
using System;
using System.Collections;


namespace Assets
{
	public class PlayerControllerRemotes : IPlayerController
	{
        private Player _player;


        public void Init(Player player)
        {
            _player = player;
        }
		public void Init(SinglePlayer player)
		{
		}

		public void OnUpdate()
		{

		}

		public void OnFixedUpdate()
		{
			if(GameMulti.Instance.Active)
				InputManager();
        }


		void InputManager () {
			if (SystemInfo.deviceType == DeviceType.Desktop)
			{
				if(!Input.GetKey(KeyCode.Mouse0)) return;
				Vector2 t = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				GameMulti.Instance.PlayerBlue.GoTo(t);
				t.y = -t.y;
				t.x = -t.x;
				if(GameMulti.Instance.PlayerRed != null)	GameMulti.Instance.PlayerRed.GoTo(t);

			}
			else if (Input.touchCount > 0)
			{
				var t = Input.GetTouch(0).position;
				GameMulti.Instance.PlayerBlue.GoTo(t);
				t.y = -t.y;
				t.x = -t.x;
				if(GameMulti.Instance.PlayerRed != null)	GameMulti.Instance.PlayerRed.GoTo(t);

			}
		}
    }
}

