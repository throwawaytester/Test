using UnityEngine;
using System.Collections;

public class ReplaySystem : MonoBehaviour {

	private const int bufferFrames = 100;
	private MyKeyFrame[] keyFrames = new MyKeyFrame[bufferFrames];
	private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		Record ();
	}

	void PlayBack(){
		rigidBody.isKinematic = true;
		int frame = Time.frameCount % bufferFrames;
		transform.position = keyFrames[frame].pos;
		transform.rotation = keyFrames[frame].rot;
	}

	void Record ()
	{
		rigidBody.isKinematic = false;
		int frame = Time.frameCount % bufferFrames;
		keyFrames [frame] = new MyKeyFrame (Time.time, transform.position, transform.rotation);
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