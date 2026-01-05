using IdentityService.Domain.CustomExceptions;

namespace IdentityService.Domain.Models.UserContext;

/// <summary>
/// Модель специальности
/// </summary>
public class Specialty
{
    private Specialty(string specialtyTitle)
    {
        SpecialtyTitle = specialtyTitle;
    }

    public int SpecialtyId { get; private set; }

    public string SpecialtyTitle { get; private set; }

    /// <summary>
    /// Создание специальности
    /// </summary>
    /// <param name="specialtyTitle">Название специальности</param>
    /// <returns>Модель специальности</returns>
    /// <exception cref="DomainException">Выбрасывается в случае провальной валидации</exception>
    public static Specialty CreateSpecialty(string specialtyTitle)
    {
        if (string.IsNullOrEmpty(specialtyTitle))
            throw new DomainException("Название специальности не может быть пустым");

        return new Specialty(specialtyTitle);
    }

    /// <summary>
    /// Восстановление специальности
    /// </summary>
    /// <param name="idSpecialty">Id специальности</param>
    /// <param name="specialtyTitle">Название специальности</param>
    /// <returns>Модель специальности</returns>
    internal static Specialty Restore(int idSpecialty, string specialtyTitle)
    {
        return new Specialty(specialtyTitle)
        {
            SpecialtyId = idSpecialty
        };
    }

    /// <summary>
    /// Обновление специальности
    /// </summary>
    /// <param name="newSpecialtyTitle">Новое название специальности</param>
    /// <exception cref="DomainException">Выбрасывается при провале ваилдации</exception>
    public void SpecialityUpdate(string newSpecialtyTitle)
    {
        if (string.IsNullOrEmpty(newSpecialtyTitle))
            throw new DomainException("Название специальности не может быть пустым");

        SpecialtyTitle = newSpecialtyTitle;
    }
}