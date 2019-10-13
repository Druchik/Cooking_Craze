using UnityEngine;

/// <summary>
/// Окно заказа покупателя
/// </summary>
public class OrderUI : MonoBehaviour
{
    /// <summary>
    /// UI таймера
    /// </summary>
    public TimerUI timerUI;

    /// <summary>
    /// Покупатель
    /// </summary>
    public Buyer buyer;

    /// <summary>
    /// Таймер
    /// </summary>
    [SerializeField]
    private float _timer;

    /// <summary>
    /// Время ожидания покупателя
    /// </summary>
    private int _time = 18;

    /// <summary>
    /// Время ожидания
    /// </summary>
    public float WaitTime
    {
        get => _timer;
        set => _timer = Mathf.Min(value, 18);
    }

    /// <summary>
    /// Массив для отображения различного количества блюд
    /// </summary>
    public GameObject[] meals;

    void Start()
    {
        timerUI.timerBar.fillAmount = 1;
        _timer = _time;
    }

    void Update()
    {
        Counter(_time);
    }

    /// <summary>
    /// Счетчик времени
    /// </summary>
    /// <param name="time"> Время </param>
    void Counter(int time)
    {
        if (timerUI.timerBar.fillAmount != 0)
        {
            timerUI.timerBar.fillAmount = _timer / time;
            _timer -= Time.deltaTime;
        }
        else GameController.Instance.BuyerLeave(buyer);
    }
}
