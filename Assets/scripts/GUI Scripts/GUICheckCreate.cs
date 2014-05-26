using UnityEngine;
using System.Collections;

public class GUICheckCreate : MonoBehaviour {

	public GUIMaster GUIMaster;

	void Awake () {
		GUIMaster.GUIMasterCheckCreate(GUIMaster);
	}

}
