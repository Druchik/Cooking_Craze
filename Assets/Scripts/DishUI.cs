using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Блюдо, отображаемое в окне заказа
/// </summary>
public class DishUI : MonoBehaviour
{
    /// <summary>
    /// Блюдо
    /// </summary>
    public Dish dish;

    public Image[] images;

    private void Start()
    {
        images = GetComponentsInChildren<Image>();
    }
}
