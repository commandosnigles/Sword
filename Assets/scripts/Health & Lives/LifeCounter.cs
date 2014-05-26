using UnityEngine;
using System.Collections;

public class LifeCounter : MonoBehaviour {
	
	public static LifeCounter instance;

	public int StartingLives = 3;
	private int lifeCount;
	public int GetLifeCount {
		get{return lifeCount;}
	}

	void Awake () {
		instance = this;
		ResetLives();
	}

	public void IncrementLife() {
		lifeCount++;
	}

	public void DecrementLife() {
		lifeCount--;
	}

	public void ResetLives() {
		lifeCount = StartingLives;
	}
}
