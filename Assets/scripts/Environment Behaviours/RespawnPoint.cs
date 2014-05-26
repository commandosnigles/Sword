using UnityEngine;
using System.Collections;

public class RespawnPoint : MonoBehaviour {
	public static RespawnPoint[] Respawns;
	private bool spawnActive = false;
	// Use this for initialization
	void Start () {
		Respawns = (RespawnPoint[]) FindObjectsOfType (typeof(RespawnPoint));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider col){
		if(col.tag == "Player" && !spawnActive){
			foreach (RespawnPoint respawn in Respawns){
				respawn.DisableSpawn();
			}
			EnableSpawn();
		}
	}

	void DisableSpawn(){
		spawnActive = false;
		renderer.material.SetColor("_Color",new Color(0.1882f,0.165f,0.384f,1f));
	}
	
	void EnableSpawn(){
		spawnActive = true;
		renderer.material.SetColor("_Color", Color.yellow);
	}

}
