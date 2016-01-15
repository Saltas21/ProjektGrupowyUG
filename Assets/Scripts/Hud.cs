using UnityEngine;

namespace Assets.Scripts
{
    public class Hud : MonoBehaviour
    {
        public SpriteRenderer Goal;
        public SpriteRenderer RedWins;
        public SpriteRenderer BlueWins;
        private float _delay;
        private bool _active;

        // Use this for initialization
        void Start ()
        {
            Reset();
        }
	
        // Update is called once per frame
        void Update () {
        }

        public void Reset()
        {
            Goal.enabled = false;
            RedWins.enabled = false;
            BlueWins.enabled = false;
        }

        public void AnnounceGoal()
        {
            Goal.enabled = true;
        }

        public void AnnounceRedWins()
        {
            RedWins.enabled = true;
        }

        public void AnnounceBlueWins()
        {
            BlueWins.enabled = true;
        }
    }
}
