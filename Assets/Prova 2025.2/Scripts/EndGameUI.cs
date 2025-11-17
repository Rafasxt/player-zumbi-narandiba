using UnityEngine;
using TMPro;

public class EndGameUI : MonoBehaviour
{
    public TMP_Text resultText; 
    public TMP_Text killsText;  

    void Start()
    {
        if (resultText != null)
        {
            resultText.text = GameStats.PlayerWon ? "Vitória!" : "Derrota!";
        }

        if (killsText != null)
        {
            killsText.text = "Zumbis eliminados: " + GameStats.Kills;
        }
    }
}
