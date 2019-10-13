using System;
using UnityEngine;

/// <summary>
/// Таймер
/// </summary>
public class Timer : MonoBehaviour
{
    /// <summary>
    /// UI таймера
    /// </summary>
    public TimerUI timerUI;

    /// <summary>
    /// Прошедшее время
    /// </summary>
    public float timer;

    /// <summary>
    /// Общее время
    /// </summary>
    public int time;

    /// <summary>
    /// Продукт
    /// </summary>
    public Food food;

    /// <summary>
    /// Событие готовности
    /// </summary>
    public event Action<Food> done;

    void Start()
    {
        timerUI.timerBar.fillAmount = 0;
        timer = 0;
    }

    void Update()
    {
        Counter(time);
    }

    /// <summary>
    /// Счетчик времени
    /// </summary>
    /// <param name="time"> Общее время </param>
    void Counter(int time)
    {
        if (timer < time && food != null)
        {
            timer += Time.deltaTime;
            timerUI.timerBar.fillAmount = timer / time;
        }
        else if(food == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
            done?.Invoke(food);
        }
    }

    /// <summary>
    /// Получение координат для расположения таймера
    /// </summary>
    /// <param name="food"> Продукт </param>
    /// <param name="offsetX"> Смещение по х </param>
    /// <param name="offsetY"> Смещение по y </param>
    /// <param name="canvas"> Канвас </param>
    /// <returns></returns>
    public Vector3 GetCoordinates(Transform food, float offsetX, float offsetY, Canvas canvas)
    {
        Vector3 newPos = food.position;
        float scale = canvas.transform.lossyScale.x;
        newPos.x = newPos.x / scale + offsetX;
        newPos.y = newPos.y / scale + offsetY;
        newPos.z = 0;

        return newPos;
    }
}
