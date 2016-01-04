using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Assets
{
	public class InitClient : NetworkBehaviour {
		public Player player;
		void Start(){
			gameObject.transform.position = new Vector3(0,3,0);
			if(isServer && isLocalPlayer || !isServer && !isLocalPlayer){
				gameObject.SetActive(false);
			}
			else{
				GameMulti.Instance.PlayerRed = player;
			}
		}
	}
}
