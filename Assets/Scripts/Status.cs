using UnityEngine;
using System.Collections;

public class Status : Photon.MonoBehaviour
{
	public int status;
	public Vector3 currentPosition;
	public GameObject TurnManager;

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

	}

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (this.transform.position);
			stream.SendNext (this.transform.rotation);
		} else {
			currentPosition = ((Vector3)stream.ReceiveNext ());
		}
	}

	void SyncVariables ()
	{
		this.transform.position = currentPosition;
	}

	// Update is called once per frame
	void Update ()
	{
		if (!photonView.isMine) {
			//photonで値を同期
			if (TurnManager.GetComponent<Turn> ().turn < 3) {
				SyncVariables ();
			}
		} else {
			if (TurnManager.GetComponent<Turn> ().turn >= 3){
				SyncVariables ();
			}
			//自分のオブジェクトは自分で動かす
		}

	}


}
