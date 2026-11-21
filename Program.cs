using System.Text;
using translator.Exceptions;
using translator.Levels;
using translator.Models;
using translator.Services;

UserStore users = new UserStore(Path.Combine(FileStorage.DataDir, "users.txt"));
GameData data = new GameData(
    new DictionaryStore(Path.Combine(FileStorage.DataDir, "dictionary.txt")),
    new SentenceStore(Path.Combine(FileStorage.DataDir, "sentences.txt")));
BookStore books = new BookStore(Path.Combine(FileStorage.DataDir, "Books"));

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

while (true)
{
    ConsoleUi.Title("LinguaTranslator — translator and language learning");
    int choice = ConsoleUi.AskChoice(new[] { "Register", "Log in", "Exit" });

    User? user;
    if (choice == 0)
    {
        user = Register();
    }
    else if (choice == 1)
    {
        user = Login();
    }
    else
    {
        ConsoleUi.Info("Goodbye!");
        return;
    }

    if (user != null)
    {
        MainMenu(user);
    }
}

User? Register()
{
    ConsoleUi.Title("Registration");

    string name;
    while (true)
    {
        name = ConsoleUi.Ask("Username (Enter = back): ");
        if (name.Length == 0)
        {
            return null;
        }
        if (name.Length < 3 || !name.All(c => char.IsLetterOrDigit(c) || c == '_'))
        {
            ConsoleUi.Bad("Username must be at least 3 letters/digits (underscore allowed).");
            continue;
        }
        if (users.Find(name) != null)
        {
            ConsoleUi.Bad(new UserAlreadyExistsException(name).Message);
            continue;
        }
        break;
    }

    string password;
    while (true)
    {
        password = ConsoleUi.AskSecret("Password (at least 4 characters): ");
        if (password.Length < 4)
        {
            ConsoleUi.Bad("Password is too short.");
            continue;
        }
        string again = ConsoleUi.AskSecret("Repeat password: ");
        if (again != password)
        {
            ConsoleUi.Bad("Passwords do not match.");
            continue;
        }
        break;
    }

    Console.WriteLine("Which language do you want to learn?");
    int languageChoice = ConsoleUi.AskChoice(new[] { "English", "Русский (Russian)" });
    Lang learning = languageChoice == 0 ? Lang.En : Lang.Ru;

    try
    {
        User user = users.Create(name, password, learning);
        ConsoleUi.Good($"Registration complete! Welcome, {user.Username}. Language to learn: {LangInfo.Name(learning)}");
        return user;
    }
    catch (UserAlreadyExistsException ex)
    {
        ConsoleUi.Bad(ex.Message);
        return null;
    }
}

User? Login()
{
    ConsoleUi.Title("Log in");
    for (int attempt = 1; attempt <= 3; attempt++)
    {
        string name = ConsoleUi.Ask("Username (Enter = back): ");
        if (name.Length == 0)
        {
            return null;
        }
        string password = ConsoleUi.AskSecret("Password: ");
        try
        {
            User user = users.Authenticate(name, password);
            ConsoleUi.Good($"Welcome, {user.Username}!");
            return user;
        }
        catch (InvalidCredentialsException ex)
        {
            ConsoleUi.Bad($"{ex.Message} ({attempt}/3)");
        }
    }
    return null;
}

void MainMenu(User user)
{
    while (true)
    {
        ConsoleUi.Title(Texts.Both("toc", user));
        ConsoleUi.Info($"{user.Username} | {user.Nickname} | {user.Points} points | level: {Math.Min(user.Level, User.MaxLevel)}/{User.MaxLevel}");
        string[] items =
        {
            Texts.Both("translator", user),
            Texts.Both("lessons", user),
            Texts.Both("test", user),
            Texts.Both("profile", user),
            Texts.Both("exit", user)
        };
        int choice = ConsoleUi.AskChoice(items);
        switch (choice)
        {
            case 0:
                RunTranslator();
                break;
            case 1:
                RunLessons(user);
                break;
            case 2:
                RunTest(user);
                break;
            case 3:
                if (ShowProfile(user))
                {
                    return;
                }
                break;
            default:
                users.Save();
                return;
        }
    }
}

Lang PickLanguage(string prompt, Lang? exclude)
{
    Lang[] options = LangInfo.All.Where(l => l != exclude).ToArray();
    Console.WriteLine(prompt);
    int index = ConsoleUi.AskChoice(options.Select(LangInfo.Name).ToArray());
    return options[index];
}

void RunTranslator()
{
    ConsoleUi.Title("Translator");
    Lang from = PickLanguage("Translate from:", null);
    Lang to = PickLanguage("Translate to:", from);

    while (true)
    {
        string word = ConsoleUi.Ask($"[{LangInfo.Name(from)} → {LangInfo.Name(to)}] word or phrase (Enter = back): ");
        if (word.Length == 0)
        {
            return;
        }

        try
        {
            ConsoleUi.Good("  " + data.Dictionary.Translate(from, to, word));
        }
        catch (WordNotFoundException ex)
        {
            ConsoleUi.Info("  " + ex.Message);
            string added = ConsoleUi.Ask($"  Enter the translation ({LangInfo.Name(to)}) to add it to the dictionary (Enter = skip): ");
            if (added.Length > 0)
            {
                data.Dictionary.Add(from, to, word, added);
                ConsoleUi.Good("  Added to the dictionary file.");
            }
        }
    }
}

