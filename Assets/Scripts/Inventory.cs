using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	//Objeto Pico
	public bool iPico = false;

	//Objeto Escopeta
	bool iEscopeta= false;

	//Objeto Silbato
	bool iSilbato= false;

	//Objeto Llave
	public bool iLlave= false;



	// SETETS
	public void setPico(bool p){
		iPico = p;
	}

	public void setEscopeta(bool e){
		iEscopeta = e;
	}

	public void setSilbato(bool s){
		iSilbato = s;
	}

	public void setLlave(bool l){
		iLlave = l;
	}


	// GETTERS
	public bool getPico(){
		return iPico;
	}

	public bool getEscopeta(){
		return iEscopeta;
	}

	public bool getSilbato(){
		return iSilbato;
	}

	public bool getLlave(){
		return iLlave;
	}
		



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
