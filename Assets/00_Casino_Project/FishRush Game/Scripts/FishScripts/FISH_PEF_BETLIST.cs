using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FISH_PEF_BETLIST : MonoBehaviour
{
    public static FISH_PEF_BETLIST Inst;

    [SerializeField] Text Title, Txt_MaxSeat, TxtPointValue, Txt_MinEntry, Txt_ActivePlayers;
    string Bet_ID;
    
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    public void SET_BET_DATA_LIST(ListItem data)
    {
        Title.text = "FishRush";
        Bet_ID = data.bet_id.ToString();
        Txt_MaxSeat.text = data.max_seat.ToString();
        TxtPointValue.text = data.point_value.ToString();
        Txt_MinEntry.text = data.min_entry.ToString();
        Txt_ActivePlayers.text = data.active_player.ToString();
    }

    public void BTN_PLAY_NOW()
    {
        PreeLoader.Inst.Show();
        SceneManager.LoadScene("FishRush");
    }
}
