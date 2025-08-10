using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Enums;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Infrastructure.Configuration;

public class TestAssigmentConfiguration : IEntityTypeConfiguration<TestAssignment>
{
    public void Configure(EntityTypeBuilder<TestAssignment> builder)
    {
        builder.ToTable("test_assignment");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                result => TestAssignmentId.Create(result)
            );

        builder.Property(ta => ta.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH_2000)
            .HasColumnName("description");

        builder.Property(ta => ta.AssignedDate)
            .HasConversion(
                v => v!.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
            .IsRequired()
            .HasColumnName("assigned_date");

        builder.Property(ta => ta.SubmissionDeadline)
            .HasConversion(
                v => v!.Value.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
            .IsRequired(false)
            .HasColumnName("submission_deadline");

        builder.Property(rp => rp.Status)
            .HasConversion(
                s => s.ToString(),
                s => (AssignmentStatus)Enum.Parse(typeof(AssignmentStatus), s))
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_25)
            .HasColumnName("assignment_status");

        builder.Property(ta => ta.SubmissionUrl)
            .IsRequired(false)
            .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH_500);
    }
}