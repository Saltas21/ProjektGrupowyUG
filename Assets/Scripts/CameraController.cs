using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public float Width;

        // Use this for initialization
        void Start ()
        {
            //Cursor.visible = false;
        }
	
        // Update is called once per frame
        void Update () {
            if (Screen.width < Screen.height)
            {
                Camera.main.orthographicSize = (float) (Width*Screen.height/Screen.width*0.5);
            }
        }
    }
}

