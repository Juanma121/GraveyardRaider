using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Warp : MonoBehaviour {

	public GameObject target;

	public GameObject targetMap;

	//Para controlar si empieza o no la transicion
	bool start = false;
	//Para controlar si la transicion es de entrada o de salida
	bool isFadeIn = false;
	//Opacidad inicial del cuadrado de transicion
	float alpha = 0;
	//TRansicion de 1 segundo
	float fadeTime = 1f;


	void Awake(){
		Assert.IsNotNull (target);

		GetComponent<SpriteRenderer>().enabled = false;
		transform.GetChild (0).GetComponent<SpriteRenderer> ().enabled = false;

		Assert.IsNotNull (targetMap);
	}

	IEnumerator OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player" && other.GetComponent<Inventory>().getLlave()) {

			other.GetComponent<Animator>().enabled =false;
			other.GetComponent<PlayerMovement>().enabled =false;
			FadeIn ();

			yield return new WaitForSeconds (fadeTime);

				
			other.transform.position = target.transform.GetChild(0).transform.position;
			Camera.main.GetComponent<SeguirPersonaje> ().SetBound (targetMap);
		

			FadeOut ();
			other.GetComponent<Animator>().enabled =true;
			other.GetComponent<PlayerMovement>().enabled =true;
		}
	}

	//Dibujamos un cuadrado con opacidad encima de la pantalla simulando la transicion
	void OnGUI(){
		//Si no empieza la transición salimos del evento directamente
		if(!start){
			return;
		}
		//Si ha empezado creamos un color con una opacidad inicial a 0
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);

		//Creamos una textura temporal para rellenar la pantalla
		Texture2D tex;
		tex = new Texture2D (1,1);
		tex.SetPixel (0,0, Color.black);
		tex.Apply ();

		//Dibujamos la textura sobre la pantalla
		GUI.DrawTexture(new Rect (0, 0, Screen.width, Screen.height),tex);

		//Controlamos la transparencia
		if (isFadeIn) {
			//Si es la de aparecer le sumamos la opacidad
			alpha = Mathf.Lerp (alpha, 1.1f, fadeTime * Time.deltaTime);
		} else {
			//Si es la de desaparecer le restamos la opacidad
			alpha = Mathf.Lerp (alpha, -0.1f, fadeTime * Time.deltaTime);
			//Si la opacidad llega a 0 desactivamos la transicion
			if (alpha < 0)
				start = false;
		}
	}

	//Método para activar la transicion de entrada
	void FadeIn(){
		start = true;
		isFadeIn = true;
	}

	//Método para activar la transicion de salida
	void FadeOut(){
		isFadeIn = false;
	}
}
