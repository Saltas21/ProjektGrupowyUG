using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Assets
{
	public class InitPuck : NetworkBehaviour {
		public Puck puck;
		public CircleCollider2D collider;
		void Start(){
			if(!isServer){
				puck.enabled = false;
				collider.enabled = false;
			//	gameObject.transform.localRotation = Quaternion.Euler(0,0,180);
			}
		}
	}
}
