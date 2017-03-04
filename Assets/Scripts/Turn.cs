using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Turn : Photon.MonoBehaviour
{
	
	public int turn;
	public int count = 3;
	public int[] counter = new int[2];
	public int currentfirstpoint;
	int moveObjstatus;
	int blockstatus;
	int[] firstatack = new int[6];
	public int firstpoint;
	public int timelimit = 75;
	public int countturn;
	public int[] span = new int[2];
	public int[] killedstatus = new int[3000];
	float timer;
	public string[] RSP = new string[3];
	public GameObject block;
	public GameObject[] button = new GameObject[2];
	public GameObject[] instantiatebutton = new GameObject[3];
	public GameObject TurnManager;
	GameObject destinationObj;
	public GameObject moveObj;
	GameObject[] piece = new GameObject[6];
	public GameObject[] piecelog = new GameObject[3000];
	bool[] flag = new bool[6];
	public bool[] killedpiece = new bool[3000];
	Vector3[] firstposition = new Vector3[6];
	Vector3[] instantiateposition = new Vector3[4];
	public Vector3[] positionlog = new Vector3[3000];
	public Vector3[] prepositionlog = new Vector3[3000];
	public Text turnplayertext;
	public Text timetext;
	public Text counttext;
	public Text result;

	TurnManager turnManager;

	// Use this for initialization
	void Start ()
	{   
		if (PhotonNetwork.isMasterClient == true) {
			PhotonNetwork.Instantiate ("TurnManager", Vector3.zero, Quaternion.identity, 0);
		}
		StartCoroutine ("Init");
		
		RSP [0] = "Rock";
		RSP [1] = "Scissors";
		RSP [2] = "Paper";
		//ブロック生成
			for (int i = 0; i < 8; i++) {
				for (int j = 0; j < 5; j++) {
					GameObject obj = Instantiate (block, new Vector3 (j, i, 0), Quaternion.identity) as GameObject;
					obj.name = "block" + i.ToString () + j.ToString ();
					if (obj.name == "block00" || obj.name == "block04") {
						obj.tag = "P1Instantiate";
					}
					if (obj.name == "block70" || obj.name == "block74") {
						obj.tag = "P2Instantiate";
					}
					if (obj.name == "block02") {
						obj.tag = "P1goal";
						obj.GetComponent<Renderer> ().material.color = Color.yellow;
					}
					if (obj.name == "block72") {
						obj.tag = "P2goal";
						obj.GetComponent<Renderer> ().material.color = Color.yellow;
					}
				}
			}

			firstposition [0] = new Vector3 (1, 0, 0);
			firstposition [1] = new Vector3 (2, 1, 0);
			firstposition [2] = new Vector3 (3, 0, 0);
			firstposition [3] = new Vector3 (1, 7, 0);
			firstposition [4] = new Vector3 (2, 6, 0);
			firstposition [5] = new Vector3 (3, 7, 0);
			//グーチョキパー生成
		if (photonView.isMine) {
			for (int a = 0; a < 2; a++) {
				int c = 0;
				while (flag [0 + a * 3] == false || flag [1 + a * 3] == false || flag [2 + a * 3] == false) {
					int b = Random.Range (0, 3);
					if (flag [b + a * 3] == false) { 
						piece [c + a * 3] = PhotonNetwork.Instantiate (RSP [b], firstposition [c + a * 3], Quaternion.identity, 0) as GameObject;
						counter [a]++;
						firstatack [c + a * 3] = piece [c + a * 3].gameObject.GetComponent<Status> ().status;
						if (a == 0) {
							piece [c + a * 3].tag = "Player1";
						} else {
							piece [c + a * 3].tag = "Player2";
							piece [c + a * 3].transform.eulerAngles = new Vector3 (0, 0, 180);
						}
						flag [b + a * 3] = true;
						c++;
					}
				}
			}

			//先攻決定
			for (int i = 0; i < 3; i++) {
				if (firstatack [0 + i] == 0 && firstatack [3 + i] == 1 || firstatack [0 + i] == 1 && firstatack [3 + i] == 2 || firstatack [0 + i] == 2 && firstatack [3 + i] == 0) {
					firstpoint++;

				}
				if (firstatack [0 + i] == 1 && firstatack [3 + i] == 0 || firstatack [0 + i] == 2 && firstatack [3 + i] == 1 || firstatack [0 + i] == 0 && firstatack [3 + i] == 2) {
					firstpoint--;
				}
			}
			if (firstpoint < 0) {
				turn = 0;
			} 
			if (firstpoint > 0) {
				turn = 3;
			}
			if (firstpoint == 0) {
				int a = Random.Range (0, 2);
				if (a == 0) {
					turn = 0;
				} else {
					turn = 3;
				}
			}
		}

		instantiateposition [0] = new Vector3 (0, 0, 0);
		instantiateposition [1] = new Vector3 (4, 0, 0);
		instantiateposition [2] = new Vector3 (0, 7, 0);
		instantiateposition [3] = new Vector3 (4, 7, 0);
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
	void Update ()
	{ 
		if (Input.GetKeyDown("space")) {
			FindObjectOfType<TurnManager> ().turn++;
		}

		if (turn < 2) {
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
			if (turn > 2) {
				turn = 6;
			} else {
				turn = 7;
			}
		}

		timetext.text = "制限時間" + timelimit;
		counttext.text = 1 + count + "手目";


		if (Input.GetKeyDown ("b") && count > 0) {
			piecelog [countturn * 3 + count - 1].transform.position = prepositionlog [countturn * 3 + count - 1];
			if (turn <= 2) {
				if (killedpiece [countturn * 3 + count - 1] == true) {
					for(int i = 0; i < 3; i++){
						if (killedstatus [countturn * 3 + count - 1] == i) {
							GameObject obj = PhotonNetwork.Instantiate (RSP [i], positionlog [countturn * 3 + count - 1], Quaternion.Euler (0, 180, 0),0) as GameObject;
							obj.tag = "Player2";
						}
					}
				}
				turn = 0;
			} else {
				if (killedpiece [countturn * 3 + count - 1] == true) {
					for(int i = 0; i < 3; i++){
						if (killedstatus [countturn * 3 + count - 1] == i) {
							GameObject obj = PhotonNetwork.Instantiate (RSP [i], positionlog [countturn * 3 + count - 1], Quaternion.identity,0) as GameObject;
							obj.tag = "Player1";
						}
					}
				}
				turn = 3;
			}
			count--;
		}

		switch (turn) {
		case 0:
                //自ターン取得
			if (Input.GetMouseButtonDown (0)) {
				moveObj = GetTouchedObj ();
				if (moveObj != null) {
					if (moveObj.tag == "Player1") {
						moveObjstatus = moveObj.gameObject.GetComponent<Status> ().status;
						turn++;
					}
					if (moveObj.tag == "P1Instantiate" && counter[0] + span[0] < 3) {
						turn = 2;
					}
				}
			}

                //3回行動したら敵ターン取得へ
			if (count == 3) {
				countturn++;
				count = 0;
				turn = 3;
				span [0] = 0;
				timelimit = 75;
			}

			break;
		case 1:
                //自ターン移動
			if (Input.GetMouseButtonDown (0)) {
				destinationObj = GetTouchedObj ();
				if (destinationObj != null) {
					switch (destinationObj.tag) {
					case "Player1":
						moveObj = destinationObj;
						moveObjstatus = destinationObj.GetComponent<Status> ().status;
						break;
					case "Player2":
						blockstatus = destinationObj.GetComponent<Status> ().status;
						if (JudgeIfNearBy () == true && JudgeJanken () == true) {
							killedpiece [countturn * 3 + count] = true;
							killedstatus [countturn * 3 + count] = blockstatus;
							Destroy (destinationObj.gameObject);
							counter [1]--;
							span [1]++;
							piecelog [countturn * 3 + count] = moveObj;
							prepositionlog [countturn * 3 + count] = moveObj.transform.position;
							moveObj.transform.position = destinationObj.transform.position;
							positionlog [countturn * 3 + count] = moveObj.transform.position;
							count++;
							turn = 0;
						}
						break;
					case "Block":
						if (JudgeIfNearBy () == true) {
							piecelog [countturn * 3 + count] = moveObj;
							prepositionlog [countturn * 3 + count] = moveObj.transform.position;
							moveObj.transform.position = destinationObj.transform.position;
							positionlog [countturn * 3 + count] = moveObj.transform.position;
							count++;
							turn = 0;
						}
						break;
					case "Instantiate":
						if (JudgeIfNearBy () == true) {
							piecelog [countturn * 3 + count] = moveObj;
							prepositionlog [countturn * 3 + count] = moveObj.transform.position;
							moveObj.transform.position = destinationObj.transform.position;
							positionlog [countturn * 3 + count] = moveObj.transform.position;
							count++;
							turn = 0;
						}
						break;
					case "P2goal":
						if (JudgeIfNearBy () == true) {
							piecelog [countturn * 3 + count] = moveObj;
							prepositionlog [countturn * 3 + count] = moveObj.transform.position;
							moveObj.transform.position = destinationObj.transform.position;
							positionlog [countturn * 3 + count] = moveObj.transform.position;
							turn = 6;
						}
						break;
					}
				}
			}

			break;
		case 2:
                //グーチョキパー生成
			instantiatebutton[0].gameObject.SetActive (true);
			instantiatebutton[1].gameObject.SetActive (true);
			instantiatebutton[2].gameObject.SetActive (true);

			break;
		case 3:
                //敵ターン取得
			if (Input.GetMouseButtonDown (0)) {
				moveObj = GetTouchedObj ();
				if (moveObj != null) {
					if (moveObj.tag == "Player2") {
						moveObjstatus = moveObj.gameObject.GetComponent<Status> ().status;
						turn++;
					}
				}
				if (moveObj.tag == "P2Instantiate" && counter[1] + span[1] < 3) {
					turn = 5;
				}
			}

                //3回行動したら自ターン取得へ
			if (count == 3) {
				countturn++;
				count = 0;
				turn = 0;
				span [1] = 0;
				timelimit = 75;
			}

			break;
		case 4:
                //敵ターン移動
			if (Input.GetMouseButtonDown (0)) {
				destinationObj = GetTouchedObj ();
				if (destinationObj != null) {

					switch (destinationObj.tag) {
					case "Player2":
						moveObj = destinationObj;
						moveObjstatus = destinationObj.GetComponent<Status> ().status;
						break;
					case "Player1":
						blockstatus = destinationObj.GetComponent<Status> ().status;
						if (JudgeIfNearBy () == true && JudgeJanken () == true) {
							killedpiece [countturn * 3 + count] = destinationObj;
							killedstatus [countturn * 3 + count] = blockstatus;
							Destroy (destinationObj.gameObject);
							counter [0]--;
							span [0]++;
							piecelog [countturn * 3 + count] = moveObj;
							prepositionlog [countturn * 3 + count] = moveObj.transform.position;
							moveObj.transform.position = destinationObj.transform.position;
							positionlog [countturn * 3 + count] = moveObj.transform.position;
							count++;
							turn = 3;
						}
						break;
					case "Block":
						if (JudgeIfNearBy () == true) {
							piecelog [countturn * 3 + count] = moveObj;
							prepositionlog [countturn * 3 + count] = moveObj.transform.position;
							moveObj.transform.position = destinationObj.transform.position;
							positionlog [countturn * 3 + count] = moveObj.transform.position;
							count++;
							turn = 3;
						}
						break;
					case "Instantiate":
						if (JudgeIfNearBy () == true) {
							piecelog [countturn * 3 + count] = moveObj;
							prepositionlog [countturn * 3 + count] = moveObj.transform.position;
							moveObj.transform.position = destinationObj.transform.position;
							positionlog [countturn * 3 + count] = moveObj.transform.position;
							count++;
							turn = 0;
						}
						break;
					case "P1goal":
						if (JudgeIfNearBy () == true) {
							piecelog [countturn * 3 + count] = moveObj;
							prepositionlog [countturn * 3 + count] = moveObj.transform.position;
							moveObj.transform.position = destinationObj.transform.position;
							positionlog [countturn * 3 + count] = moveObj.transform.position;
							turn = 7;
						}
						break;
					}
				}
			}
                
			break;
		case 5:
                //グーチョキパー生成
			instantiatebutton[0].gameObject.SetActive (true);
			instantiatebutton[1].gameObject.SetActive (true);
			instantiatebutton[2].gameObject.SetActive (true);

			break;
		case 6:
			result.text = "P1win!";
			result.gameObject.SetActive (true);
			button[0].gameObject.SetActive (true);
			button[1].gameObject.SetActive (true);

			break;
		case 7:
			result.text = "P2win!";
			result.gameObject.SetActive (true);
			button[0].gameObject.SetActive (true);
			button[1].gameObject.SetActive (true);
			break;
		}
	}

	GameObject GetTouchedObj ()
	{
		Ray ray;
		RaycastHit hit;
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			GameObject touchedObj = hit.transform.gameObject;
			return touchedObj;
		} else {
			return null;
		}
	}

	bool JudgeIfNearBy ()
	{
		if (moveObj.transform.position.x == destinationObj.transform.position.x + 1 && moveObj.transform.position.y == destinationObj.transform.position.y || moveObj.transform.position.x == destinationObj.transform.position.x - 1 && moveObj.transform.position.y == destinationObj.transform.position.y || moveObj.transform.position.x == destinationObj.transform.position.x && moveObj.transform.position.y == destinationObj.transform.position.y + 1 || moveObj.transform.position.x == destinationObj.transform.position.x && moveObj.transform.position.y == destinationObj.transform.position.y - 1) {
			return true;
		} else {
			return false;
		}
	}

	bool JudgeJanken ()
	{
		if (moveObjstatus == 0 && blockstatus == 1 || moveObjstatus == 1 && blockstatus == 2 || moveObjstatus == 2 && blockstatus == 0) {
			return true;
		} else {
			return false;
		}
	}
}
