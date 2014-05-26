using UnityEngine;
using System.Collections;

public class DamageOnCollision : MonoBehaviour {

	public bool DamagesPlayer = true;
	public bool DamagesEnemy = true;

	public int DamageOutput = 1;

	void OnCollisionEnter(Collision col){
		//Vector3 knockback = this.transform.forward;	
		//Vector3 knockback = col.contacts[0].normal;	
		Vector3 knockback = Vector3.Scale(col.transform.position - this.transform.position, new Vector3 (1,0,1)).normalized;
		knockback.y = 1;
		applyDamage(col.gameObject, knockback);
	}

	public void applyDamage(GameObject victim, Vector3 knockback){
		Debug.Log(victim.tag + " hit!");
		if (victim.GetComponent<HealthCounter>() != null){

			if (victim.tag == "Player" && DamagesPlayer)
				victim.GetComponent<HealthCounter>().TakeDamage (DamageOutput, knockback);
			else if (victim.tag == "Enemy" && DamagesEnemy)
				victim.GetComponent<HealthCounter>().TakeDamage (DamageOutput, knockback);
		
		}
	}	
}
