using UnityEngine;
using System.Collections;

public class ReplaySystem : MonoBehaviour {
	private GameManager gameManager;
	private const int bufferFrames = 1000;
	private MyKeyFrame[] keyFrames = new MyKeyFrame[bufferFrames];
	private Rigidbody rigidBody;

	private bool firstPlayback;	// first time the replay starts or 2nd(etc) loop
	private int framesAvailable;	// Available frames for replay
	private bool notEnoughFrames; // less frames than bufferFrames?
	private int frameNumber;	// Number of current frame

	private MyKeyFrame crrntState;	// State(pos/rot) before Playback
	private Vector3 crrntVelocity;	// velocity before Playback
	private Vector3 crrntAngularVelocity; // angularVelocity before Playback

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		gameManager = GameObject.FindObjectOfType<GameManager>();
		Time.captureFramerate = 60;
	}
	
	// Update is called once per frame
	void Update () {
		frameNumber++;
		if (gameManager.recording){
			Record ();
		} else {
			if (!firstPlayback && framesAvailable != 0){
				while (frameNumber > framesAvailable){ 
					frameNumber -= framesAvailable;
				}
			}
			PlayBack();
		}
	}

	void PlayBack(){
		int frame = frameNumber % bufferFrames;
		if (firstPlayback){
			// First Playback loop
			if (frameNumber < bufferFrames){
				framesAvailable = frameNumber;
				firstPlayback=false;
				notEnoughFrames = true;
				SaveState();
			}
			rigidBody.isKinematic = true;
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
		int frame = frameNumber % bufferFrames;
		keyFrames [frame] = new MyKeyFrame (Time.time, transform.position, transform.rotation);
	}

	void Reset(){
		if (!firstPlayback){
			// Starting recording again, resetting stuff
			frameNumber = 0;
			keyFrames = new MyKeyFrame[bufferFrames];
			rigidBody.isKinematic = false;
			LoadState();
		}
		firstPlayback = true;
		notEnoughFrames = false;
	}

	void SaveState(){
		crrntState = new MyKeyFrame(Time.time, transform.position, transform.rotation);
		crrntVelocity = rigidBody.velocity;
		crrntAngularVelocity = rigidBody.angularVelocity;
		Debug.Log("V: " + crrntVelocity + ", AV: " + crrntAngularVelocity);
	}

	void LoadState(){
		transform.position = crrntState.pos;
		transform.rotation = crrntState.rot;
		rigidBody.velocity = crrntVelocity;
		rigidBody.angularVelocity = crrntAngularVelocity;
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