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


		public void Start(){
			NetworkConnectionError error = Initialize_Server(1234,2,"Multi", "Nazwa gry");
			if(error != NetworkConnectionError.NoError){
				Debug.LogError("Nie utworzono serwera");
			}		
		}
		public void OnUpdate()
		{

		}

		public void OnFixedUpdate()
		{
			Debug.Log("fix");
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
			else{
#if UNITY_EDITOR
				Debug.LogError("Nie znaleziono gracza: "+ _player.tag);
#endif
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

		public static NetworkConnectionError Initialize_Server (int Port, int MaxPlayer, string gameType, string gameName) {
			Network.isMessageQueueRunning = true;
			bool useNat = !Network.HavePublicAddress();
			NetworkConnectionError error = Network.InitializeServer(MaxPlayer-1, Port, useNat);
			if (error == NetworkConnectionError.NoError){
				Network.minimumAllocatableViewIDs = 200;
			}
			return error;
		}

		public static NetworkConnectionError ConnectToServer(int Port, string IP, string Map){
			Network.isMessageQueueRunning = true;
			NetworkConnectionError error = Network.Connect(IP, Port);
			if (error == NetworkConnectionError.NoError){
				Network.isMessageQueueRunning = false;
				Application.LoadLevel(Map);
				Network.isMessageQueueRunning = true;
			}
			return error;
		}
    }
}

