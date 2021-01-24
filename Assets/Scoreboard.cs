using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    PhotonView PV;
    PlayerManager playerManager;

    [SerializeField]
    GameObject playerFieldPrefab;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        PlayerInfo[] scores = GameManager.Instance.GetScores();

        foreach(PlayerInfo player in scores)
        {
            GameObject field = Instantiate(playerFieldPrefab, transform);
            field.transform.Find("PlayerName").GetComponent<Text>().text = player.nickname;
            field.transform.Find("KillsValue").GetComponent<Text>().text = player.kills.ToString();
            field.transform.Find("DeathsValue").GetComponent<Text>().text = player.deaths.ToString();

        }
    }

    private void OnDisable()
    {
        foreach(Transform child in transform)
        {
            if (child.name == "Header")
                continue;

            Destroy(child.gameObject);
        }
    }
}
