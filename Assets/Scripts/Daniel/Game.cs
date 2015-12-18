using System;
using UnityEngine;

namespace Assets
{
    public class Game : MonoBehaviour
    {
		
		#region singletone
		private static Game _instance;
		public static Game Instance
		{
			get 
			{
				if(_instance == null)
				{
					_instance = FindObjectOfType<Game>() as Game;
				}
				return _instance;
			}
		}
		#endregion

        public enum GameMode
        {
            AiVsAi,
            PlayerVsAi,
            PlayerVsPlayer
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                Puck.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Puck.GetComponent<Collider2D>().enabled = value;
                if (PlayerBlue != null)
                    PlayerBlue.Active = value;
                if (PlayerRed != null)
                    PlayerRed.Active = value;
            }
        }

        public GameMode Mode;
        public GameObject Puck;
        public Player PlayerRed;
        public Player PlayerBlue;
        private bool _active = true;
        private float _delay;
        private float _resetTime = 1.5f;

        // Use this for initialization
        void Start()
        {
            var body = Puck.GetComponent<Rigidbody2D>();

            switch (Mode)
            {
                case GameMode.AiVsAi:
					PlayerBlue.gameObject.SetActive(true);
					PlayerRed.Controller = new AiController { Puck = body };
                    PlayerBlue.Controller = new AiController { Puck = body, Direction = AiController.PlayingDirection.Up};

				break;
                case GameMode.PlayerVsAi:
					PlayerBlue.gameObject.SetActive(true);
					PlayerRed.Controller = new AiController { Puck = body };
                    PlayerBlue.Controller = new PlayerController();

				break;
                case GameMode.PlayerVsPlayer:
					PlayerBlue.gameObject.SetActive(false);
					PlayerBlue.Controller = new PlayerControllerRemotes();
					break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FixedUpdate()
        {
            if (Active)
            {
                var body = Puck.GetComponent<Rigidbody2D>();
                if (body.position.y > 5.2)
                    BlueScored();
                else if (body.position.y < -5.2)
                    RedScored();
            }
            else
            {
                _delay += Time.fixedDeltaTime;
                if (_delay >= _resetTime)
                {
                    _delay = 0;
                    Active = true;
                }
            }
        }

        private void RedScored()
        {
            Active = false;
            Debug.Log("Red scores!"); // TODO
            iTween.MoveTo(Puck, Vector3.down, _resetTime);
        }

        private void BlueScored()
        {
            Active = false;
            Debug.Log("Blue scores!"); // TODO
            iTween.MoveTo(Puck, Vector3.up, _resetTime);
        }
    }
}
