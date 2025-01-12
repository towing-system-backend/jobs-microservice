using System.ComponentModel.DataAnnotations;

namespace Order.Infrastructure
{
    public interface IDto { }

    public record OrderToAcceptDto
    (
        [RegularExpression(@"^([0-9A-Fa-f]{8}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{12})$", ErrorMessage = "Id must be a 'Guid'.")]
        Guid Id,
        string TowDriverId,
        string DevideToken 
    ) : IDto;

}
