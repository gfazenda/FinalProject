public static class Utility {
    
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random prng = new System.Random(seed);//System.DateTime.Today.Millisecond);
        for (int i = 0; i < array.Length -1; i++)
        {
            int randomIndex = prng.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }

}

public static class Events
{
    public const string PlayerTurn = "PlayerTurn";
    public const string EnemiesTurn = "EnemiesTurn";
    public const string EnemiesCreated = "EnemiesCreated";

}


public static class Tags
{
    public const string Enemy = "Enemy";

}


