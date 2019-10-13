using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Контроллер игры
/// </summary>
public class GameController : Singleton<GameController>
{
    /// <summary>
    /// Смещение по x для таймера
    /// </summary>
    private float offsetX = -70f;

    /// <summary>
    /// Смещение по y для таймера
    /// </summary>
    private float offsetY = 40f;

    /// <summary>
    /// Смещение по x для окна заказа
    /// </summary>
    private float charUI_offsetX = -165f;

    /// <summary>
    /// Смещение по y для окна заказа
    /// </summary>
    private float charUI_offsetY = 45f;
    
    /// <summary>
    /// Таймер приготовления
    /// </summary>
    public GameObject cookingTimer;

    /// <summary>
    /// Таймер горения
    /// </summary>
    public GameObject burningTimer;

    /// <summary>
    /// Префаб бургера
    /// </summary>
    public GameObject burgerPrefab;

    /// <summary>
    /// Префаб хот-дога
    /// </summary>
    public GameObject hotDogPrefab;

    public GameObject menu;

    /// <summary>
    /// Массив префабов покупателей
    /// </summary>
    public Buyer[] buyerPrefabs;

    /// <summary>
    /// Префаб окна заказа
    /// </summary>
    public GameObject orderUI;

    /// <summary>
    /// Массив блюд, отображаемых в заказе
    /// </summary>
    public DishUI[] dish;

    /// <summary>
    /// Спрайт совпадения
    /// </summary>
    public Sprite check;

    /// <summary>
    /// Список всех заказов
    /// </summary>
    public List<DishUI[]> orders = new List<DishUI[]>();
    
    /// <summary>
    /// Список покупателей
    /// </summary>
    private List<Buyer> buyers = new List<Buyer>();

    /// <summary>
    /// Текущий индекс
    /// </summary>
    private int currentIndex;

    /// <summary>
    /// Массив точек спавна покупателей
    /// </summary>
    public BuyerSpawnPoint[] buyerSpawnPoints;

    /// <summary>
    /// Отображаемое количество блюд для победы
    /// </summary>
    public Text needToWeen;

    /// <summary>
    /// Отображаемое число покупателей
    /// </summary>
    public Text buyersCount;

    /// <summary>
    /// Полоса прогресса игры
    /// </summary>
    public Image progressBar;

    /// <summary>
    /// Окно победы
    /// </summary>
    public WinUI winUI;

    /// <summary>
    /// Окно поражения
    /// </summary>
    public GameOverUI gameoverUI;

    /// <summary>
    /// Количество блюд
    /// </summary>
    public int countDish = 0;

    /// <summary>
    /// Отданные блюда
    /// </summary>
    public int givenDish = 0;

    /// <summary>
    /// Количество оставшихся покупателей
    /// </summary>
    private int countBuyers;

    /// <summary>
    /// Канвас
    /// </summary>
    public Canvas canvas;

    /// <summary>
    /// Флаг нахождения в стартовом меню
    /// </summary>
    public bool onStartMenu;

    /// <summary>
    /// Событие выпитая кола
    /// </summary>
    public event Action colaDrunk;

    protected override void Awake()
    {
        base.Awake();
        //menu.SetActive(false);
        CreateOrdersList();
    }

    /// <summary>
    /// Начать игру
    /// </summary>
    public void StartGame()
    {
        onStartMenu = false;
        menu.SetActive(true);
        progressBar.fillAmount = 0;
        
        UpdateProgress();
        UpdateBuyersCount();

        StartCoroutine(FirstBuyers());
    }

    /// <summary>
    /// Обновить информацию о покупателях
    /// </summary>
    private void UpdateBuyersCount()
    {
        buyersCount.text = $"{countBuyers}";
    }

    /// <summary>
    /// Обновить игровой прогресс
    /// </summary>
    private void UpdateProgress()
    {
        needToWeen.text = $"{givenDish}/{countDish - 2}";
        progressBar.fillAmount = (float)givenDish / (countDish - 2);
    }

