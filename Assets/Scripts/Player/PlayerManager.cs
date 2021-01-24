using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller;
    public PlayerInfo playerInfo;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        GameManager.Instance.AddPlayer(this);
    }

   void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }

        playerInfo = new PlayerInfo(0, 0, PV.Owner.NickName);
    }

    void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);

        playerInfo.deaths++;
        PV.RPC("RPC_UpdateDeathScore", RpcTarget.All, playerInfo.deaths);

        CreateController();
    }

    public void Kill()
    {
        if (PV.IsMine)
        {
            playerInfo.kills++;
            PV.RPC("RPC_UpdateKillScore", RpcTarget.All, playerInfo.kills);
        }
    }

    [PunRPC]
    public void RPC_UpdateKillScore(int kills)
    {
        playerInfo.kills = kills;
    }

    [PunRPC]
    public void RPC_UpdateDeathScore(int deaths)
    {
        playerInfo.deaths = deaths;
    }
   
}
