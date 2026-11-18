namespace Assignment.GameLogic
{
    // Abstraction over randomness so the game can be unit tested
    public interface IDice
    {
        // Returns a number between 1 and 100
        int Roll();
    }

    public class RandomDice : IDice
    {
        public int Roll() => Random.Shared.Next(1, 101);
    }
}
