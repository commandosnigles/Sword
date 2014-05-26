using UnityEngine;
using System.Collections;
//Inherit DamageOnCollision members
public class DamageOnTrigger : DamageOnCollision {
	//use trigger to call applyDamage instead
	void OnTriggerEnter(Collider col){
		Vector3 knockback = Vector3.Scale(col.transform.position - this.transform.position, new Vector3 (1,0,1)).normalized;
		knockback.y = 1;
		applyDamage(col.gameObject, knockback);
	}

}
