using Appointments.Domain.Entities.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.Persistence.Configurations;

public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.HasOne(cr => cr.User1)
            .WithMany()
            .HasForeignKey(cr => cr.User1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cr => cr.User2)
            .WithMany()
            .HasForeignKey(cr => cr.User2Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
