using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Infrastructure.Configuration;

public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToTable("candidate");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                result => CandidateId.Create(result)
            );

        builder.ComplexProperty(a => a.FullName, fb =>
        {
            fb.Property(a => a!.FirstName).IsRequired(false).HasColumnName("first_name");
            fb.Property(a => a!.SecondName).IsRequired(false).HasColumnName("second_name");
        });

        builder.ComplexProperty(w => w.ContactInfo,
            db =>
            {
                db.Property(p => p.Email)
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_100)
                    .HasColumnName("email");
                db.Property(p => p.Phone)
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_25)
                    .HasColumnName("phone");
            }
        );

        builder.Property(h => h.ResumeUrl)
            .IsRequired()
            .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH_500)
            .HasColumnName("resume_url");
    }
}