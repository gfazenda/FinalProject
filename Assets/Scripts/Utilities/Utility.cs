﻿public static class Utility {
    
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

    public static float distanceMultiplier = 1.42f;
}

public static class Events
{
    public const string PlayerTurn = "PlayerTurn";
    public const string EnemiesTurn = "EnemiesTurn";
    public const string EnemiesCreated = "EnemiesCreated";
    public const string LevelWon = "LevelWon";
    public const string LevelLost = "LevelLost";
    public const string LevelLoaded = "LevelLoaded";
    public const string DamageUpdate = "DamageUpdate";
    public const string PlayerConfigured = "PlayerConfigured";
    public const string DisableMoveButtons = "DisableMoveButtons";
    public const string EnableMoveButtons = "EnableMoveButtons";
}


public static class Tags
{
    public const string Enemy = "Enemy";
    public const string Player = "Player";
    public const string Poison = "Poison";
    public const string Marker = "Marker";
    public const string Mine = "Mine";
    public const string Explosion = "Explosion";
    public const string ElectricExplosion = "ElectricExplosion";
    public const string Ground = "Ground";
    public const string PoisonExplosion = "PoisonExplosion";

}

public static class Skills
{
    public const string Overcharge = "Overcharge";
    public const string RemoteMine = "RemoteMine";
    public const string Missile = "Missile";
}




