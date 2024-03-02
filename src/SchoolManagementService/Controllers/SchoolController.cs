using SchoolManagementService.Core.Application.School.Commands.CreateSchool;
using SchoolManagementService.Core.Domain.Enums.School;

namespace SchoolManagementService.Controllers;

public class SchoolController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var command = new CreateSchoolCommand(
            RegisterCode: 123456789,
            Name: "Test School",
            ShortName: "TS",
            GradingSystem: 1,
            Email: "test@example.com",
            Phone: "123-456-789",
            Type: SchoolType.Higher,
            PostalCode: 12345,
            OwnershipType: SchoolOwnershipType.State,
            StudentsQuantity: 1000,
            Region: SchoolRegion.Cherkasy,
            TerritorialCommunity: "Test Community",
            Address: "123 Test St.",
            AreOccupied: false
        );

        var schoolId = await Mediator.Send(command);
        return Ok(schoolId);
    }
}
