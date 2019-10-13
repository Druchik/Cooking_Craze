/// <summary>
/// Окно победы
/// </summary>
public class WinUI : CompleteLevelUI
{
    /// <summary>
    /// Продолжить
    /// </summary>
    public void Continue()
    {
        GameController.Instance.Quit();
    }

    private void Start()
    {
        Init();
    }
}
