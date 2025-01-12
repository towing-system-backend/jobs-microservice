using System.ComponentModel.DataAnnotations;

namespace jobs_microservice.Src.Infrastructure.Controllers.Dtos
{
    public interface IDto { }

    public record UpdateOrderDto
    (
        [Required]
        [RegularExpression(@"^([0-9A-Fa-f]{8}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{12})$", ErrorMessage = "Id must be a 'Guid'.")]
        string Id,
        [Required][RegularExpression(@"^(ToAssign|ToAccept|Accepted|Located|InProgress|Completed|Cancelled|Paid)$", ErrorMessage = "Status is not valid.")]
        string Status
    ) : IDto;

}
