using Project;
using TestLib;
using TestLib.attributes;

namespace TestProject;

[TestClass]
public class Class1Test
{
    [TestMethod]
    public void Add_ReturnCorrectValue()
    {
        var proj = new Class1();
        var result = proj.Add(1, 2);
        Assert.AreNotEqual(result, 0);
    }
}