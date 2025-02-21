namespace SignalRApp;

public class Joke
{
    public required string Type { get; set; }

    public required string Setup { get; set; }

    public required string Punchline { get; set; }

    public int Id { get; set; }
}
