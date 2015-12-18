using UnityEngine;

namespace Assets
{
	public class PlayerControllerRemotes : IPlayerController
	{
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
        }

		public double InterpolationBackTime = 0.1;
		public double ExtrapolationLimit = 0.5;
		
		internal struct MyTransform{
			internal double timestamp;
			internal Vector3 position;
			internal Vector3 velocity;

		}
		
		MyTransform[] BufferedTransform = new MyTransform[20];
		int TimeStampCount;

		public void OnUpdate()
		{

		}

		public void OnFixedUpdate()
		{
			if(Game.Instance.Active)
				InputManager();
        }


		void InputManager () {
			if(_player.tag == "Down"){
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
			
				double interpolationTime = Network.time - InterpolationBackTime;
				if (BufferedTransform[0].timestamp > interpolationTime){
					for(int i=0; i<TimeStampCount; i++){
						if (BufferedTransform[i].timestamp <= interpolationTime || i == TimeStampCount-1){
							MyTransform rhs = BufferedTransform[Mathf.Max(i-1, 0)];
							MyTransform lhs = BufferedTransform[i];
							double length = rhs.timestamp - lhs.timestamp;
							float t = 0.0f;
							if (length > 0.0001){
								t = (float)((interpolationTime - lhs.timestamp) / length);
							}
							Game.Instance.PlayerRed.GoTo(Vector3.Lerp(lhs.position, rhs.position, t));
							return;
						}
					}
				}
				else{
					MyTransform latest = BufferedTransform[0];
					float extrapolationLength = (float)(interpolationTime - latest.timestamp);
					if (extrapolationLength < ExtrapolationLimit){
						Game.Instance.PlayerRed.GoTo( latest.position + latest.velocity * extrapolationLength);
					}
				}
			}

		}
		void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
			if (stream.isWriting)
			{
				Vector3 pos = _player.Body.position;
				Vector3 velocity = _player.Body.velocity;
				stream.Serialize(ref pos);
				stream.Serialize(ref velocity);
			}
			else
			{
				Vector3 pos = Vector3.zero;
				Vector3 velocity = Vector3.zero;
				stream.Serialize(ref pos);
				stream.Serialize(ref velocity);

				for (int i = BufferedTransform.Length-1; i >= 1; i--){
					BufferedTransform[i] = BufferedTransform[i-1];
				}
				
				MyTransform myTransform;
				myTransform.timestamp = info.timestamp;
				myTransform.position = pos;
				myTransform.velocity = velocity;
				BufferedTransform[0] = myTransform;
				TimeStampCount = Mathf.Min(TimeStampCount + 1,BufferedTransform.Length);
			}
		}
    }
}

