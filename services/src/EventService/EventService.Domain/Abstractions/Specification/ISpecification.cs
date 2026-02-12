namespace EventService.Domain.Abstractions.Specification;

public interface ISpecification<in TDomainModel>
{
    bool IsSatisfiedBy(TDomainModel item);

    string Name { get; }
}
