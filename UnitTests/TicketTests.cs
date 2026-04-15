using System.Reflection;

namespace UnitTests;

[TestClass]
public sealed class TestTicket
{


    [TestMethod]
    // [DataRow("kevin@kevin.nl", "kevin")]
    public void SeatIsUnavailble()
    {
        // arrange
        // AccountsLogic l = new();
        // AccountsAccess access = new();
        TicketLogic ticket = new TicketLogic();

        // act 
        MethodInfo method = ticket.GetType().GetMethod("IsValidateSeatAvailable", BindingFlags.NonPublic | BindingFlags.Instance);

        object[] parmerters = new object[0] {};

        var result = method.Invoke(ticket, parmerters);



        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(false, result);
    }

    // [DataTestMethod]
    // [DataRow("kevin@kevin.nl", "wrong")] // wrong password
    // [DataRow("wrong1", "kevin")] // wrong email
    // [DataRow("wrong2", "wrong")] // everything wrong
    // [DataRow("", "")]
    // [DataRow(null, null)]
    // public void LoginInvalidCredentials(string m, string p)
    // {
    //     // arrange
    //     AccountsLogic l = new();

    //     // act 
    //     AccountModel result = l.CheckLogin(m, p);

    //     // assert
    //     Assert.IsNull(result);
    // }
}