    /// <summary>
    /// Создание списка заказов
    /// </summary>
    void CreateOrdersList()
    {
        int newRandom = 0;
        int num;

        for (int i = 0; i < 15; i++)
        {
            do
            {
                num = UnityEngine.Random.Range(1, 4);
            } while (newRandom == num);

            newRandom = num;
            countDish += num;

            AddRandomCountDishInOrder(num);
        }
        countBuyers = orders.Count;
        Debug.Log($"количество заказов: {orders.Count}");
    }

    /// <summary>
    /// Добавить случайное количество блюдв заказ
    /// </summary>
    /// <param name="index"> Количество блюд </param>
    void AddRandomCountDishInOrder(int index)
    {
        DishUI[] tempOrder = new DishUI[index];
        for (int i = 0; i < index; i++)
        {
            tempOrder[i] = RandomDish();
        }
        orders.Add(tempOrder);
    }

    /// <summary>
    /// Случайное блюдо
    /// </summary>
    /// <returns> Блюдо </returns>
    DishUI RandomDish()
    {
        int rand = UnityEngine.Random.Range(0, dish.Length);
        return dish[rand];
    }

    /// <summary>
    /// Новый покуупатель
    /// </summary>
    /// <param name="bSpawnPoint"> Точка спавна </param>
    void NewBuyer(BuyerSpawnPoint bSpawnPoint)
    {
        SpawnCurrent(bSpawnPoint);
    }

    /// <summary>
    /// Спавн покупателя с текущим заказом
    /// </summary>
    /// <param name="bSpawnPoint"> Точка спавна </param>
    void SpawnCurrent(BuyerSpawnPoint bSpawnPoint)
    {
        Spawn(bSpawnPoint);
    }

    /// <summary>
    /// Спавн покупателя с текущим заказом
    /// </summary>
    /// <param name="bSpawnPoint"> Точка спавна </param>
    void Spawn(BuyerSpawnPoint bSpawnPoint)
    {
        int index = UnityEngine.Random.Range(0, buyerPrefabs.Length);
        var newBuyer = Instantiate(buyerPrefabs[index], bSpawnPoint.spawnPoint);
        Buyer buyer = newBuyer.GetComponent<Buyer>();
        newBuyer.name = $"Char_{currentIndex}";

        var orderPanel = Instantiate(orderUI, canvas.transform);
        OrderUI orderPanelUI = orderPanel.GetComponent<OrderUI>();
        orderPanel.name = $"{newBuyer.name}_orderUI";

        buyer.orderUI = orderPanelUI;
        orderPanelUI.buyer = buyer;

        orderPanel.GetComponent<RectTransform>().anchoredPosition
                    = GetCoordinates(bSpawnPoint.spawnPoint, charUI_offsetX, charUI_offsetY, canvas);

        bSpawnPoint.IsEmpty = false;
        var order = orders[currentIndex];

        orderPanelUI.meals[order.Length - 1].SetActive(true);

        foreach (var meal in order)
        {
            var food = Instantiate(meal, orderPanelUI.meals[order.Length - 1].transform);
            buyer.dishList.Add(food);
        }
        buyers.Add(buyer);
        countBuyers--;
        currentIndex++;
        UpdateBuyersCount();
    }

