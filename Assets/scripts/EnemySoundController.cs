using UnityEngine;
using System.Collections;
using RAIN.Core;

public class EnemySoundController : MonoBehaviour {

	public AudioClip WalkSound;
	public AudioClip AttackSound;
	public AudioClip DetactSound;

	private string _currentState = "";
	private string _previousState = "";
	private string _detectObj = "";
	private AIRig aiRig = null;
	private AudioSource _audio;
	// Use this for initialization
	void Start () {
		aiRig = gameObject.GetComponentInChildren<AIRig>();
		_audio = gameObject.GetComponent<AudioSource>();
		_detectObj = aiRig.AI.WorkingMemory.GetItem<string>("detectTarget");
		_currentState = aiRig.AI.WorkingMemory.GetItem<string>("varstate");
		_previousState = "Walk";

	}
	
	// Update is called once per frame
	void Update () {
		_detectObj = aiRig.AI.WorkingMemory.GetItem<string>("detectTarget");
		_currentState = aiRig.AI.WorkingMemory.GetItem<string>("varState");

		if(_currentState != _previousState){
			_previousState = _currentState;
			ChangeSound();
			Debug.Log("test1");
			if(_detectObj!="" && _currentState == "Run"){
				_previousState = _currentState;
				ChangeSound();
				Debug.Log("test2");
			}
		}
	}
	
	void ChangeSound(){
		if(_currentState == "Walk"){
			_audio.clip = WalkSound;
			_audio.loop = true;
			_audio.priority = 100;
			_audio.pitch = 1;
			_audio.Play();
			Debug.Log("test5");
		}else if(_currentState == "Run"){
			_audio.clip = WalkSound;
			_audio.pitch = 3;
			_audio.priority = 80;
			_audio.loop = true;
			_audio.Play();
			Debug.Log("test7");
		}
		if(_detectObj != "" && _currentState == "Run"){
			_audio.clip = DetactSound;
			_audio.loop = false;
			_audio.pitch = 1;
			_audio.priority = 10;
			_audio.Play();
			Debug.Log("test6");
		}
	}
}
