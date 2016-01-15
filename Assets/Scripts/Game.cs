using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        public enum GameMode
        {
            AiVsAi,
            PlayerVsAi,
            Server,
            Client
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                Puck.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Puck.GetComponent<Collider2D>().enabled = value;
                if (PlayerDown != null)
                    PlayerDown.Active = value;
                if (PlayerUp != null)
                    PlayerUp.Active = value;
            }
        }

        public bool End { get; set;  }

        public GameMode Mode;
        public GameObject Puck;
        public AudioSource Stadium;
        public Canvas Menu;
        public Button ResumeButton;
        public Player PlayerUp;
        public Player PlayerDown;
        public Points PlayerPointsUp;
        public Points PlayerPointsDown;
        public Hud Hud;
        private bool _active = true;
        private float _delay;
        private float _resetTime = 1.5f;
        private bool _isMenuVisible;

        public bool IsMenuVisible
        {
            get { return _isMenuVisible; }
            set
            {
                _isMenuVisible = value;
                if (value)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                }
                ResumeButton.gameObject.SetActive(!End);
                Menu.gameObject.SetActive(IsMenuVisible);
            }
        }

        public void InitGame(GameMode mode, int aiLevel)
        {
            var body = Puck.GetComponent<Rigidbody2D>();

            Mode = mode;
            End = false;
            Active = false;
            _delay = _resetTime - .5f;
            Puck.GetComponent<Rigidbody2D>().position = Vector2.down;
            Puck.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            PlayerPointsDown.Count = 0;
            PlayerPointsUp.Count = 0;
            Hud.Reset();

            switch (Mode)
            {
                case GameMode.AiVsAi:
                    PlayerUp.PlayerColor = Player.Color.Red;
                    PlayerDown.PlayerColor = Player.Color.Blue;
                    PlayerUp.Controller = new AiController { Puck = body, AiLevel = 8 };
                    PlayerDown.Controller = new AiController { Puck = body, AiLevel = 8, Direction = AiController.PlayingDirection.Up };
                    break;
                case GameMode.PlayerVsAi:
                    PlayerUp.PlayerColor = Player.Color.Red;
                    PlayerDown.PlayerColor = Player.Color.Blue;
                    PlayerUp.Controller = new AiController { Puck = body, AiLevel = aiLevel};
                    PlayerDown.Controller = new PlayerController();
                    break;
                case GameMode.Server:
                    // TODO
                    PlayerUp.PlayerColor = Player.Color.Red;
                    PlayerDown.PlayerColor = Player.Color.Blue;
                    break;
                case GameMode.Client:
                    // TODO
                    PlayerUp.PlayerColor = Player.Color.Blue;
                    PlayerDown.PlayerColor = Player.Color.Red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Use this for initialization
        void Start()
        {
            End = true;
            IsMenuVisible = true;
        }

        private void FixedUpdate()
        {
            if (End)
                return;

            if (Active)
            {
                var body = Puck.GetComponent<Rigidbody2D>();
                if (body.position.y > 5.2)
                    DownScored();
                else if (body.position.y < -5.2)
                    UpScored();
            }
            else
            {
                _delay += Time.fixedDeltaTime;
                if (_delay >= _resetTime)
                {
                    _delay = 0;
                    Hud.Reset();
                    Active = true;
                }
            }
        }

        void Update()
        {
            if (Input.GetKeyUp("escape"))
            {
                if (End && IsMenuVisible)
                    Application.Quit();

                if (End && !IsMenuVisible)
                    IsMenuVisible = true;
                else if (!End)
                    IsMenuVisible = !IsMenuVisible;
            }
        }

        private void UpScored()
        {
            Active = false;
            Stadium.Play();
            Debug.Log("Red scores!"); // TODO
            PlayerPointsUp.Count++;
            if (PlayerPointsUp.Count == 9)
            {
                if (Mode == GameMode.Client)
                    Hud.AnnounceBlueWins();
                else
                    Hud.AnnounceRedWins();
                End = true;
            } else
                Hud.AnnounceGoal();
            iTween.MoveTo(Puck, Vector3.down, _resetTime);
        }

        private void DownScored()
        {
            Active = false;
            Stadium.Play();
            Debug.Log("Blue scores!"); // TODO
            PlayerPointsDown.Count++;
            if (PlayerPointsDown.Count == 9)
            {
                if (Mode == GameMode.Client)
                    Hud.AnnounceRedWins();
                else
                    Hud.AnnounceBlueWins();
                End = true;
            }
            else
                Hud.AnnounceGoal();
            iTween.MoveTo(Puck, Vector3.up, _resetTime);
        }

        public void StartGameAivsAi()
        {
            InitGame(GameMode.AiVsAi, 7);
            IsMenuVisible = false;
            GetComponent<AudioSource>().Play();
        }

        public void StartGameEasy()
        {
            InitGame(GameMode.PlayerVsAi, 5);
            IsMenuVisible = false;
            GetComponent<AudioSource>().Play();
        }

        public void StartGameNormal()
        {
            InitGame(GameMode.PlayerVsAi, 6);
            IsMenuVisible = false;
            GetComponent<AudioSource>().Play();
        }

        public void StartGameHard()
        {
            InitGame(GameMode.PlayerVsAi, 7);
            IsMenuVisible = false;
            GetComponent<AudioSource>().Play();
        }

        public void Resume()
        {
            IsMenuVisible = false;
        }
    }
}