void RunLessons(User user)
{
    ConsoleUi.Title(Texts.Both("lessons", user));
    if (books.Books.Count == 0)
    {
        ConsoleUi.Bad("No books found (Data/Books).");
        return;
    }

    List<string> titles = new List<string>();
    for (int b = 0; b < books.Books.Count; b++)
    {
        string mark = user.ReadBooks.Contains(b) ? "[✓] " : "[ ] ";
        titles.Add(mark + books.Books[b].Title[(int)user.Native] + " / " + books.Books[b].Title[(int)user.Learning]);
    }
    titles.Add("← Back");
    int pick = ConsoleUi.AskChoice(titles.ToArray());
    if (pick == books.Books.Count)
    {
        return;
    }
    Book book = books.Books[pick];

    Console.WriteLine("How do you want to read?");
    int mode = ConsoleUi.AskChoice(new[]
    {
        $"Top: {LangInfo.Name(user.Learning)}, below: {LangInfo.Name(user.Native)} translation",
        $"Top: {LangInfo.Name(user.Native)}, below: {LangInfo.Name(user.Learning)} translation"
    });
    Lang top = mode == 0 ? user.Learning : user.Native;
    Lang bottom = mode == 0 ? user.Native : user.Learning;

    ConsoleUi.Title(book.Title[(int)top]);
    for (int i = 0; i < book.Paragraphs.Count; i++)
    {
        string[] paragraph = book.Paragraphs[i];
        Console.WriteLine();
        ConsoleUi.Highlight($"{i + 1}. {paragraph[(int)top]}");
        ConsoleUi.Info($"   → {paragraph[(int)bottom]}");
        string answer = ConsoleUi.Ask("   (Enter = next, q = quit) ");
        if (answer.ToLowerInvariant() == "q")
        {
            return;
        }
    }
    user.ReadBooks.Add(pick);
    users.Save();
    ConsoleUi.Good("Book finished!");
}

void RunTest(User user)
{
    while (true)
    {
        ConsoleUi.Title(Texts.Both("test", user));
        string[] items = new string[User.MaxLevel + 1];
        for (int n = 1; n <= User.MaxLevel; n++)
        {
            string status = n < user.Level ? "[✓]" : n == user.Level ? "[>]" : "[ ]";
            items[n - 1] = $"{status} {n}. {Texts.Both("level" + n, user)}";
        }
        items[User.MaxLevel] = "← Back";

        int pick = ConsoleUi.AskChoice(items);
        if (pick == User.MaxLevel)
        {
            return;
        }

        int number = pick + 1;
        if (number > user.Level)
        {
            ConsoleUi.Bad("This level is locked — complete the previous levels first.");
            continue;
        }

        bool practice = number < user.Level;
        Level level = LevelFactory.Create(number, data, user);
        bool passed = level.Play(practice);
        users.Save();

        if (passed && !practice && number == User.MaxLevel)
        {
            Finale(user);
        }
    }
}

void Finale(User user)
{
    Console.WriteLine();
    ConsoleUi.Highlight("      ★            ★            ★");
    ConsoleUi.Highlight("     ★★★          ★★★          ★★★");
    ConsoleUi.Highlight("   ★★★★★★★      ★★★★★★★      ★★★★★★★");
    ConsoleUi.Highlight("     ★★★          ★★★          ★★★");
    Console.WriteLine();
    ConsoleUi.Good(Texts.Get("congrats", user.Learning));
    ConsoleUi.Good(Texts.Get("congrats", user.Native));
    ConsoleUi.Highlight($"Nickname: {user.Nickname} | Points: {user.Points}");
    ConsoleUi.Info("Tip: you can start everything again in Profile -> Reset progress.");
}

bool ShowProfile(User user)
{
    ConsoleUi.Title(Texts.Both("profile", user));
    Console.WriteLine($"Username      : {user.Username}");
    Console.WriteLine($"Nickname      : {user.Nickname}");
    Console.WriteLine($"Learning      : {LangInfo.Name(user.Learning)}");
    Console.WriteLine($"Points        : {user.Points}");
    Console.WriteLine($"Books read    : {user.ReadBooks.Count}/{books.Books.Count}");
    if (user.Completed)
    {
        ConsoleUi.Highlight("Level         : all 8 levels completed  ★ ★ ★");
    }
    else
    {
        Console.WriteLine($"Level         : {user.Level}/{User.MaxLevel}");
    }

    int action = ConsoleUi.AskChoice(new[]
    {
        "Back",
        "Reset progress (start all levels and books again)",
        "Delete account"
    });
    if (action == 1)
    {
        string confirm = ConsoleUi.Ask("This will erase your points, level and books progress. Type YES to confirm: ");
        if (confirm.Equals("yes", StringComparison.OrdinalIgnoreCase))
        {
            users.ResetProgress(user);
            ConsoleUi.Good("Progress has been reset. You can start again from level 1.");
        }
        else
        {
            ConsoleUi.Info("Cancelled.");
        }
    }
    else if (action == 2)
    {
        string confirm = ConsoleUi.Ask($"This will permanently delete your account. Type your username ({user.Username}) to confirm: ");
        if (confirm.Equals(user.Username, StringComparison.OrdinalIgnoreCase))
        {
            users.Delete(user);
            ConsoleUi.Good("Your account has been deleted.");
            return true;
        }
        ConsoleUi.Info("Cancelled.");
    }
    return false;
}