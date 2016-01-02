using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	public void OnStartServer(){
		NetworkConnectionError error = Initialize_Server(1234,2,"Multi", "Nazwa gry");
		if(error != NetworkConnectionError.NoError){
			Debug.LogError("Nie utworzono serwera");
		}
	}
	public void OnJoinToServer(){
		NetworkConnectionError error = ConnectToServer(1234, "127.0.0.1");
		if(error != NetworkConnectionError.NoError){
			Debug.LogError("Nie nawiązano połączenia");
		}

	}

	public static NetworkConnectionError Initialize_Server (int Port, int MaxPlayer, string gameType, string gameName) {
		Network.isMessageQueueRunning = true;
		bool useNat = !Network.HavePublicAddress();
		NetworkConnectionError error = Network.InitializeServer(MaxPlayer-1, Port, useNat);
		if (error == NetworkConnectionError.NoError){
			Network.minimumAllocatableViewIDs = 200;
			Debug.Log("Run");
			MyAwake();
		}
		return error;
	}
	
	public static NetworkConnectionError ConnectToServer(int Port, string IP){
		Network.isMessageQueueRunning = true;
		NetworkConnectionError error = Network.Connect(IP, Port);
		if (error == NetworkConnectionError.NoError){
			Debug.Log("Connected");
			MyAwake();

		}
		return error;
	}

	static void MyAwake(){
		if(Network.isServer){
			Debug.Log("MyAwake");
		}
	}

	void OnConnectedToServer(){
		Debug.Log("OnConnectedToServer");

	}
	
	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("OnFailedToConnect");
	}
	
	void OnPlayerDisconnected(NetworkPlayer player){
		Debug.Log("OnPlayerDisconnected");

	}

	void OnDisconnectedFromServer(NetworkDisconnection info){
		Debug.Log("OnDisconnectedFromServer");

	}

}
