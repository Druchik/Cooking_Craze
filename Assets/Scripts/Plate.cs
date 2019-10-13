using UnityEngine;

/// <summary>
/// Дощечка для булочек
/// </summary>
public class Plate : Cookware
{
    /// <summary>
    /// Массив дощечек
    /// </summary>
    public Plate[] plates;

    private void OnMouseUp()
    {
        if (this != null && IsEmpty && !GameController.Instance.onStartMenu)
        {
            Debug.Log($"Таг: {gameObject.tag}");
            GameController.Instance.PutBreadOnPlate(this);
        }
    }
}
