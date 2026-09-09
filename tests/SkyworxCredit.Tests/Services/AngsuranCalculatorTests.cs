using FluentAssertions;
using Moq;
using SkyworxCredit.Application.DTOs;
using SkyworxCredit.Application.Services;
using SkyworxCredit.Application.Interfaces;
using Xunit;

namespace SkyworxCredit.Tests.Services;

public class AngsuranCalculatorTests
{
    private readonly PengajuanKreditService _service;
    private readonly Mock<IPengajuanKreditRepository> _mockRepo;

    public AngsuranCalculatorTests()
    {
        _mockRepo = new Mock<IPengajuanKreditRepository>();
        _service = new PengajuanKreditService(_mockRepo.Object);
    }

    [Fact]
    public void HitungAngsuran_WithValidData_ShouldReturnCorrectInstallment()
    {
        // Arrange
        var request = new AngsuranCalculationRequest
        {
            Plafon = 100000000,
            Bunga = 12,
            Tenor = 60
        };

        // Act
        var result = _service.HitungAngsuran(request);

        // Assert
        result.Should().NotBeNull();
        result.AngsuranPerBulan.Should().BeApproximately(2224444.77m, 0.01m);
        result.TotalPembayaran.Should().BeApproximately(133466686.20m, 0.01m);
        result.TotalBunga.Should().BeApproximately(33466686.20m, 0.01m);
    }

    [Fact]
    public void HitungAngsuran_WithZeroBunga_ShouldReturnPlafonDividedByTenor()
    {
        // Arrange
        var request = new AngsuranCalculationRequest
        {
            Plafon = 100000000,
            Bunga = 0,
            Tenor = 12
        };

        // Act
        var result = _service.HitungAngsuran(request);

        // Assert
        result.AngsuranPerBulan.Should().Be(8333333.33m);
        result.TotalBunga.Should().BeApproximately(0, 0.1m);  // ← Tolerance 0.1
        result.TotalPembayaran.Should().BeApproximately(100000000, 0.1m);
    }

    [Fact]
    public void HitungAngsuran_WithLargeValues_ShouldNotThrowOverflow()
    {
        // Arrange
        var request = new AngsuranCalculationRequest
        {
            Plafon = 10000000000m,
            Bunga = 18,
            Tenor = 120
        };

        // Act
        var result = _service.HitungAngsuran(request);

        // Assert
        result.Should().NotBeNull();
        result.AngsuranPerBulan.Should().BeGreaterThan(0);
    }
}