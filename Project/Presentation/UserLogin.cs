static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address:");
        string email = Console.ReadLine();
        Console.WriteLine("\nPlease enter your password:");
        string password = Console.ReadLine();
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        if (acc != null)
        {
            Console.Clear();
            Console.WriteLine("Welcome back " + acc.FullName);
            Console.WriteLine("Your email address is " + acc.EmailAddress);
        }
        else
        {
            Console.Clear();
            Console.WriteLine("No account found with that email and password");
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}