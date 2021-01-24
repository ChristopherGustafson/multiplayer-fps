using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    List<PlayerManager> players;

    void Awake()
    {
        Instance = this;
        players = new List<PlayerManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPlayer(PlayerManager player)
    {
        players.Add(player);
    }

    public PlayerInfo[] GetScores()
    {
        PlayerInfo[] scores = new PlayerInfo[players.Count];
        foreach(PlayerManager player in players)
        {
            scores[players.IndexOf(player)] = player.playerInfo;
        }

        return scores.OrderBy(x=>x.deaths).ToArray();
    }
}
