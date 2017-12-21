using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMovement : MonoBehaviour {

	public float speed = 4f;

	Animator anim;
	Rigidbody2D rb2d;
	Vector2 mov;

	CircleCollider2D attackCollider;

	public GameObject initialMap;

	void Awake(){
		Assert.IsNotNull (initialMap);
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();

		//Para mantener el personaje entre escenas persistente.
		//DontDestroyOnLoad(this);

		// Recuperamos el collider de ataque del primer hijo
		attackCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
		//Lo desactivamos al principio para que solo funcione si el jugador ataca
		attackCollider.enabled = false;

		Camera.main.GetComponent<SeguirPersonaje> ().SetBound (initialMap);
	}
	
	// Update is called once per frame
	void Update () {
		//Detectamos el input de movimiento
		mov = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		//transform.position = Vector3.MoveTowards (transform.position, transform.position + mov, speed * Time.deltaTime);

		//Cambiamos entre estados de animaciones
		if (mov != Vector2.zero) {
			anim.SetFloat ("movX", mov.x);
			anim.SetFloat ("movY", mov.y);
			anim.SetBool ("walking", true);
		} else {
			anim.SetBool ("walking", false);
		}

		//Para el ataque
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		bool attacking = stateInfo.IsName ("Player_Attack");
		//Detectamos que esta atacando
		if (Input.GetKeyDown ("space") && !attacking) {
			anim.SetTrigger("attacking");
		}

		//Vamos actualizando la posición de la colision de ataque
		if (mov != Vector2.zero)
			attackCollider.offset = new Vector2 (mov.x/2, mov.y/2);

		//Activamos el collider del ataque a mitad de animacion
		if(attacking){
			float playbackTime = stateInfo.normalizedTime; //guardamos el tiempo que dura la animacion
			//print(playbackTime);
			if (playbackTime > 0.33 && playbackTime < 0.66) {
				attackCollider.enabled = true;
			} else {
				attackCollider.enabled = false;
			}
		}

		//Para debugear
		if (Input.GetKeyDown ("d")) {
			Debug.Log("debug key was pressed");
		}
	}

	void FixedUpdate () {
		//Movimiento por fisicas del rigidbody
		rb2d.MovePosition(rb2d.position + mov * speed * Time.deltaTime);
	}

	void setInitialMap(GameObject map){
		initialMap = map;
	}
}
