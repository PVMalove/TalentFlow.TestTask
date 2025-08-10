using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Enums;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Infrastructure.Configuration;

public class RecruitmentProcessConfiguration : IEntityTypeConfiguration<RecruitmentProcess>
{
    public void Configure(EntityTypeBuilder<RecruitmentProcess> builder)
    {
        builder.ToTable("recruitment_processes");

        builder.HasKey(rp => rp.Id);
        builder.Property(rp => rp.Id)
            .HasConversion(
                id => id.Value,
                result => RecruitmentProcessId.Create(result)
            );

        builder.HasOne<Vacancy>()
            .WithMany()
            .HasForeignKey(rp => rp.VacancyId)
            .HasPrincipalKey(v => v.Id)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne<Candidate>()
            .WithMany()
            .HasForeignKey(rp => rp.CandidateId)
            .HasPrincipalKey(c => c.Id)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(rp => rp.TestAssignment)
            .WithMany()
            .HasForeignKey(rp => rp.TestAssignmentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(rp => rp.CurrentStage)
            .HasConversion(
                s => s.ToString(),
                s => (RecruitmentStageType)Enum.Parse(typeof(RecruitmentStageType), s))
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_25);

        builder.Property(rp => rp.Stages)
            .HasConversion(
                valueObjects => JsonSerializer.Serialize(valueObjects, JsonSerializerOptions.Default),
                json =>
                    JsonSerializer.Deserialize<IReadOnlyList<RecruitmentStage>>(json, JsonSerializerOptions.Default)!,
                CreateCollectionValueComparer<RecruitmentStage>())
            .HasColumnType("nvarchar(max)")
            .HasColumnName("recruitment_stage");

        builder.Property(rp => rp.ProbationPassed)
            .HasDefaultValue(false)
            .HasColumnName("probation_passed");
    }

    private static ValueComparer<IReadOnlyList<T>> CreateCollectionValueComparer<T>() =>
        new(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
            c => c.ToList());
}