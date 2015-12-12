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
			InputManager();
        }


		void InputManager () {
			if(_player.networkView.isMine && _player.tag == "Down"){
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
			else if(_player.tag == "Up"){
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
							_player.GoTo(Vector3.Lerp(lhs.position, rhs.position, t));
							return;
						}
					}
				}
				else{
					MyTransform latest = BufferedTransform[0];
					float extrapolationLength = (float)(interpolationTime - latest.timestamp);
					if (extrapolationLength < ExtrapolationLimit){
						_player.GoTo( latest.position + latest.velocity * extrapolationLength);
					}
				}
			}
		}
    }
}

