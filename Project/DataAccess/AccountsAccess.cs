using Microsoft.Data.Sqlite;
using Dapper;

public class AccountsAccess
{
    private readonly string _connectionString = "Data Source=DataSources/project.db";
    private readonly string Table = "Accounts";

    public void CreateTable()
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        CREATE TABLE IF NOT EXISTS {Table} (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            emailAddress TEXT NOT NULL UNIQUE,
            phoneNumber TEXT,
            firstName TEXT,
            lastName TEXT,
            dateOfBirth TEXT,
            password TEXT,
            createdAt TEXT
        );";

        connection.Execute(sql);
    }

    public void Write(AccountModel account)
    {
        using var connection = new SqliteConnection(_connectionString);

        account.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string sql = $@"
        INSERT INTO {Table}
        (emailAddress, phoneNumber, firstName, lastName, dateOfBirth, password, createdAt)
        VALUES
        (@EmailAddress, @PhoneNumber, @FirstName, @LastName, @DateOfBirth, @Password, @CreatedAt);";

        connection.Execute(sql, account);
    }

    public bool EmailExists(string email)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"SELECT COUNT(1) FROM {Table} WHERE emailAddress = @Email;";
        int count = connection.ExecuteScalar<int>(sql, new { Email = email });

        return count > 0;
    }

    public AccountModel GetByEmail(string email)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"SELECT * FROM {Table} WHERE emailAddress = @Email;";
        return connection.QueryFirstOrDefault<AccountModel>(sql, new { Email = email });
    }

    public AccountModel GetById(long id)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"SELECT * FROM {Table} WHERE id = @Id;";
        return connection.QueryFirstOrDefault<AccountModel>(sql, new { Id = id });
    }

    public List<AccountModel> GetAll()
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"SELECT * FROM {Table};";
        return connection.Query<AccountModel>(sql).ToList();
    }

    public void Update(AccountModel account)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        UPDATE {Table}
        SET
            emailAddress = @EmailAddress,
            phoneNumber = @PhoneNumber,
            firstName = @FirstName,
            lastName = @LastName,
            dateOfBirth = @DateOfBirth,
            password = @Password
        WHERE id = @Id;";

        connection.Execute(sql, account);
    }

    public void Delete(long id)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"DELETE FROM {Table} WHERE id = @Id;";
        connection.Execute(sql, new { Id = id });
    }

    public void ResetAccountsTable()
    {
        using var connection = new SqliteConnection(_connectionString);

        string dropSql = $@"DROP TABLE IF EXISTS {Table};";
        connection.Execute(dropSql);

        string createSql = $@"
        CREATE TABLE {Table} (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            emailAddress TEXT NOT NULL UNIQUE,
            phoneNumber TEXT,
            firstName TEXT,
            lastName TEXT,
            dateOfBirth TEXT,
            password TEXT,
            createdAt TEXT
        );";

        connection.Execute(createSql);
    }
}