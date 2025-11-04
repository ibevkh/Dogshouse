namespace Dogshouse.Exceptions;

public class DuplicateDogNameException : Exception
{
    public DuplicateDogNameException(string name)
       : base($"Dog with name '{name}' already exists.") { }
}
