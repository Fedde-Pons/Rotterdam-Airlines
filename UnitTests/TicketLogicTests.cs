// using System.Reflection;

namespace UnitTests;

[TestClass]
public sealed class TestTicketLogic
{ 
    
    [TestMethod]
    public void CreateTicket_Succesfull()
    {
        //act & arrange
        (bool IsSuccesfull, string message, TicketModel? ticket) result =  TicketLogic.CreateTicket(1,1,1,1,(float)1.0,0);
        //assert 
        Assert.IsTrue(result.IsSuccesfull);
    }   
}
