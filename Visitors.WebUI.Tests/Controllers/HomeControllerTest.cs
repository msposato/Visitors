using System.Web.Mvc;
using Visitors.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visitors.Domain;

namespace Visitors.WebUI.Tests.Controllers
{
  [TestClass]
  public class HomeControllerTest
  {
    protected readonly IVisitorRepository Db;

    public HomeControllerTest()
    {
      Db = new FakeClassVisitContext();
    }

    [TestMethod]
    public void CreateVisit()
    {
      // Arrange
      ClassVisitorController controller = new ClassVisitorController(Db);

      // Act
      ViewResult result = controller.Create() as ViewResult;

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void About()
    {
      // Arrange
      HomeController controller = new HomeController();

      // Act
      ViewResult result = controller.About() as ViewResult;

      // Assert
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Contact()
    {
      // Arrange
      HomeController controller = new HomeController();

      // Act
      ViewResult result = controller.Contact() as ViewResult;

      // Assert
      Assert.IsNotNull(result);
    }
  }
}