using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public enum SpawnState {SPAWNING, WAITING, COUNTING};

	[System.Serializable]
	public class Wave{
		
		public string name; //Nombre de la oleada
		public GameObject enemy; //Prefab enemigo
		public int count; //Numero de enemigos
		//public float rate; 
		public float delay = 0.8f; // Delay de respawn enemigo

	}

	public Wave[] waves;
	private int nextWave = 0;
	public int maxEnemies = 12;

	public float timeBetweenWaves = 5f;
	public float waveCountdown; //Pasarlo a privada

	//private float searchCountdown = 1f;

	private SpawnState state = SpawnState.COUNTING;



	void Start(){
		waveCountdown = timeBetweenWaves;
	}

	void Update(){

		if (state == SpawnState.WAITING) {
			Debug.Log ("Wave completed");
		}

		if (waveCountdown <= 0) {
			if (state != SpawnState.SPAWNING) {
				//Empezamos a spawnear enemigos
				StartCoroutine(SpawnWave(waves[nextWave]));
			}
		} else {
			waveCountdown -= Time.deltaTime;
		}
	}

	IEnumerator SpawnWave(Wave _wave){
		
		//Comprobamos si excedemos el nuemero máximo de enemigos
		if(EnemiesAlive() > maxEnemies){
			yield break;
		}

		Debug.Log ("Spawning Wave: " + _wave.name);
		state = SpawnState.SPAWNING;

		// Spawn
		for (int i = 0; i< _wave.count; i++){
			SpawnEnemy (_wave.enemy);
			yield return new WaitForSeconds(_wave.delay);
		}

		state = SpawnState.WAITING;

		// Reiniciamos el contador
		waveCountdown = 5;

		yield break;
	}

	void SpawnEnemy(GameObject _enemy){
		//Spawn enemigo
		Debug.Log("Spawning enemy: " + _enemy.name);
		float x = Random.Range (-10.0f, 10.0f);
		float y = Random.Range (-10.0f, 10.0f);
		Vector3 position = new Vector3(transform.position.x + x, transform.position.y + y, 0);
		Instantiate (_enemy, position, transform.rotation);

	}

	int EnemiesAlive(){
		// Nos devuelve cuantos enemigos estan con vida
		return (GameObject.FindGameObjectsWithTag("Enemy").Length);
	}
}
