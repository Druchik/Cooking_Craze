using UnityEngine;

/// <summary>
/// Родительский класс для кухонной утвари
/// </summary>
[System.Serializable]
public abstract class Cookware : MonoBehaviour
{
    /// <summary>
    /// Id
    /// </summary>
    public int id;

    /// <summary>
    /// Точка спавна
    /// </summary>
    public Transform spawnPoint;

    /// <summary>
    /// Префаб продукта
    /// </summary>
    public GameObject foodPrefab;

    /// <summary>
    /// Продукт
    /// </summary>
    public Food food;

    /// <summary>
    /// Свойство занятости
    /// </summary>
    public bool IsEmpty { get; set; } = true;
}
