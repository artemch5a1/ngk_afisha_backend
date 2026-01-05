using EventService.Domain.Enums;
using EventService.Domain.Models;

namespace EventService.UnitTests.Models;

public class MemberTests
    {
        [Fact]
        public void Create_ShouldInitializeWithReviewStatus()
        {
            // Arrange
            var invitationId = Guid.NewGuid();
            var studentId = Guid.NewGuid();

            // Act
            var member = Member.Create(invitationId, studentId);

            // Assert
            Assert.Equal(invitationId, member.InvitationId);
            Assert.Equal(studentId, member.StudentId);
            Assert.Equal(MemberStatus.Review, member.Status);
        }

        [Fact]
        public void Restore_ShouldRestoreAllProperties()
        {
            // Arrange
            var invitationId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var status = MemberStatus.Accepted;

            // Act
            var member = Member.Restore(invitationId, studentId, status);

            // Assert
            Assert.Equal(invitationId, member.InvitationId);
            Assert.Equal(studentId, member.StudentId);
            Assert.Equal(status, member.Status);
        }

        [Fact]
        public void ChangeStatus_ShouldUpdateMemberStatus()
        {
            // Arrange
            var member = Member.Create(Guid.NewGuid(), Guid.NewGuid());
            var newStatus = MemberStatus.Review;

            // Act
            member.ChangeStatus(newStatus);

            // Assert
            Assert.Equal(newStatus, member.Status);
        }

        [Fact]
        public void AddInvitationNavigation_ShouldSetInvitationReference()
        {
            // Arrange
            var invitation = Invitation.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                1,
                "short description valid",
                "long valid description with more than 35 chars",
                2,
                DateTime.UtcNow.AddDays(1)
            );
            var member = Member.Create(invitation.InvitationId, Guid.NewGuid());

            // Act
            member.AddInvitationNavigation(invitation);

            // Assert
            Assert.NotNull(member.Invitation);
            Assert.Equal(invitation, member.Invitation);
            Assert.Equal(invitation.InvitationId, member.InvitationId);
        }
    }