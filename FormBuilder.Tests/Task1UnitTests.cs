using FormBuilder.Core.Models;
using FormBuilder.Core;

namespace FormBuilder.Tests;

[TestFixture]
public class Task1UnitTests
{
    private FormTemplate _formTemplate;

    [SetUp]
    public void SetUp()
    {
        var fields = new List<Field>
        {
            new UserField("WorkHours"),
            new UserField("TasksCompleted"),
            new CalculatedField("TotalScore", new List<string> { "WorkHours", "TasksCompleted" })
        };

        _formTemplate = new FormTemplate("Performance Evaluation", fields);
    }

    [Test]
    public void GetFieldValue_ShouldReturnUserFieldValue_WhenFieldIsUserField()
    {
        // Arrange
        var userFieldValues = new Dictionary<string, double>
        {
            { "WorkHours", 40.0 },
            { "TasksCompleted", 10.0 }
        };

        // Act
        var result = FormService.GetFieldValue(_formTemplate, userFieldValues, "WorkHours");

        // Assert
        Assert.That(result, Is.EqualTo(40.0));
    }

    [Test]
    public void GetFieldValue_ShouldReturnCalculatedFieldValue_WhenFieldIsCalculatedField()
    {
        // Arrange
        var userFieldValues = new Dictionary<string, double>
        {
            { "WorkHours", 40.0 },
            { "TasksCompleted", 10.0 }
        };

        // Act
        var result = FormService.GetFieldValue(_formTemplate, userFieldValues, "TotalScore");

        // Assert
        Assert.That(result, Is.EqualTo(50.0));  
    }

    [Test]
    public void GetFieldValue_ShouldThrowArgumentException_WhenFieldDoesNotExist()
    {
        // Arrange
        var userFieldValues = new Dictionary<string, double>
        {
            { "WorkHours", 40.0 },
            { "TasksCompleted", 10.0 }
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => FormService.GetFieldValue(_formTemplate, userFieldValues, "NonExistentField"));
    }

    [Test]
    public void GetFieldValue_ShouldThrowArgumentException_WhenDependentFieldIsMissing()
    {
        // Arrange
        var userFieldValues = new Dictionary<string, double>
        {
            { "WorkHours", 40.0 }
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => FormService.GetFieldValue(_formTemplate, userFieldValues, "TotalScore"));
    }

    [Test]
    public void GetFieldValue_ShouldThrowKeyNotFoundException_WhenUserFieldIsMissing()
    {
        // Arrange
        var userFieldValues = new Dictionary<string, double>
        {
            { "TasksCompleted", 10.0 }
        };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => FormService.GetFieldValue(_formTemplate, userFieldValues, "WorkHours"));
    }
}