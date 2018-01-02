using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirPersonaje : MonoBehaviour {

	Transform personaje;
	float tLX, tLY, bRX,bRY; //Limites sumperiores e inferiores

	void Awake(){
		personaje = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Start(){
		Screen.SetResolution (800, 800, true);
	}

	// Update is called once per frame
	void Update () {
		//Forzamos resolucion cuadrada
		if(!Screen.fullScreen || Camera.main.aspect != 1){
			Screen.SetResolution (800, 800, true);
		}

		//Para salirnos
		if (Input.GetKey ("escape"))
			Application.Quit ();
		//Para pausar
		if (Input.GetKey ("p")) {
			if (Time.timeScale == 1) {
				Time.timeScale = 0;
			} else {
				Time.timeScale = 1;
			}
		}

		//transform.position = new Vector3 (personaje.position.x, personaje.position.y, transform.position.z);
		transform.position = new Vector3 (
			Mathf.Clamp (personaje.position.x, tLX, bRX),
			Mathf.Clamp (personaje.position.y, bRY, tLY),
			transform.position.z);
	}

	public void SetBound (GameObject map){
		Tiled2Unity.TiledMap config = map.GetComponent<Tiled2Unity.TiledMap> ();
		float cameraSize = Camera.main.orthographicSize;

		tLX = map.transform.position.x + cameraSize;
		tLY = map.transform.position.y - cameraSize;
		bRX = map.transform.position.x + config.NumTilesWide - cameraSize;
		bRY = map.transform.position.y - config.NumTilesHigh + cameraSize;
	}
}
