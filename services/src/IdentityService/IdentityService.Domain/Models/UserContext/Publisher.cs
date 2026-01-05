namespace IdentityService.Domain.Models.UserContext;

/// <summary>
/// Модель профиля публикатора
/// </summary>
public class Publisher
{
    public Guid PublisherId { get; private set; }

    public int PostId { get; private set;  }

    public Post Post { get; private set; } = null!;

    public User User { get; private set; } = null!;

    private Publisher(Guid publisherId, int postId)
    {
        PublisherId = publisherId;
        PostId = postId;
    }

    /// <summary>
    /// Создание профиля публикатора
    /// </summary>
    /// <param name="publisherId">Id публикатора (пользователя)</param>
    /// <param name="postId">Id должности</param>
    /// <returns></returns>
    internal static Publisher Create(Guid publisherId, int postId)
    {
        return new Publisher(publisherId, postId);
    }
    
    /// <summary>
    /// Восстановление профиля из БД
    /// </summary>
    /// <param name="publisherId">Id публикатора</param>
    /// <param name="postId">Id должности</param>
    /// <returns>Модель публикатора</returns>
    internal static Publisher Restore(Guid publisherId, int postId)
    {
        return new Publisher(publisherId, postId);
    }

    /// <summary>
    /// Обновление публикатора
    /// </summary>
    /// <param name="postId">Id новой должности</param>
    internal void UpdatePublisher(int postId)
    {
        PostId = postId;
    }

    /// <summary>
    /// Добавление навигиацонного свойства должности
    /// </summary>
    /// <param name="post"></param>
    internal void AddPostNavigation(Post post)
    {
        Post = post;
    }
    
    /// <summary>
    /// Добавление навигиационного поля пользователя
    /// </summary>
    /// <param name="user"></param>
    internal void AddUserNavigation(User user)
    {
        User = user;
    }
}