using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;



namespace Assets
{
	public class PlayerControllerRemotes : NetworkBehaviour, IPlayerController
	{
        private Player _player;
		public Player _otherPlayer;


        public void Init(Player player)
        {
            _player = player;
        }

		public void OnUpdate()
		{

		}

		public void OnFixedUpdate()
		{
			if(Game.Instance.Active)
				InputManager();
        }


		void InputManager () {
			if(isServer){
				if (SystemInfo.deviceType == DeviceType.Desktop)
				{
					Vector2 t = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					Game.Instance.PlayerRed.GoTo(t);

				}
				else if (Input.touchCount > 0)
				{
					var t = Input.GetTouch(0).position;
					Game.Instance.PlayerRed.GoTo(t);
				}
			}
			else{
				if (SystemInfo.deviceType == DeviceType.Desktop)
				{
					Vector2 t = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					Game.Instance.PlayerBlue.GoTo(t);
					
				}
				else if (Input.touchCount > 0)
				{
					var t = Input.GetTouch(0).position;
					Game.Instance.PlayerBlue.GoTo(t);
				}
				
			}

		}
    }
}

