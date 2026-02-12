using EventService.Domain.CustomExceptions;
using EventService.Domain.Enums;
using EventService.Domain.Models;
using FluentAssertions;

namespace EventService.UnitTests.Models
{
    public class InvitationTests
    {
        private readonly Guid _eventId = Guid.NewGuid();
        private readonly int _roleId = 1;
        private readonly string _shortDesc = new('a', 30);
        private readonly string _desc = new('b', 100);
        private readonly DateTime _deadline = DateTime.UtcNow.AddDays(1);

        [Fact]
        public void Create_Should_Create_Valid_Invitation()
        {
            // Act
            var invitation = Invitation.Create(
                Guid.NewGuid(),
                _eventId,
                _roleId,
                _shortDesc,
                _desc,
                3,
                _deadline
            );

            // Assert
            invitation.Should().NotBeNull();
            invitation.Status.Should().Be(InvitationStatus.Active);
            invitation.RequiredMember.Should().Be(3);
            invitation.AcceptedMember.Should().Be(0);
            invitation.Members.Should().BeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Create_Should_Throw_When_RequiredMember_Less_Than_1(int required)
        {
            Action act = () =>
                Invitation.Create(
                    Guid.NewGuid(),
                    _eventId,
                    _roleId,
                    _shortDesc,
                    _desc,
                    required,
                    _deadline
                );
            act.Should().Throw<DomainException>().WithMessage("*должно быть больше 0*");
        }

        [Fact]
        public void Create_Should_Throw_When_ShortDescription_Too_Short()
        {
            var shortDesc = new string('a', 5);
            Action act = () =>
                Invitation.Create(
                    Guid.NewGuid(),
                    _eventId,
                    _roleId,
                    shortDesc,
                    _desc,
                    2,
                    _deadline
                );

            act.Should().Throw<DomainException>().WithMessage("*Короткое описание события*");
        }

        [Fact]
        public void Create_Should_Throw_When_Description_Too_Long()
        {
            var desc = new string('b', 800);
            Action act = () =>
                Invitation.Create(
                    Guid.NewGuid(),
                    _eventId,
                    _roleId,
                    _shortDesc,
                    desc,
                    2,
                    _deadline
                );

            act.Should().Throw<DomainException>().WithMessage("*Описание события*");
        }

        [Fact]
        public void TakeRequest_Should_Add_Member_When_Valid()
        {
            var invitation = CreateDefaultInvitation();

            invitation.TakeRequest(Guid.NewGuid());

            invitation.Members.Should().HaveCount(1);
            invitation.AcceptedMember.Should().Be(0);
            invitation.Members[0].Status.Should().Be(MemberStatus.Review);
        }

        [Fact]
        public void TakeRequest_Should_Throw_When_Already_Exists()
        {
            var invitation = CreateDefaultInvitation();
            var studentId = Guid.NewGuid();
            invitation.TakeRequest(studentId);

            Action act = () => invitation.TakeRequest(studentId);

            act.Should().Throw<DomainException>().WithMessage("*уже подал заявку*");
        }

        [Fact]
        public void TakeRequest_Should_Throw_When_Deadline_Passed()
        {
            var invitation = Invitation.Create(
                Guid.NewGuid(),
                _eventId,
                _roleId,
                _shortDesc,
                _desc,
                2,
                DateTime.UtcNow.AddSeconds(-1)
            );

            Action act = () => invitation.TakeRequest(Guid.NewGuid());

            act.Should().Throw<DomainException>().WithMessage("*Время для подачи заявок окончено*");
        }

        [Fact]
        public void AcceptRequest_Should_Approve_Member()
        {
            var invitation = CreateDefaultInvitation();
            var studentId = Guid.NewGuid();
            invitation.TakeRequest(studentId);

            invitation.AcceptRequest(studentId);

            invitation.Members[0].Status.Should().Be(MemberStatus.Accepted);
            invitation.AcceptedMember.Should().Be(1);
            invitation.Status.Should().Be(InvitationStatus.Active);
        }

        [Fact]
        public void AcceptRequest_Should_Set_Status_Done_When_Limit_Reached()
        {
            var invitation = CreateDefaultInvitation(required: 1);
            var studentId = Guid.NewGuid();
            invitation.TakeRequest(studentId);

            invitation.AcceptRequest(studentId);

            invitation.Status.Should().Be(InvitationStatus.Done);
        }

        [Fact]
        public void AcceptRequest_Should_Throw_When_Already_Accepted()
        {
            var invitation = CreateDefaultInvitation();
            var studentId = Guid.NewGuid();
            invitation.TakeRequest(studentId);
            invitation.AcceptRequest(studentId);

            Action act = () => invitation.AcceptRequest(studentId);

            act.Should().Throw<DomainException>().WithMessage("*уже одобрена*");
        }

        [Fact]
        public void AcceptRequest_Should_Throw_When_NotFound()
        {
            var invitation = CreateDefaultInvitation();
            Action act = () => invitation.AcceptRequest(Guid.NewGuid());

            act.Should().Throw<DomainException>().WithMessage("*Заявка не найдена*");
        }

        [Fact]
        public void AcceptRequest_Should_Throw_When_Limit_Reached()
        {
            var invitation = CreateDefaultInvitation(required: 1);
            var student1 = Guid.NewGuid();
            var student2 = Guid.NewGuid();

            invitation.TakeRequest(student1);
            invitation.AcceptRequest(student1);

            Action act = () => invitation.TakeRequest(student2);

            act.Should().Throw<DomainException>();
        }

        private Invitation CreateDefaultInvitation(int required = 3)
        {
            return Invitation.Create(
                Guid.NewGuid(),
                _eventId,
                _roleId,
                _shortDesc,
                _desc,
                required,
                _deadline
            );
        }
    }
}
