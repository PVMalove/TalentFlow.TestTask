using FluentAssertions;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects;
using TalentFlow.Domain.ValueObjects.EntityIds;
using Xunit;

namespace TalentFlow.Domain.Tests;

public class HrSpecialistTests
{
    [Fact]
    public void Create_WithValidData_ReturnsHrSpecialist()
    {
        // Arrange
        var id = HrSpecialistId.NewId();
        var fullName = FullName.Create("John", "Doe").Value;
        var contactInfo = ContactInfo.Create("john.doe@example.com", "123456789").Value;

        // Act
        var result = HrSpecialist.Create(id, fullName, contactInfo);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(id);
        result.Value.FullName.Should().Be(fullName);
        result.Value.ContactInfo.Should().Be(contactInfo);
    }

    [Fact]
    public void Create_WithEmptyFirstName_ReturnsError()
    {
        // Arrange
        var id = HrSpecialistId.NewId();
        var fullName = FullName.Create("", "Doe").Value;
        var contactInfo = ContactInfo.Create("john.doe@example.com", "123456789").Value;

        // Act
        var result = HrSpecialist.Create(id, fullName, contactInfo);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.General.ValueIsInvalid("FirstName can not be empty"));
    }

    [Fact]
    public void Create_WithEmptySecondName_ReturnsError()
    {
        // Arrange
        var id = HrSpecialistId.NewId();
        var fullName = FullName.Create("John", "").Value;
        var contactInfo = ContactInfo.Create("john.doe@example.com", "123456789").Value;

        // Act
        var result = HrSpecialist.Create(id, fullName, contactInfo);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.General.ValueIsInvalid("SecondName can not be empty"));
    }

    [Fact]
    public void AssignToVacancy_WithinLimit_SuccessfullyAssignsVacancy()
    {
        // Arrange
        var hrSpecialist = CreateHrSpecialist();
        var vacancy = Vacancy.Create(VacancyId.NewId(), DepartmentId.NewId(), HrSpecialistId.NewId(), "title",
            "description").Value;

        // Act
        var result = hrSpecialist.AssignToVacancy(vacancy);

        // Assert
        result.IsSuccess.Should().BeTrue();
        hrSpecialist.AssignedVacancies.Should().Contain(vacancy);
    }

    [Fact]
    public void AssignToVacancy_ExceedsLimit_ReturnsError()
    {
        // Arrange
        var hrSpecialist = CreateHrSpecialist();
        var vacancies = Enumerable.Range(0, 5)
            .Select(i => Vacancy.Create(VacancyId.NewId(), DepartmentId.NewId(), HrSpecialistId.NewId(), $"Vacancy {i}", "description").Value)
            .ToList();

        foreach (var vacancy in vacancies)
        {
            hrSpecialist.AssignToVacancy(vacancy);
        }

        var newVacancy = Vacancy.Create(VacancyId.NewId(), DepartmentId.NewId(), HrSpecialistId.NewId(), "title", "description").Value;

        // Act
        var result = hrSpecialist.AssignToVacancy(newVacancy, maxVacancies: 5);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.General.ValueIsInvalid("HR has too many assigned vacancies"));
        hrSpecialist.AssignedVacancies.Should().HaveCount(5);
    }

    [Fact]
    public void RemoveVacancy_WithExistingVacancy_SuccessfullyRemovesVacancy()
    {
        // Arrange
        var hrSpecialist = CreateHrSpecialist();
        var vacancy = Vacancy.Create(VacancyId.NewId(), DepartmentId.NewId(), HrSpecialistId.NewId(), "title", "description").Value;
        hrSpecialist.AssignToVacancy(vacancy);

        // Act
        var result = hrSpecialist.RemoveVacancy(vacancy.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        hrSpecialist.AssignedVacancies.Should().NotContain(vacancy);
    }

    [Fact]
    public void RemoveVacancy_WithNonExistingVacancy_ReturnsError()
    {
        // Arrange
        var hrSpecialist = CreateHrSpecialist();
        var nonExistingVacancyId = VacancyId.NewId();

        // Act
        var result = hrSpecialist.RemoveVacancy(nonExistingVacancyId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.General.NotFound(nonExistingVacancyId));
    }

    private HrSpecialist CreateHrSpecialist()
    {
        var id = HrSpecialistId.NewId();
        var fullName = FullName.Create("John", "Doe").Value;
        var contactInfo = ContactInfo.Create("john.doe@example.com", "123456789").Value;
        return HrSpecialist.Create(id, fullName, contactInfo).Value;
    }
}