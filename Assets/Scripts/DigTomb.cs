using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigTomb : MonoBehaviour {

	//Variable para controlar el tiempo que el personaje estara cavando sin poder moverse
	public float DiggingTime = 5;

	//La usaremos para controlar cuando esta dentro y cuando esta fuera del collider
	bool triggerEntered;

	//Animador para controlar la animacion tumba
	Animator anim;

	//Collider de tumba
	Collider2D TombCollider;

	//Variable para guardar al jugador
	GameObject player;

	public GameObject picoPrefab;
	public GameObject escopetaPrefab;
	public GameObject silbatoPrefab;
	public GameObject llavePrefab;
	public GameObject oroPrefab;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		TombCollider = GetComponent<Collider2D>();
		triggerEntered = false;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") { //Evalua la expresion en cuanto colisiona por lo tanto hay que presionar Q antes de tocar la tumba
			triggerEntered = true;
			player = GameObject.FindGameObjectWithTag("Player");
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			//Aqui reseteamos la variable
			triggerEntered = false;
			//Desvinculamos el jugador
			player = null;
		}
	}


	void SacaObjecto(){

		//Variables para randomizar el objeto
		float probabilidad = 0f;
		if (this.tag == "Tumba_normal") { //Si es una tumba normal sacamos uno de los 3 objetos posibles
			probabilidad = Mathf.Round (Random.Range (1f, 6f));
		} else if(this.tag == "Tumba_hormigon"){ //Si es una tumba de hormigon sacamos la llave
			probabilidad = 7f;
		}
		int iprobabilidad = (int)probabilidad;

		//Coordenadas donde se crea el objeto
		Vector3 posicion = new Vector3(transform.position.x + 0.3f, transform.position.y + 1.1f, transform.position.z);

		switch (iprobabilidad) {
		case 1://Si es 1 sacamos un pico

			//Creamos la instancia del pico
			GameObject picoNuevo = Instantiate(picoPrefab,posicion,Quaternion.identity);

			Debug.Log("Pico");
			break;
		case 2://Si es 2 sacamos una escopeta

			//Creamos la instancia de la escopeta
			GameObject escopetaNueva = Instantiate(escopetaPrefab,posicion,Quaternion.identity);

			Debug.Log("Escopeta");
			break;
		case 3://Si es 3 sacamos silbato

			//Creamos la instancia del silbato
			GameObject silbatoNuevo = Instantiate(silbatoPrefab,posicion,Quaternion.identity);

			Debug.Log("Silbato");
			break;
		case 4: // Si es 4 no sacamos nada

			Debug.Log("Nada");
			break;
		case 7: // Si es 7 sacamos la llave

			//Creamos la instancia de la llave
			GameObject llaveNueva = Instantiate (llavePrefab, posicion, Quaternion.identity);
			Debug.Log("Llave");
			break;
		default:// Si es cualquier rango enrte 5 y 6

			//Creamos la instacia del oro
			GameObject oroNuevo = Instantiate (oroPrefab, posicion, Quaternion.identity);
			Debug.Log("Oro");
			break;
		}
	}

	// Update is called once per frame
	void Update () { 
		StartCoroutine(AccionCavar());
	}


	IEnumerator AccionCavar(){
		if (Input.GetKey ("q") && triggerEntered == true) { //Evalua la expresion en cuanto colisiona por lo tanto hay que presionar Q antes de tocar la tumba

			//yield return new WaitUntil (() => (Input.GetKey ("q")) == true); //No me sirve, se queda bloqueado hasta que pulso Q aunque me salga de la zona

			//Si es de hormigon y no tengo el pico me salgo
			if (this.tag == "Tumba_hormigon" && !player.GetComponent<Inventory> ().getPico ())
				yield break;


			//Espero los segundos que tarda en empezar a cavar
			yield return new WaitForSeconds (.2f);

			//Desactivo el movimiento del jugador
			player.GetComponent<PlayerMovement>().enabled =false;

			//Muevo al jugador a una posicion centrada al cavar tumba
			Vector3 reposicionamiento = new Vector3(transform.position.x, transform.position.y -0.3f, transform.position.z);
			player.transform.position = reposicionamiento;

			//Animación de cavar
			player.GetComponent<Animator>().Play("Player_Dig");

			//Espero los segundos que tarda en cavar
			yield return new WaitForSeconds (DiggingTime);

			//Abro la tumba dependiendo del tipo
			if (this.tag == "Tumba_normal") {
				anim.Play ("Tomb_R_open");
			} else if(this.tag == "Tumba_hormigon") {
				anim.Play ("Tomb_H_open");
			}

			//Desactivo animacion de cavar ejecutando la de Idle
			player.GetComponent<Animator>().Play("Player_Idle");

			//Activo el movimiento del jugador
			player.GetComponent<PlayerMovement>().enabled =true;

			//Desactivo el collider para que no se pueda volver a cavar
			TombCollider.enabled = false;

			//Hacemos aparecer un objeto aleatorio de la tumba
			SacaObjecto ();

		}
	}
}
