using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour {


	void OnTriggerEnter2D(Collider2D other){


		if (other.tag == "Player") {
			
			//Incluimos los objetos en el inventario del jugador
			switch (gameObject.tag) {
			case "Pico":
				other.GetComponent<Inventory> ().setPico (true);
				break;
			case "Escopeta":
				other.GetComponent<Inventory> ().setEscopeta (true);
				break;
			case "Silbato":
				other.GetComponent<Inventory> ().setSilbato (true);
				break;
			case "Llave":
				other.GetComponent<Inventory> ().setLlave (true);
				break;

			case "Oro":
				//Tendremos que añadir la puntuacion al score
				break;
			default:
				break;
			}

			//Destruimos el objeto
			Destroy (gameObject);
		}
	}
		
}
