using FormBuilder.Core;
using FormBuilder.Core.Models;
using NUnit.Framework;

namespace FormBuilder.Tests
{
    [TestFixture]
    public class FormServiceTests
    {
        [Test]
        public void GetFieldValue_UserField_ReturnsCorrectValue()
        {
            // Arrange
            var formTemplate = new FormTemplate("TestForm", new List<Field>
        {
            new UserField("Field1")
        });

            var userFieldValues = new Dictionary<string, double>
        {
            { "Field1", 100 }
        };

            // Act
            var result = FormService.GetFieldValue(formTemplate, userFieldValues, "Field1");

            // Assert
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void GetFieldValue_CalculatedField_ReturnsCorrectValue()
        {
            // Arrange
            var formTemplate = new FormTemplate("TestForm", new List<Field>
        {
            new UserField("Field1"),
            new UserField("Field2"),
            new CalculatedField("Total", "1 + 1")
        });

            var userFieldValues = new Dictionary<string, double>
        {
            { "Field1", 50 },
            { "Field2", 30 }
        };

            // Act
            var result = FormService.GetFieldValue(formTemplate, userFieldValues, "Total");

            // Assert
            Assert.That(result, Is.EqualTo(80));
        }

        [Test]
        public void GetFieldValue_NonExistentField_ThrowsArgumentException()
        {
            // Arrange
            var formTemplate = new FormTemplate("TestForm", new List<Field>
        {
            new UserField("Field1")
        });

            var userFieldValues = new Dictionary<string, double>
        {
            { "Field1", 100 }
        };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => FormService.GetFieldValue(formTemplate, userFieldValues, "NonExistentField"));
            Assert.That(ex.Message, Is.EqualTo("Field 'NonExistentField' does not exist in the form template."));
        }
    }
}
