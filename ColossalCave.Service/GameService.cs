namespace ColossalCave.Service;

public interface IGameService
{
    string ProcessCommand(string command);
    string GetWelcomeMessage();
}

public class GameService : IGameService
{
    private readonly ICommandParser _commandParser;

    public GameService(ICommandParser commandParser)
    {
        _commandParser = commandParser;
    }

    public string ProcessCommand(string command)
    {
        var commandObject = _commandParser.Parse(command);
        // TODO: Process tokens and implement game logic
        return $"Processed command: Verb = {commandObject.Verb}, Subject = {commandObject.Subject}, Tool = {commandObject.Tool}";
    }

    public string GetWelcomeMessage()
    {
        return "Welcome to Colossal Cave Adventure!";
    }
}

public interface ICommandParser
{
    Command Parse(string input);
}

public class CommandParser : ICommandParser
{
    private static readonly HashSet<string> FillerWords = new HashSet<string> { "a", "the", "an", "in", "on", "at", "to", "from", "and" };

    public Command Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new Command { Verb = string.Empty, Subject = string.Empty, Tool = string.Empty };

        // Split the input into tokens based on spaces and punctuation
        var tokens = input.Split(new[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(token => token.Trim().ToLower())
                          .Where(token => !FillerWords.Contains(token))
                          .ToList();

        // Find the index of "with" to determine if a tool is used
        int withIndex = tokens.IndexOf("with");
        string verb, subject, tool;

        if (withIndex > 0 && withIndex < tokens.Count - 1)
        {
            // Extract verb, subject, and tool
            verb = tokens[0];
            subject = string.Join(" ", tokens.Skip(1).Take(withIndex - 1));
            tool = string.Join(" ", tokens.Skip(withIndex + 1));
        }
        else
        {
            // Assume the first token is the verb and the second token is the subject
            verb = tokens.ElementAtOrDefault(0) ?? string.Empty;
            subject = tokens.ElementAtOrDefault(1) ?? string.Empty;
            tool = string.Empty;
        }

        return new Command { Verb = verb, Subject = subject, Tool = tool };
    }
}

public class Command
{
    public string Verb { get; set; }
    public string Subject { get; set; }
    public string Tool { get; set; }
}