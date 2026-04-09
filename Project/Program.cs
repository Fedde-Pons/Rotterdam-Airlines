namespace project;

class Program
{
    static void Main(string[] args)
    {
        DatabaseInitializer initializer = new();
        initializer.Initialize();
        Menu.Start();
    }
}