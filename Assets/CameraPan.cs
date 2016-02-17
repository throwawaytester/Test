using UnityEngine;
using System.Collections;

public class CameraPan : MonoBehaviour {

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//transform.position = player.transform.position + Vector3.right * 4;
		transform.LookAt(player.transform);
	}
}
