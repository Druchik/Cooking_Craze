using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Родительский класс для окон победы и поражения
/// </summary>
[System.Serializable]
public abstract class CompleteLevelUI : MonoBehaviour
{
    /// <summary>
    /// Количество отданных блюд
    /// </summary>
    public int givenMeals;

    /// <summary>
    /// Количество блюд, необходимых для выигрыша
    /// </summary>
    public int countMeals;

    /// <summary>
    /// Цели
    /// </summary>
    public Text goals;

    /// <summary>
    /// Полоска прогресса
    /// </summary>
    public Image progressBar;

    /// <summary>
    /// Инициализация
    /// </summary>
    public void Init()
    {
        givenMeals = GameController.Instance.givenDish;
        countMeals = GameController.Instance.countDish - 2;
        goals.text = $"{givenMeals} / {countMeals}";
        progressBar.fillAmount = (float)givenMeals / countMeals;
    }
}
