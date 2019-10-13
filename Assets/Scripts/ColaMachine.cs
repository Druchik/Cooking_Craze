using UnityEngine;

/// <summary>
/// Аппарат разлива колы
/// </summary>
public class ColaMachine : Cookware
{
    /// <summary>
    /// Канвас
    /// </summary>
    public Canvas canvas;

    /// <summary>
    /// Таймер приготовления
    /// </summary>
    public GameObject cookingTimer;

    /// <summary>
    /// Сдвиг по оси х
    /// </summary>
    private float offsetX = 15;

    /// <summary>
    /// Сдвиг по оси y
    /// </summary>
    private float offsetY = 75f;

    /// <summary>
    /// Начать разлив колы
    /// </summary>
    public void StartPouring()
    {
        id = int.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) + 1;
        CreateCola();
        GameController.Instance.colaDrunk += CreateCola;
    }

    /// <summary>
    /// Производство колы
    /// </summary>
    public void CreateCola()
    {
        if (IsEmpty)
        {
            var drink = Instantiate(foodPrefab, spawnPoint);
            Food food = drink.GetComponent<Food>();

            var arr = drink.GetComponentsInChildren<SpriteRenderer>();

            foreach (var item in arr)
            {
                item.sortingOrder= id - 1;
            }
                
            var timerObj = Instantiate(cookingTimer, canvas.transform);
            Timer timer = timerObj.GetComponent<Timer>();

            timerObj.GetComponent<RectTransform>().anchoredPosition
                    = timer.GetCoordinates(spawnPoint, offsetX * id, offsetY, canvas);
            timer.time = food.cookingTime;
            timer.food = food;
            IsEmpty = false;
            timer.done += ColaDone;
        }
    }

    /// <summary>
    /// Кола готова
    /// </summary>
    /// <param name="cola"> Ссылка на экземпляр продукта </param>
    void ColaDone(Food cola)
    {
        var drink = Instantiate(cola.prefab, cola.parentTransform);
        drink.GetComponentInChildren<SpriteRenderer>().sortingOrder = id - 1;
        Destroy(cola.gameObject);
        drink.GetComponentInChildren<Food>().dish = Dish.Cola;
    }
}
