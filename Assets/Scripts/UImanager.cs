using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UImanager : Photon.MonoBehaviour {
	public GameObject camera;
	public GameObject[] RSP = new GameObject[3];
	public Text turnplayertext;
	public Text timetext;
	public Text counttext;
	public Text result;
	int turn;
	int count;
	int RSPnumber;
	int timelimit;
	float timer;

	TurnManager turnManager;

	// Use this for initialization
	void Start () {
	
	}

	IEnumerator Init(){
		Debug.Log ("TurnManagerを探しはじめました");
		for (;;) {
			if (FindObjectOfType<TurnManager> () != null) {
				turnManager = FindObjectOfType<TurnManager> ();
				break;
			}
			yield return new WaitForSeconds (0.1f);
		}
		Debug.Log ("TurnManagerを発見しました");
	}

	// Update is called once per frame
	void Update () {
		if (turnManager.turn < 2) {
			turnplayertext.text = "Player1";
		} else {
			turnplayertext.text = "Player2";
		}

		timer += Time.deltaTime;
		if (timer >= 1) {
			timelimit -= 1;
			timer = 0;
		}
		if (timelimit <= 0) {
			if (turnManager.turn > 2) {
				camera.GetComponent<Turn> ().turn = 6;
			} else {
				camera.GetComponent<Turn> ().turn = 7;
			}
		}
		count = camera.GetComponent<Turn> ().count;

		timetext.text = "制限時間" + timelimit;
		counttext.text = 1 + count + "手目";

	}

	public void StartButton () {
		SceneManager.LoadScene ("Room");
	}

	public void BackButton () {
		SceneManager.LoadScene ("Start");
	}

	public void Restart () {
		BackButton ();
		StartButton ();
	}

	public void InstantiateRock () {
		RSPnumber = 0;
		Instantiate ();
	}

	public void InstantiateScissors () {
		RSPnumber = 1;
		Instantiate ();
	}

	public void InstantiatePaper () {
		RSPnumber = 2;
		Instantiate ();
	}

	void Instantiate () {
		GameObject obj1 = PhotonNetwork.Instantiate ("RSP [RSPnumber]", GetComponent<Camera>().GetComponent<Turn> ().moveObj.transform.position, Quaternion.identity,0) as GameObject;
		camera.GetComponent<Turn> ().count++;
		if (camera.GetComponent<Turn> ().turn < 3) {
			obj1.tag = "Player1";
			camera.GetComponent<Turn> ().counter [0]++;
			camera.GetComponent<Turn> ().turn = 0;
		} else {
			obj1.tag = "Player2";
			camera.GetComponent<Turn> ().counter [1]++;
			camera.GetComponent<Turn> ().turn = 3;
			obj1.transform.eulerAngles = new Vector3 (0, 0, 180);
		}
		for (int i = 0; i < 3; i++) {
			camera.GetComponent<Turn> ().instantiatebutton [i].gameObject.SetActive (false);
		}
	}
}
