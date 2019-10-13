using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    public Text goals;

    private void Start()
    {
        GameController.Instance.onStartMenu = true;
        goals.text = $"х {GameController.Instance.countDish - 2}";
    }

    public void PlayGame()
    {
        gameObject.SetActive(false);
        GameController.Instance.StartGame();
    }
}
