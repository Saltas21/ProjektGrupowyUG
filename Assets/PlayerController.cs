using UnityEngine;

namespace Assets
{
    public class PlayerController : Player
    {
        // Update is called once per frame
        private void Update()
        {
            Vector2 m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GoTo(m);
        }
    }
}

