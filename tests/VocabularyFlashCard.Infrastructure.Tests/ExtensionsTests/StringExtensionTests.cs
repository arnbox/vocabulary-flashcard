using VocabularyFlashCard.Infrastructure.Extensions;

namespace VocabularyFlashCard.Infrastructure.Tests.ExtensionsTests;

public class StringExtensionTests
{
    public class SampleInput
    {
        //public string TextField;
        public string? TextProperty { get; set; }
        // public string[] TextPropertyArray { get; set; }
        // public IEnumerable<string> TextProperties { get; set; }
        public int Number { get; set; }
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("A", "A")]
    [InlineData("   A", "A")]
    [InlineData("   A   ", "A")]
    [InlineData("       ", "")]
    public void TrimStringProperties(string input, string expected)
    {
        // Arrange
        var sampleInput = new SampleInput
        {
            TextProperty = input
        };

        // Act
        sampleInput.TrimAllStrings<SampleInput>();

        // Assert
        Assert.Equal(expected, sampleInput.TextProperty);
    }

    //[Theory]
    //[InlineData(" A ", "A")]
    //public void TrimStringFields(string input, string expected)
    //{
    //    // Arrange
    //    var sampleInput = new SampleInput
    //    {
    //        TextField = input
    //    };

    //    // Act
    //    sampleInput.TrimAllStrings<SampleInput>();

    //    // Assert
    //    Assert.Equal(expected, sampleInput.TextField);
    //}

    //[Theory]
    //[InlineData(" A ", "A")]
    //public void TrimStringArray(string input, string expected)
    //{
    //    // Arrange
    //    var sampleInput = new SampleInput
    //    {
    //        TextPropertyArray = new string[] { input }
    //    };

    //    // Act
    //    sampleInput.TrimAllStrings();

    //    // Assert
    //    Assert.Equal(expected, sampleInput.TextPropertyArray[0]);

    //}

    //[Theory]
    //[InlineData(" A ", "A")]
    //public void TrimStringPropertiesIEnumerable(string input, string expected)
    //{
    //    // Arrange
    //    var sampleInput = new SampleInput
    //    {
    //        TextProperties = new List<string>() { input }
    //    };

    //    // Act
    //    sampleInput.TrimAllStrings();

    //    // Assert
    //    Assert.Equal(expected, sampleInput.TextProperties.First());

    //}
}