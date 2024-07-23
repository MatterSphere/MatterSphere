namespace Models.Interfaces
{
    public interface ISuggestionsFactory
    {
        string[] CreateSuggestions(string value);
    }
}
