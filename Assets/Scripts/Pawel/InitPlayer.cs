using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Assets
{
	public class InitPlayer : NetworkBehaviour {
		public Player playerR;

		void Start(){
			if(isLocalPlayer){
				GameMulti.Instance.PlayerRed = playerR;
				GameMulti.Instance.PlayerRed.collider.enabled = false;
				GameMulti.Instance.PlayerRed.sprite.enabled = false;
			}
		}
	}
}
