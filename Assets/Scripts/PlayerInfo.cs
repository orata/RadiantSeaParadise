using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerInfo : Photon.MonoBehaviour
{
	[SerializeField] int battlePoint;
	//Photon同期用
	public int currentBattlePoint;

	PhotonView myPhotonView;

	void Start ()
	{
		battlePoint = 1000;

		if (photonView.isMine) {
			// Roomに参加しているプレイヤー情報を配列で取得.
			PhotonPlayer[] playerArray = PhotonNetwork.playerList;
			// プレイヤー名とIDを表示.
			for (int i = 0; i < playerArray.Length; i++) {
				Debug.Log ((i).ToString () + " : " + playerArray [i].name + " ID = " + playerArray [i].ID);
			}
			//自分のohotonViewを取得する
			myPhotonView = this.gameObject.GetComponent<PhotonView> ();
			//もしプレイヤーがいたら、どっちサイドか判定する
			if (playerArray.Length > 1) {

				//PlayerInfoのタグがついてるオブジェクトを探す
				GameObject[] playerInfos = GameObject.FindGameObjectsWithTag ("PlayerInfo");
				Debug.Log ("playerInfos = " + playerInfos.Length);

				PhotonView[] otherViews = new PhotonView[playerInfos.Length];
				for (int i = 0; i < playerInfos.Length; i++) {
					otherViews [i] = playerInfos [i].GetComponent<PhotonView> ();
					otherViews [i].RPC ("GoToNextScene", PhotonPlayer.Find (otherViews [i].ownerId));
				}
			}
		}

	}
	//photonによる座標の同期
	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (battlePoint);
		} else {
			currentBattlePoint = (int)stream.ReceiveNext ();
		}
	}

	void SyncVariables ()
	{
		battlePoint = currentBattlePoint;
	}

	void Update ()
	{
		//自分のビューのでないとき
		if (!photonView.isMine) {
			SyncVariables ();

			//自分のビューのであるとき
		} else {
			if (Input.GetKeyDown ("space")) {
				battlePoint++;
			}
		}
	}

	[PunRPC]
	void GoToNextScene ()
	{
		SceneManager.LoadScene ("RadiantSeaParadise");
	}
}