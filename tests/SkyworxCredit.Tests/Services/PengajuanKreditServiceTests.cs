using FluentAssertions;
using Moq;
using SkyworxCredit.Application.DTOs;
using SkyworxCredit.Application.Services;
using SkyworxCredit.Domain.Entities;
using SkyworxCredit.Application.Interfaces;

namespace SkyworxCredit.Tests.Services;

public class PengajuanKreditServiceTests
{
    private readonly Mock<IPengajuanKreditRepository> _mockRepo;
    private readonly PengajuanKreditService _service;

    public PengajuanKreditServiceTests()
    {
        _mockRepo = new Mock<IPengajuanKreditRepository>();
        _service = new PengajuanKreditService(_mockRepo.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResponse()
    {
        // Arrange
        var request = new PengajuanKreditRequest
        {
            Plafon = 100000000,
            Bunga = 12,
            Tenor = 60
        };

        var expectedEntity = new PengajuanKredit
        {
            Id = Guid.NewGuid(),
            Plafon = 100000000,
            Bunga = 12,
            Tenor = 60,
            Angsuran = 2224444.77m,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mockRepo.Setup(x => x.CreateAsync(It.IsAny<PengajuanKredit>()))
            .ReturnsAsync(expectedEntity);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Plafon.Should().Be(100000000);
        result.Tenor.Should().Be(60);
        result.Angsuran.Should().BeApproximately(2224444.77m, 0.01m);
        _mockRepo.Verify(x => x.CreateAsync(It.IsAny<PengajuanKredit>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnData()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedEntity = new PengajuanKredit
        {
            Id = id,
            Plafon = 100000000,
            Bunga = 12,
            Tenor = 60,
            Angsuran = 2224444.77m,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mockRepo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(expectedEntity);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Plafon.Should().Be(100000000);
        _mockRepo.Verify(x => x.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((PengajuanKredit?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
        _mockRepo.Verify(x => x.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_ShouldReturnUpdatedData()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new PengajuanKreditRequest
        {
            Plafon = 150000000,
            Bunga = 10,
            Tenor = 48
        };

        var existingEntity = new PengajuanKredit
        {
            Id = id,
            Plafon = 100000000,
            Bunga = 12,
            Tenor = 60,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var updatedEntity = new PengajuanKredit
        {
            Id = id,
            Plafon = 150000000,
            Bunga = 10,
            Tenor = 48,
            Angsuran = 3806799.00m,
            CreatedAt = existingEntity.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        _mockRepo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(existingEntity);
        _mockRepo.Setup(x => x.UpdateAsync(It.IsAny<PengajuanKredit>())).ReturnsAsync(updatedEntity);

        // Act
        var result = await _service.UpdateAsync(id, request);

        // Assert
        result.Should().NotBeNull();
        result!.Plafon.Should().Be(150000000);
        result.Tenor.Should().Be(48);
        _mockRepo.Verify(x => x.GetByIdAsync(id), Times.Once);
        _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<PengajuanKredit>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new PengajuanKreditRequest
        {
            Plafon = 150000000,
            Bunga = 10,
            Tenor = 48
        };

        _mockRepo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((PengajuanKredit?)null);

        // Act
        var result = await _service.UpdateAsync(id, request);

        // Assert
        result.Should().BeNull();
        _mockRepo.Verify(x => x.GetByIdAsync(id), Times.Once);
        _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<PengajuanKredit>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepo.Setup(x => x.ExistsAsync(id)).ReturnsAsync(true);
        _mockRepo.Setup(x => x.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        result.Should().BeTrue();
        _mockRepo.Verify(x => x.ExistsAsync(id), Times.Once);
        _mockRepo.Verify(x => x.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepo.Setup(x => x.ExistsAsync(id)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        result.Should().BeFalse();
        _mockRepo.Verify(x => x.ExistsAsync(id), Times.Once);
        _mockRepo.Verify(x => x.DeleteAsync(id), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfData()
    {
        // Arrange
        var entities = new List<PengajuanKredit>
        {
            new() {
                Id = Guid.NewGuid(),
                Plafon = 100000000,
                Bunga = 12,
                Tenor = 60,
                Angsuran = 2224444.77m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() {
                Id = Guid.NewGuid(),
                Plafon = 50000000,
                Bunga = 10,
                Tenor = 24,
                Angsuran = 2300000.00m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(entities);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        _mockRepo.Verify(x => x.GetAllAsync(), Times.Once);
    }
}