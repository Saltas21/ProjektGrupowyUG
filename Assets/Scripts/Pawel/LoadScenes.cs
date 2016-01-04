using UnityEngine;
using System.Collections;

public class LoadScenes : MonoBehaviour{

	public void OnLoadMenuScene(){
		Application.LoadLevel(0);
	}
	public void OnLoadSingleScene(){
		Application.LoadLevel(1);
	}
	public void OnLoadMultiScene(){
		Application.LoadLevel(2);
	}
}
