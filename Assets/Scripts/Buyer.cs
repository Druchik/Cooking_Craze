using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Покупатель
/// </summary>
public class Buyer : MonoBehaviour
{
    /// <summary>
    /// Список блюд
    /// </summary>
    public List<DishUI> dishList = new List<DishUI>();

    /// <summary>
    /// Окно заказа
    /// </summary>
    public OrderUI orderUI;

    /// <summary>
    /// Точка спавна
    /// </summary>
    public BuyerSpawnPoint buyerSpawn;

    private void Start()
    {
        buyerSpawn = GetComponentInParent<BuyerSpawnPoint>();
    }
}