    /// <summary>
    /// Сопрограмма для первых 4 покупателей
    /// </summary>
    /// <returns></returns>
    IEnumerator FirstBuyers()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(3f);
            SpawnInEmptyPoint();
        }
    }

    /// <summary>
    /// Сопрограмма спавна нового покупателя, после ухода другого
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnAfterBuyerLeave()
    {
        yield return new WaitForSeconds(3f);
        if (currentIndex < orders.Count)
        {
            SpawnInEmptyPoint();
        }
    }

    /// <summary>
    /// Спавн в свободной точке
    /// </summary>
    void SpawnInEmptyPoint()
    {
        foreach (var spawnPoint in buyerSpawnPoints)
        {
            if (spawnPoint.IsEmpty)
            {
                NewBuyer(spawnPoint);
                break;
            }
        }
    }
    /// <summary>
    /// Получение координат для создания окна заказа
    /// </summary>
    /// <param name="food"> Продукт </param>
    /// <param name="offsetX"> Смещение по x </param>
    /// <param name="offsetY"> Смещение по y </param>
    /// <param name="canvas"> Канвас </param>
    /// <returns> Новые координаты </returns>
    public Vector3 GetCoordinates(Transform food, float offsetX, float offsetY, Canvas canvas)
    {
        Vector3 newPos = food.position;
        float scale = canvas.transform.lossyScale.x;
        newPos.x = newPos.x / scale + offsetX;
        newPos.y = newPos.y / scale + offsetY;
        newPos.z = 0;

        return newPos;
    }
    
    /// <summary>
    /// Приготовление
    /// </summary>
    /// <param name="p"> Сковорода </param>
    public void Cooking(Pan p)
    {
        foreach (var pan in p.pans)
        {
            if (pan.IsEmpty)
            {
                var foodObj = Instantiate(pan.foodPrefab, pan.spawnPoint);
                Food food = foodObj.GetComponent<Food>();

                var timerObj = Instantiate(cookingTimer, canvas.transform);
                Timer timer = timerObj.GetComponent<Timer>();

                timerObj.name = $"{pan.name}_cookingTimer";
                timerObj.GetComponent<RectTransform>().anchoredPosition
                    = timer.GetCoordinates(pan.spawnPoint, offsetX, offsetY, canvas);
                timer.time = food.cookingTime;
                timer.food = food;
                pan.IsEmpty = false;
                timer.done += BurningFood;
                break;
            }
        }
    }

    /// <summary>
    /// Горение еды
    /// </summary>
    /// <param name="f"> Продукт </param>
    private void BurningFood(Food f)
    {
        var foodObj = Instantiate(f.prefab, f.parentTransform);
        Destroy(f.gameObject);
        Food food = foodObj.GetComponent<Food>();

        var timerObj = Instantiate(burningTimer, canvas.transform);
        Timer timer = timerObj.GetComponent<Timer>();

        timerObj.name = $"{foodObj.transform.root.name}_burningTimer";
        food.timer = timer;
        timerObj.GetComponent<RectTransform>().anchoredPosition
            = timer.GetCoordinates(food.parentTransform, offsetX, offsetY, canvas);
        timer.time = food.burningTime;
        timer.food = food;
        timer.done += Burn;
    }

    /// <summary>
    /// Сгоревший продукт
    /// </summary>
    /// <param name="food"> Продукт </param>
    private void Burn(Food food)
    {
        Instantiate(food.prefab, food.parentTransform);
        Destroy(food.transform.parent.GetChild(0).gameObject);
    }

    /// <summary>
    /// Положить булочку на свободную дощечку
    /// </summary>
    /// <param name="p"> Дощечка для булочек </param>
    public void PutBreadOnPlate(Plate p)
    {
        foreach (var plate in p.plates)
        {
            if (plate.IsEmpty)
            {
                var bread = Instantiate(plate.foodPrefab, plate.spawnPoint);
                plate.food = bread.GetComponent<Food>();
                plate.IsEmpty = false;
                break;
            }
        }
    }

    /// <summary>
    /// Положить на булочку
    /// </summary>
    /// <param name="food"> Продукт </param>
    public void PutOnBread(Food food)
    {
        Pan p = food.gameObject.transform.root.GetChild(0).GetComponent<Pan>();
        foreach (var plate in p.plates)
        {
            if (!plate.IsEmpty && plate.food.dish == Dish.Bread)
            {
                GameObject newObj;
                Destroy(plate.spawnPoint.gameObject.transform.GetChild(0).gameObject);

                if (plate.gameObject.tag == "BurgerBread")
                    newObj = Instantiate(burgerPrefab, plate.spawnPoint);
                else
                    newObj = Instantiate(hotDogPrefab, plate.spawnPoint);

                plate.food = newObj.GetComponentInChildren<Food>();
                p.IsEmpty = true;
                Destroy(food.transform.parent.gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// Отдать блюдо
    /// </summary>
    /// <param name="food"> Продукт </param>
    public void GiveDish(Food food)
    {
        if (buyers.Count > 0)
        {
            var buyer = GetBuyerWithMinTimer(food);

            if (buyer != null)
            {
                var dishUI = GetDishInOrder(buyer, food);

                dishUI.gameObject.GetComponent<Image>().sprite = check;

                if(dishUI.images.Length > 1)
                {
                    for (int i = 1; i < dishUI.images.Length; i++)
                    {
                        dishUI.images[i].enabled = false;
                    }
                }

                givenDish++;
                buyer.dishList.Remove(dishUI);
                UpdateProgress();

                food.gameObject.GetComponentInParent<Cookware>().IsEmpty = true;
                Destroy(food.parentTransform.gameObject);

                if(dishUI.dish == Dish.Cola)
                    colaDrunk?.Invoke();

                if (buyer.dishList.Count != 0)
                    buyer.orderUI.WaitTime += 6f;
                else
                    BuyerLeave(buyer);
            }
        }
    }

    /// <summary>
    /// Получить блюдо из заказа покупателя
    /// </summary>
    /// <param name="buyer"> Покупатель </param>
    /// <param name="food"> Продукт </param>
    /// <returns> Блюдо из заказа </returns>
    DishUI GetDishInOrder(Buyer buyer, Food food)
    {
        var dish = buyer.dishList.FirstOrDefault(m => m.GetComponent<DishUI>().dish == food.dish);
        return dish;
    }

    /// <summary>
    /// Получить покупателя с наименьшим таймером
    /// </summary>
    /// <param name="food"> Продукт </param>
    /// <returns> Покупатель </returns>
    Buyer GetBuyerWithMinTimer(Food food)
    {
        var buyer = buyers.Where(b => b.dishList.FirstOrDefault(m => m.GetComponent<DishUI>().dish == food.dish))
                         .OrderBy(b => b.orderUI.WaitTime)
                         .FirstOrDefault();

        return buyer;
    }

    /// <summary>
    /// Уход покупателя
    /// </summary>
    /// <param name="buyer"> Покупатель </param>
    public void BuyerLeave(Buyer buyer)
    {
        Destroy(buyer.orderUI.gameObject);
        buyer.buyerSpawn.IsEmpty = true;
        Destroy(buyer.gameObject);
        buyers.Remove(buyer);

        if(currentIndex < orders.Count)
            StartCoroutine(SpawnAfterBuyerLeave());

        if (currentIndex == orders.Count && buyers.Count == 0)
            FinishLevel();
    }

    /// <summary>
    /// Выбрасывание в корзину сгоревших продуктов
    /// </summary>
    /// <param name="food"> Продукт </param>
    public void Trash(Food food)
    {
        food.gameObject.transform.root.GetChild(0).GetComponent<Pan>().IsEmpty = true;
        Destroy(food.transform.parent.gameObject);
    }

    /// <summary>
    /// Окончание уровня
    /// </summary>
    void FinishLevel()
    {
        //Time.timeScale = 0;

        if ((float)givenDish / (countDish - 2) >= 1)
        {
            Instantiate(winUI, canvas.transform);
        }
        else
        {
            Instantiate(gameoverUI, canvas.transform);
        }
    }

    /// <summary>
    /// Перезапуск сцены
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Выход
    /// </summary>
    public void Quit()
    {
        #if UNITY_EDITOR

            if (UnityEditor.EditorApplication.isPlaying)
                UnityEditor.EditorApplication.isPlaying = false;

        #endif

        Application.Quit();
    }
}
