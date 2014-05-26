using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Action;

public class HealthCounter : MonoBehaviour {
	public float DeathPause = 2f;
	public float HitImmunity = 1;
	public int MaxHealth = 4;
	public GameObject TopLevel;
	public GameObject RenderedMesh;
	public Color ImmunityFlicker = Color.red;
	public float FlickerFrequency = 0.2f;
	public bool FlickerOnDeath = true;
	private float flickerTime = 0;
	private int currentHealth = 4;
	private bool immune = false;
	private Color initialColor;
	private Color initialShadow;
	
	public Texture2D faceTexture;
	public Texture2D healthBarBg;
	public Texture2D healthTexture;
	public Texture2D heart;
	private float healthbarlength;
	private int currentLifeCount;
	private AIRig ai;

	public int GetCurrentHealth {
		get{return currentHealth;}
	}

	void Start () {
		currentHealth = MaxHealth;
		if (TopLevel == null)
			TopLevel = this.gameObject;
		initialColor = RenderedMesh.renderer.material.color;
		initialShadow = RenderedMesh.renderer.material.GetColor ("_ShadowColor");
		
		if(gameObject.tag != "Player"){
			ai = this.gameObject.transform.parent.GetComponentInChildren<AIRig>();
			ai.AI.WorkingMemory.SetItem<float>("health",currentHealth);
		}
	}

	void Update () {
		if (immune) {
			flickerTime += Time.deltaTime;
			float phase = flickerTime/FlickerFrequency;
			float blend = Mathf.Sin (phase*2*Mathf.PI);
			RenderedMesh.renderer.material.color = Color.Lerp(initialColor, ImmunityFlicker, blend);
			RenderedMesh.renderer.material.SetColor ("_ShadowColor", Color.Lerp(initialShadow, ImmunityFlicker, blend));
		} 
		
		if(gameObject.tag != "Player"){
			ai.AI.WorkingMemory.SetItem("health",currentHealth);
		}
	}

	void OnGUI () {
		if(gameObject.tag != "Player")
			return;
		////////////////////////
		//Cindy: healthbar GUI
		int heartCounter = 0;
		float gap = Screen.height * 0.048f;
		//GUI.DrawTexture(new Rect(36, 20,(Screen.width/5) + 8, 28), healthBarBg);
		//healthbarlength = (Screen.width/5)*(currentHealth/(float)MaxHealth);
		//GUI.DrawTexture(new Rect(40, 24, healthbarlength , 20), healthTexture,ScaleMode.StretchToFill);
		//GUI.DrawTexture(new Rect(10, 9,50, 50), faceTexture);
		GUI.DrawTexture(new Rect(Screen.width * 0.038f, Screen.height * 0.027f,(Screen.width/5) + Screen.width * 0.009f, Screen.height * 0.038f), healthBarBg);
		healthbarlength = (Screen.width/5)*(currentHealth/(float)MaxHealth);
		GUI.DrawTexture(new Rect(Screen.width * 0.043f, Screen.height * 0.032f, healthbarlength , Screen.height * 0.027f), healthTexture,ScaleMode.StretchToFill);
		GUI.DrawTexture(new Rect(Screen.width * 0.011f,  Screen.height * 0.012f,Screen.width * 0.053f, Screen.height * 0.068f), faceTexture);

		/////////////

		currentLifeCount = LifeCounter.instance.GetLifeCount;

		while(heartCounter<currentLifeCount){
			GUI.DrawTexture(new Rect((Screen.width/5)+ Screen.width * 0.027f + gap, Screen.height * 0.025f ,Screen.width * 0.032f, Screen.height * 0.041f), heart);
			heartCounter++;
			gap = (heartCounter+1) * Screen.height * 0.048f;;

		}

//		string health = "Health: " + currentHealth.ToString();
//		GUI.depth = 2;
//		GUI.Label (new Rect(.1f,.1f,100f,20f), health); 
	}

	public void TakeDamage (int damageTaken, Vector3 knockback){
		StartCoroutine(CoTakeDamage(damageTaken, knockback));
	}

	public IEnumerator CoTakeDamage (int damageTaken, Vector3 knockback) {
		if (currentHealth > 0 && !immune) {
			Debug.Log(damageTaken.ToString () + " Damage Taken");
			immune = true;
			currentHealth = Mathf.Clamp(currentHealth - damageTaken, 0, MaxHealth);
			if (currentHealth <= 0){
				immune = FlickerOnDeath;
				StartCoroutine (Death ());
			}
			else{
				////////// add  TakeDamage Animation and knockback
				transform.Translate (knockback * damageTaken);
				yield return new WaitForSeconds(HitImmunity);
				immune = false;
				flickerTime = 0f;
				RenderedMesh.renderer.material.color = initialColor;
				RenderedMesh.renderer.material.SetColor ("_ShadowColor", initialShadow);
			}
		}
	}

	public void Heal (int healing) {
		if (currentHealth > 0)
			currentHealth = Mathf.Clamp (currentHealth + healing, 0, MaxHealth);
	}

	public void FullHealth() {
		currentHealth = MaxHealth;
	}

	public IEnumerator Death () {
		if(gameObject.tag == "Player"){
			gameObject.GetComponent<TDS_Controller>().SetFrozen = true;
			StartCoroutine (Fade_InOut.instance.FadeOut(3f));
			gameObject.GetComponentInChildren<CharacterAnimations>().Die();
		}
		////////////// add  play death animation
		yield return new WaitForSeconds(DeathPause);


		if(gameObject.tag == "Player"){

			LifeCounter.instance.DecrementLife();
			GUIPauseMenu.instance.Pause();
			if (LifeCounter.instance.GetLifeCount > 0)
				GUIMaster.instance.ActivateWindow (GUIDeath.instance);
			else
				GUIMaster.instance.ActivateWindow (GUIGameOver.instance);
			FullHealth();
			immune = false;
		}
		else
			Destroy (TopLevel);
	}
}
