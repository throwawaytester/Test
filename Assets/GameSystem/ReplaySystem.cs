using UnityEngine;
using System.Collections;

public class ReplaySystem : MonoBehaviour {
	private GameManager gameManager;
	private const int bufferFrames = 1000;
	private MyKeyFrame[] keyFrames = new MyKeyFrame[bufferFrames];
	private Rigidbody rigidBody;

	public bool firstPlayback;
	public int framesAvailable;
	public bool notEnoughFrames;
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		gameManager = GameObject.FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameManager.recording){
			Record ();
		} else {
			PlayBack();
		}
	}

	void PlayBack(){
		rigidBody.isKinematic = true;
		int frame = Time.frameCount % bufferFrames;

		if (Time.frameCount < bufferFrames && firstPlayback){
			framesAvailable = Time.frameCount;
			firstPlayback=false;
			notEnoughFrames = true;
		}
		if (notEnoughFrames){
			frame = frame % framesAvailable;
		}

		transform.position = keyFrames[frame].pos;
		transform.rotation = keyFrames[frame].rot;
	}

	void Record ()
	{
		Reset(); 
		rigidBody.isKinematic = false;
		int frame = Time.frameCount % bufferFrames;
		keyFrames [frame] = new MyKeyFrame (Time.time, transform.position, transform.rotation);
	}

	void Reset(){
		firstPlayback = true;
		notEnoughFrames = false;
	}
}

/// <summary>
/// A structure for storing time, rotation and position
/// </summary>
public struct MyKeyFrame {
	public float time;
	public Vector3 pos;
	public Quaternion rot;

	public MyKeyFrame(float time, Vector3 pos, Quaternion rot){
		this.time = time;
		this.pos = pos;
		this.rot = rot;
	}
			
}