/// <summary>
/// Окно проигрыша
/// </summary>
public class GameOverUI : CompleteLevelUI
{
    /// <summary>
    /// Перезапуск уровня
    /// </summary>
    public void Restart()
    {
        GameController.Instance.Restart();
    }

    /// <summary>
    /// Выход
    /// </summary>
    public void Quit()
    {
        GameController.Instance.Quit();
    }

    private void Start()
    {
        Init();
    }
}
