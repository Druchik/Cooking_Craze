using UnityEngine;

/// <summary>
/// Перечисление блюд
/// </summary>
public enum Dish
{
    None,
    Bread,
    Burger,
    Cola,
    HotDog
}

/// <summary>
/// Продукт
/// </summary>
public class Food : MonoBehaviour
{
    /// <summary>
    /// Родительский трансформ
    /// </summary>
    public Transform parentTransform;

    /// <summary>
    /// Префаб
    /// </summary>
    public GameObject prefab;

    /// <summary>
    /// Таймер
    /// </summary>
    public Timer timer;

    /// <summary>
    /// Время приготовления
    /// </summary>
    public int cookingTime = 5;

    /// <summary>
    /// Время горения
    /// </summary>
    public int burningTime = 7;

    /// <summary>
    /// Блюдо
    /// </summary>
    public Dish dish = Dish.None;

    /// <summary>
    /// Первый щелчок ЛКМ
    /// </summary>
    private float firstClick;

    /// <summary>
    /// Второй щелчок ЛКМ
    /// </summary>
    private float secondClick;

    /// <summary>
    /// Это первый щелчок?
    /// </summary>
    private bool isFirstClick;

    private void Awake()
    {
        if(gameObject.transform.parent != null)
            parentTransform = gameObject.transform.parent;
    }
    
    /// <summary>
    /// Метод, срабатывающий после поднятия ЛКМ
    /// </summary>
    private void OnMouseUp()
    {
        if (this != null && !GameController.Instance.onStartMenu)
        {
            if (!isFirstClick)
            {
                firstClick = Time.time;
                isFirstClick = true;
            }
            else
            {
                secondClick = Time.time;
                isFirstClick = false;

                if (secondClick - firstClick < 0.5f)
                {
                    if (gameObject.tag == "BurnMeat" || gameObject.tag == "BurnSausage")
                    {
                        Debug.Log($"Двойной клик: {gameObject.tag}");
                        GameController.Instance.Trash(this);
                    }
                }
            }

            if (gameObject.tag == "NormMeat" || gameObject.tag == "NormSausage")
            {
                Debug.Log($"Таг: {gameObject.tag}");
                GameController.Instance.PutOnBread(this);
            }

            if (dish == Dish.Burger || dish == Dish.HotDog || dish == Dish.Cola)
            {
                GameController.Instance.GiveDish(this);
            }
        }
    }
}
