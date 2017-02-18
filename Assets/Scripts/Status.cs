using UnityEngine;
using System.Collections;

public class Status : Photon.MonoBehaviour
{
	public int status;
	public Vector3 currentPosition;
	public GameObject camera;
	public Quaternion currentRotation;

	TurnManager turnManager;

	void Awake ()
	{
		if (this.gameObject.name == "Rock(Clone)") {
			status = 0;
		}
		if (this.gameObject.name == "Scissors(Clone)") {
			status = 1;
		}
		if (this.gameObject.name == "Paper(Clone)") {
			status = 2;
		}
	}
		

	// Use this for initialization
	void Start ()
	{
		StartCoroutine ("Init");
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

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (this.transform.position);
			stream.SendNext (this.transform.rotation);
		} else {
			currentPosition = ((Vector3)stream.ReceiveNext ());
			currentRotation = ((Quaternion)stream.ReceiveNext ());
		}
	}

	void SyncVariables ()
	{
		this.transform.position = currentPosition;
		this.transform.rotation = currentRotation;
	}

	// Update is called once per frame
	void Update ()
	{
		if (!photonView.isMine) {
			//photonで値を同期
			if (turnManager.turn <= 3) {
				SyncVariables ();
			}
		}
	}
}
