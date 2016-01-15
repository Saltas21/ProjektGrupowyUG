using UnityEngine;

namespace Assets.Scripts
{
    public class Points : MonoBehaviour
    {
        public Sprite Num0;
        public Sprite Num1;
        public Sprite Num2;
        public Sprite Num3;
        public Sprite Num4;
        public Sprite Num5;
        public Sprite Num6;
        public Sprite Num7;
        public Sprite Num8;
        public Sprite Num9;

        private Sprite[] _numbers;
        private int _count;

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                GetComponent<SpriteRenderer>().sprite = _numbers[Mathf.Abs(_count % 10)];
            }
        }

        // Use this for initialization
        void Start ()
        {
            _numbers = new Sprite[] { Num0, Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9 };
        }
	
        // Update is called once per frame
        void Update () {
        }
    }
}
