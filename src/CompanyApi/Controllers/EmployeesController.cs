namespace CompanyApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyApi.Data;
using CompanyApi.DTO;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly CompanyDbContext _context;

    public EmployeesController(CompanyDbContext context)
    {
        _context = context;
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeSummaryDto>>> GetEmployees()
    {
        var employees = await _context.Employees
            .Include(e => e.Person)
            .Select(e => new EmployeeSummaryDto
            {
                Id = e.Id,
                FullName = (e.Person.FirstName + " " + (e.Person.MiddleName ?? "") + " " + e.Person.LastName)
                    .Replace("  ", " ").Trim()
            })
            .ToListAsync();
        return Ok(employees);
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDetailsDto>> GetEmployee(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Person)
            .Include(e => e.Position)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null || employee.Person == null)
        {
            return NotFound();
        }

        var employeeDetailsDto = new EmployeeDetailsDto
        {
            
            PassportNumber = employee.Person.PassportNumber,
            FirstName = employee.Person.FirstName,
            MiddleName = employee.Person.MiddleName,
            LastName = employee.Person.LastName,
            PhoneNumber = employee.Person.PhoneNumber,
            Email = employee.Person.Email,
            Salary = employee.Salary,
            Position = employee.Position == null
                ? null
                : new PositionDto
                {
                    Id = employee.Position.Id,
                    Name = employee.Position.Name
                },
            HireDate = employee.HireDate
        };

        return Ok(employeeDetailsDto);
    }
}