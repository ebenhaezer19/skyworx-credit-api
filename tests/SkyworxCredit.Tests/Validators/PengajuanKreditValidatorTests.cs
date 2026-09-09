using FluentAssertions;
using SkyworxCredit.Application.DTOs;
using SkyworxCredit.Application.Validators;
using Xunit;

namespace SkyworxCredit.Tests.Validators;

public class PengajuanKreditValidatorTests
{
    private readonly PengajuanKreditValidator _validator;

    public PengajuanKreditValidatorTests()
    {
        _validator = new PengajuanKreditValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldPass()
    {
        // Arrange
        var request = new PengajuanKreditRequest
        {
            Plafon = 100000000,
            Bunga = 12,
            Tenor = 60
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithZeroPlafon_ShouldFail()
    {
        // Arrange
        var request = new PengajuanKreditRequest
        {
            Plafon = 0,
            Bunga = 12,
            Tenor = 60
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Plafon");
    }

    [Fact]
    public void Validate_WithBungaLessThanZero_ShouldFail()
    {
        // Arrange
        var request = new PengajuanKreditRequest
        {
            Plafon = 100000000,
            Bunga = -5,
            Tenor = 60
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Bunga");
    }

    [Fact]
    public void Validate_WithBungaGreaterThan100_ShouldFail()
    {
        // Arrange
        var request = new PengajuanKreditRequest
        {
            Plafon = 100000000,
            Bunga = 150,
            Tenor = 60
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Bunga");
    }

    [Fact]
    public void Validate_WithZeroTenor_ShouldFail()
    {
        // Arrange
        var request = new PengajuanKreditRequest
        {
            Plafon = 100000000,
            Bunga = 12,
            Tenor = 0
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Tenor");
    }
}