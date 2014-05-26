using UnityEngine;
using System.Collections;

public class SwordSpawnPedestal : MonoBehaviour {
	[HideInInspector]
	public static SwordSpawnPedestal[] Pedestals;

	public float spawnTime = 1f;
	public GameObject childPedestal;
	public Texture activateText;
	public Texture deactivateText;
	
	private Renderer pedestalRen;
	private bool summoning = false;
	private bool summoned = false;
	private bool pedestalActive = false;
	private float timeHeld = 0f;
	private SwordControls player;
	private Sword swordPrefab;
	private CharacterAnimations animator;


	void Start () {
		player = (SwordControls)GameObject.FindGameObjectWithTag("Player").GetComponent<SwordControls>();
		animator = player.gameObject.GetComponentInChildren<CharacterAnimations>();
		swordPrefab = player.SwordPrefab[0];
		//create a list of pedestals in the level
		Pedestals = (SwordSpawnPedestal[]) FindObjectsOfType (typeof(SwordSpawnPedestal));
		pedestalRen = childPedestal.renderer;
	}

	void Update () {

		//hold the button to respawn the sword at the pedestal
		if (Input.GetButton("Summon") && !player.getHasSword && pedestalActive && !summoned){
			if (timeHeld == 0 && !player.GetComponent<TDS_Controller>().GetFrozen){
				summoning = true;
				animator.StartSummon();
			}
			if (timeHeld < spawnTime && summoning) {
				timeHeld += Time.deltaTime;
			} else timeHeld = 0f;
		}
		
		if (Input.GetButtonUp ("Summon")){
			if (summoning) {
				timeHeld = 0f;
				animator.EndSummon();
				summoning = false;
			}
			summoned = false;
		}	
		if (timeHeld >= spawnTime) {
			RespawnSword();
			animator.EndSummon();
			summoned = true;
			summoning = false;
			timeHeld = 0f;
		}
		
	}

	//when triggered by player, disable all other pedestals and enable this one.
	void OnTriggerEnter(Collider col){
		if(col.tag == "Player" && !pedestalActive){
			foreach (SwordSpawnPedestal pedestal in Pedestals){
				pedestal.DisablePedestal();
			}
			EnablePedestal();
		}
	}

	void DisablePedestal(){
		pedestalActive = false;
		timeHeld = 0f;
		this.gameObject.GetComponentInChildren<Renderer>().material.mainTexture = deactivateText;
	}

	void EnablePedestal(){
		pedestalActive = true;
		this.gameObject.GetComponentInChildren<Renderer>().material.mainTexture = activateText;
	}

	public void RespawnSword(){
		if (pedestalActive){
			player.DestroySwords ();
			Sword sword = Instantiate(swordPrefab,transform.position + Vector3.up * 1.1f, Quaternion.Euler(90,0,0)) as Sword;
			sword.IgnorePlayer(false);
		}
	}

}
