using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScorePopupManager : MonoBehaviour
{
    public int par = 6;

    public GameObject winPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI congratsText;

    public void ShowWin(int strokes)
    {

        if (winPanel) winPanel.SetActive(true);
        if (congratsText) congratsText.text = "Congratulations!";
        if (scoreText) scoreText.text = $"Score: {GetScoreName(strokes, par)}";
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private string GetScoreName(int strokes, int holePar)
    {
        int diff = strokes - holePar;

        if (diff <= -4) return "Albatross";
        if (diff == -3) return "Double Eagle";
        if (diff == -2) return "Eagle";
        if (diff == -1) return "Birdie";
        if (diff == 0) return "Par";
        if (diff == 1) return "Bogey";
        if (diff == 2) return "Double Bogey";
        if (diff == 3) return "Triple Bogey";

        return diff > 3 ? $"{diff} Over Par" : $"{-diff} Under Par";
    }
}
