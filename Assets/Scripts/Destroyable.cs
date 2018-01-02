using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {

	//Variable para guardar el nombre del estado de la destruccion
	public string destroyState;
	//Variable con los segundos a esperar antes de desactivar la colision
	public float timeForDisable;

	//Animador para controlar la animacion
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	//Detectamos la colision con una corrutina
	IEnumerator OnTriggerEnter2D (Collider2D col){
		//Si es un ataque
		if(col.tag == "Attack"){
			//Reproducimos la animacion de destruccion y esperamos
			anim.Play(destroyState);
			yield return new WaitForSeconds (timeForDisable);

			//Pasados los segundos de espera desactivamos los colliders 2D
			foreach(Collider2D c in GetComponents<Collider2D>()){
				c.enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Destruir el objeto al finalizar la animacion de destruccion
		//El estado debe tener el atributo 'loop' a false para no repeirse
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

		if(stateInfo.IsName(destroyState) && stateInfo.normalizedTime >= 1){
			Destroy (gameObject);
			//En el futuro podriamos almacenar la instancia y su transform
			//para crearlos de nuevo despues de un tiempo
		}
	}
}
