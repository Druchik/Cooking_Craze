using System.Collections.Generic;

public static class IListExtensions
{
    /// <summary>
    /// Goes to the next element of the list
    /// </summary>
    /// <param name="elements">An <see cref="System.Collections.Generic.IList{T}" /> of elements to choose from</param>
    /// <param name="currentIndex">The current index to be changed via reference</param>
    /// <param name="wrap">if the list should wrap</param>
    /// <typeparam name="T">The generic parameter for the list</typeparam>
    /// <returns>true if there is a next item in the list</returns>
    public static bool Next<T>(this IList<T> elements, ref int currentIndex, bool wrap = false)
    {
        int count = elements.Count;
        if (count == 0)
        {
            return false;
        }

        currentIndex++;

        if (currentIndex >= count)
        {
            if (wrap)
            {
                currentIndex = 0;
                return true;
            }
            currentIndex = count - 1;
            return false;
        }

        return true;
    }
}
