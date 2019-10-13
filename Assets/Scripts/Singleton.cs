using UnityEngine;

/// <summary>
/// Класс одиночка
/// </summary>
/// <typeparam name="T">Тип одиночки</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    /// <summary>
    /// Статическая ссылка на экземпляр одиночки
    /// </summary>
    public static T Instance { get; protected set; }

    /// <summary>
    /// Получаем, существует ли экземпляр одиночки
    /// </summary>
    public static bool InstanceExists => Instance != null;

    /// <summary>
    /// Awake метод для связывания одиночки с экземпляром
    /// </summary>
    protected virtual void Awake()
    {
        if (InstanceExists)
            Destroy(gameObject);
        else
            Instance = (T)this;
    }

    /// <summary>
    /// OnDestroy метод для освобождения ассоциации с одиночкой
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
