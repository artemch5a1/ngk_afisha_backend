using FluentAssertions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.UnitTests.Models;

public class PublisherTests
{
    [Fact]
        public void Create_ShouldReturnPublisherWithCorrectData()
        {
            // Arrange
            Guid publisherId = Guid.NewGuid();
            int postId = 5;

            // Act
            var publisher = Publisher.Create(publisherId, postId);

            // Assert
            publisher.Should().NotBeNull();
            publisher.PublisherId.Should().Be(publisherId);
            publisher.PostId.Should().Be(postId);
            publisher.Post.Should().BeNull();
            publisher.User.Should().BeNull();
        }

        [Fact]
        public void Restore_ShouldReturnPublisherWithCorrectData()
        {
            // Arrange
            Guid publisherId = Guid.NewGuid();
            int postId = 8;

            // Act
            var publisher = Publisher.Restore(publisherId, postId);

            // Assert
            publisher.PublisherId.Should().Be(publisherId);
            publisher.PostId.Should().Be(postId);
        }

        [Fact]
        public void UpdatePublisher_ShouldChangePostId()
        {
            // Arrange
            var publisher = Publisher.Create(Guid.NewGuid(), 3);

            // Act
            publisher.UpdatePublisher(10);

            // Assert
            publisher.PostId.Should().Be(10);
        }
}