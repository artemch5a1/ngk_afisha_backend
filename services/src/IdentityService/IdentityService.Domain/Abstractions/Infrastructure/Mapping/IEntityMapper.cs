using IdentityService.Domain.Abstractions.Infrastructure.Entity;

namespace IdentityService.Domain.Abstractions.Infrastructure.Mapping;

/// <summary>
/// Маппер между доменом и инфраструктурой
/// </summary>
/// <typeparam name="TEntity">Тип сущности (Должен реализовывать <see cref="IEntity{TEntity,TDomain}"/>)</typeparam>
/// <typeparam name="TDomain">Тип доменной модели</typeparam>
public interface IEntityMapper<TEntity, TDomain>
    where TEntity : IEntity<TEntity, TDomain>
{
    /// <summary>
    /// Маппинг в доменную модель
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>Доменную модель</returns>
    /// <remarks>Использует по умолчанию метод <see cref="IEntity{TEntity, TDomain}.ToDomain"/>/></remarks>
    TDomain ToDomain(TEntity entity)
    {
        TDomain domain = entity.ToDomain();

        BeforeMapping(ref domain, entity);

        return domain;
    }

    /// <summary>
    /// Маппинг в коллекцию доменных моделей
    /// </summary>
    /// <param name="entities">Коллекция сущностей</param>
    /// <returns>Коллекция доменных моделей</returns>
    List<TDomain> ToListDomain(List<TEntity> entities)
    {
        return entities.Select(ToDomain).ToList();
    }

    /// <summary>
    /// Маппинг в сущность
    /// </summary>
    /// <param name="model">Доменная модель</param>
    /// <returns>Сущность</returns>
    /// <remarks>Использует по умолчанию метод <see cref="IEntity{TEntity, TDomain}.ToEntity"/>/></remarks>
    TEntity ToEntity(TDomain model)
    {
        return TEntity.ToEntity(model);
    }

    /// <summary>
    /// Метод, который реализует дополнительную логику с доменным объектом, прежде чем его вернет метод <see cref="ToDomain"/>
    /// </summary>
    /// <param name="domain">Доменная модель, результат маппинга</param>
    /// <param name="entity">Сущность, объект из которого выполняется маппинг</param>
    /// <remarks>Используется для допонительной логики построения навигационных свойств</remarks>
    void BeforeMapping(ref TDomain domain, TEntity entity);
}
