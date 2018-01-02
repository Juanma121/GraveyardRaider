using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	//Variables para gestionar el radio de vision, el de ataque y la velocidad
	public float visionRadius;
	public float attackRadius;
	public float speed;

	//Variables realacionadas con la vida del enemigo
	public int maxHp =1;
	public int hp;

	//Pasamos el prefab del zombie simple
	public GameObject zombieSPrefab;

	//Variable para guardar al jugador
	GameObject player;

	//Variable para guardar la posicion inicial
	Vector3 initialPosition;

	// Para recoger el collider de ataque
	CircleCollider2D attackCollider;

	//Variable para la espera entre ataque y ataque
	public float attackSpeed;
	bool attacking = false;

	//Para el vector de ataque
	Vector2 mov;

	//Animador y cuerpo cinematico con la rotacion en Z congelada
	Animator anim;
	Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {

		//Recuperamos al jugador gracias al Tag
		player = GameObject.FindGameObjectWithTag("Player");

		//Guardamos nuestra posicion inicial
		initialPosition = transform.position;

		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();

		//Inicializamos vida
		hp = maxHp;

		// Recuperamos el collider de ataque del primer hijo
		attackCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
		//Lo desactivamos al principio para que solo funcione si el enemigo ataca
		attackCollider.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {

		//Por defecto nuestro target siempre sera nuestra posicion inical
		//Vector3 target = initialPosition;
		//Por defecto nuestro target siempre sera nuestro jugador
		Vector3 target = player.transform.position;

		//Comprobamos un Raycast del enemigo hasta el jugador
		RaycastHit2D hit = Physics2D.Raycast(
			transform.position,
			player.transform.position - transform.position,
			visionRadius,
			1 << LayerMask.NameToLayer("Default")
			// Poner el propio Enemy en una layer distinta a Default para evitar el raycast
			//Tambien poner al objeto Attack una Layer Attack
			//Sino lo detectara como entorno y se mueve atras al hacer ataques
		);

		//Aqui podemos debuguear el Raycast
		Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
		Debug.DrawRay (transform.position, forward, Color.red);

		//Si el Raycast encuenta al jugador lo ponemos de target
		if (hit.collider != null) {
			if (hit.collider.tag == "Player") {
				target = player.transform.position;
			}
		}

		//Calculamos la distancia y direccion actual hasta el target
		float distance = Vector3.Distance(target, transform.position);
		Vector3 dir = (target - transform.position).normalized;

		//Si es el jugador y esta en rango de ataque nos paramos y le atacamos
		if(target != initialPosition && distance - 0.1 < attackRadius && hp > 0){
			
			//Indicamos la direccion en la que tiene que mirar
			anim.SetFloat("movX", dir.x);
			anim.SetFloat ("movY", dir.y);

			//Paramos la animacion de andar
			anim.SetBool("walking", false);
			rb2d.MovePosition(transform.position + dir * 0 * Time.deltaTime); 
			//anim.Play ("Enemy_Walk", -1, 0); //Congela la animacion de andar

			//Emepezamos a atacar (importante una layer en ataque para evitar Raycast)
			if (!attacking)
				StartCoroutine (Attack(attackSpeed, dir));
						
		}else if(hp > 0){
			//En caso contrario nos movemos hacia el
			rb2d.MovePosition(transform.position + dir * speed * Time.deltaTime);

			//Al movernos establecemos la animación de movimiento
			anim.speed = 1;
			anim.SetFloat ("movX", dir.x);
			anim.SetFloat ("movY", dir.y);
			anim.SetBool("walking", true);
		}

		//Una ultima comprobacion para evitar bugs forzando la posicion inicial
		if(target == initialPosition && distance < 0.02f){
			transform.position = initialPosition;
			// Y cambiamos la animacion de nuevo a idle
			anim.SetBool("walking", false);
		}
		//Y un debug optativo con una linea hasta el target
		Debug.DrawLine(transform.position, target, Color.green);

	}

	// Podemos dibujar el radio de vision y ataque sobre la escena dibujando una esfera
	void OnDrawGizmosSelected(){

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, visionRadius);
		Gizmos.DrawWireSphere (transform.position, attackRadius);
	}

	public IEnumerator Attacked(){
		if (--hp <= 0) { // Le restamos 1 vida al bicho y a la vez comprobamos la condicion

			anim.Play("Enemy_Idle");

			anim.SetBool("dies",true);
			//yield return new WaitForSeconds (anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime - 0.1f);
			yield return new WaitForSeconds (35 * Time.deltaTime);
			Destroy (gameObject);

			/*gameObject.GetComponent<Renderer>().enabled = false;

			//Lo vuelvo a crear en us posicion original despues de esperar un tiempo
			yield return new WaitForSeconds(2);
			gameObject.GetComponent<Renderer>().enabled = true;
			Destroy (gameObject);
			GameObject zombieNuevo = Instantiate (zombieSPrefab, initialPosition, Quaternion.identity);
			//gameObject.GetComponent<Renderer>().enabled = true;*/
		}
	}

	IEnumerator Attack(float seconds, Vector3 movimiento){
		
		yield return new WaitForSeconds (seconds); //Espera unos segundos

			bool attacking = true;

			// vector para el ataque
			mov = new Vector2 (movimiento.x, movimiento.y);

			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
			attacking = stateInfo.IsName ("Enemy_Attack");


			anim.SetTrigger ("attacking");
			Debug.Log ("ataque enemigo");


			//Vamos actualizando la posición de la colision de ataque
			if (mov != Vector2.zero)
				attackCollider.offset = new Vector2 (mov.x / 2, mov.y / 2);

			//Activamos el collider del ataque a mitad de animacion

			float playbackTime = stateInfo.normalizedTime; //guardamos el tiempo que dura la animacion
			//print(playbackTime);
			if (playbackTime > 0.33 && playbackTime < 0.66) {
				attackCollider.enabled = true;
			} else {
				attackCollider.enabled = false;
			}

			attacking = false;
		}

}
