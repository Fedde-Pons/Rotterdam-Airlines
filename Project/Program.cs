namespace project;

class Program
{
    static void Main(string[] args)
    {
        var access = new AccountsAccess();
        access.CreateTable();

        var acc = access.GetByEmail("somaya@test.nl");

        if (acc != null)
        {
            acc.FirstName = "SomayaUpdated";
            access.Update(acc);

            var updated = access.GetByEmail("somaya@test.nl");

            Console.WriteLine("Updated name: " + updated.FirstName);
        }
        else
        {
            Console.WriteLine("Account not found");
        }
    }
}