using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public PlayerInfo(int kills, int deaths, string nickname)
    {
        this.kills = kills;
        this.deaths = deaths;
        this.nickname = nickname;
    }
    public string nickname;
    public int kills;
    public int deaths;
}
