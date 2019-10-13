using UnityEngine;

/// <summary>
/// Сковородка
/// </summary>
public class Pan : Cookware
{
    /// <summary>
    /// Массив сковородок
    /// </summary>
    public Pan[] pans;

    /// <summary>
    /// Массив дощечек для булочек
    /// </summary>
    public Plate[] plates;

    private void OnMouseUp()
    {
        if (this != null && IsEmpty && !GameController.Instance.onStartMenu)
        {
            Debug.Log($"Таг: {gameObject.tag}");
            GameController.Instance.Cooking(this);
        }
    }
}
