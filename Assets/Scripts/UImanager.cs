using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UImanager : Photon.MonoBehaviour {
	public GameObject camera;
	public GameObject[] RSP = new GameObject[3];
	int RSPnumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartButton () {
		SceneManager.LoadScene ("Battle");
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
