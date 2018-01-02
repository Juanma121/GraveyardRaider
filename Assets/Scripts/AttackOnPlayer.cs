using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnPlayer : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col){
		// Restamos uno de vida si es un enemigo
		if(col.tag == "Player"){
			col.SendMessage ("Attacked");
		}
	}
}