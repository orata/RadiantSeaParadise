using UnityEngine;
using System.Collections;

public class TurnManager : Photon.MonoBehaviour
{
	public int turn;
	public int count;
//	public int[] counter = new int[2];
//	public int[] firstpoint = new int[2];
//	public int timelimit;
//	public int countturn;
//	public int[] span = new int [2];
//	public int[] killedstatus = new int[3000];
//	public bool[] killedpiece = new bool[3000];
//	public GameObject[] piecelog = new GameObject[3000];
//	public Vector3[] positionlog = new Vector3[3000];
//	public Vector3[] prepositionlog = new Vector3[3000];

	int currentTurn ;
    int currentCount;
//	int[] currentCounter = new int[2];
//	int[] currentFirstpoint = new int[2];
//    int currentTimelimit;
//	int currentCountturn;
//	int[] currentSpan = new int [2];
//	int[] currentKilledstatus = new int[3000];
//	bool[] currentKilledpiece = new bool[3000];
//	GameObject[] currentPiecelog = new GameObject[3000];
//	Vector3[] currentPositionlog = new Vector3[3000];
//	Vector3[] currentPrepositionlog = new Vector3[3000];

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (turn);
			stream.SendNext (count);
//			stream.SendNext (counter);
//			stream.SendNext (firstpoint);
//			stream.SendNext (timelimit);
//			stream.SendNext (countturn);
//			stream.SendNext (span);
//			stream.SendNext (killedstatus);
//			stream.SendNext (killedpiece);
//			stream.SendNext (piecelog);
//			stream.SendNext (positionlog);
//			stream.SendNext (prepositionlog);
		} else {
			currentTurn = ((int)stream.ReceiveNext ());
			currentCount = ((int)stream.ReceiveNext ());
//			currentCounter = ((int[])stream.ReceiveNext ());
//			currentFirstpoint = ((int[])stream.ReceiveNext ());
//			currentTimelimit = ((int)stream.ReceiveNext ());
//			currentCountturn = ((int)stream.ReceiveNext ());
//			currentSpan = ((int[])stream.ReceiveNext ());
//			currentKilledstatus = ((int[])stream.ReceiveNext ());
//			currentKilledpiece = ((bool[])stream.ReceiveNext ());
//			currentPiecelog = ((GameObject[])stream.ReceiveNext ());
//			currentPositionlog = ((Vector3[])stream.ReceiveNext ());
//			currentPrepositionlog = ((Vector3[])stream.ReceiveNext ());
		}
	}

	void SyncVariables ()
	{
		turn = currentTurn;
		count = currentCount;
//		counter = currentCounter;
//		firstpoint = currentFirstpoint;
//		timelimit = currentTimelimit;
//		countturn = currentCountturn;
//		span = currentSpan;
//		killedstatus = currentKilledstatus;
//		killedpiece = currentKilledpiece;
//		piecelog = currentPiecelog;
//		positionlog = currentPositionlog;
//		prepositionlog = currentPrepositionlog;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!photonView.isMine) {
			//photonで値を同期
			SyncVariables ();

		} else {
			//自分のオブジェクトは自分で動かす

		}
	}


}
