using System.Text.Json;

namespace CompanyApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyApi.Data;
using CompanyApi.DTO;

[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly CompanyDbContext _context;
    private readonly ILogger<DevicesController> _logger;

    public DevicesController(CompanyDbContext context, ILogger<DevicesController> logger)
    {
        _context = context;
        _logger = logger;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceSummaryDto>>> GetDevices()
    {
        var devices = await _context.Devices
            .Select(d => new DeviceSummaryDto
            {
                Id = d.Id,
                Name = d.Name
            })
            .ToListAsync();
        return Ok(devices);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceDetailsDto>> GetDevice(int id)
    {
        var device = await _context.Devices
            .Include(d => d.DeviceType)
            .Include(d => d.DeviceEmployees)
            .ThenInclude(de => de.Employee)
            .ThenInclude(e => e.Person)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (device == null)
        {
            return NotFound();
        }

        object? additionalPropsObject = null;
        if (!string.IsNullOrWhiteSpace(device.AdditionalProperties))
        {
            try
            {
                additionalPropsObject = JsonSerializer.Deserialize<object>(device.AdditionalProperties);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex,
                    "Failed to deserialize AdditionalProperties for Device ID {DeviceId}. Raw value: {RawValue}",
                    device.Id, device.AdditionalProperties);
                additionalPropsObject = new
                    { error = "Invalid JSON format in database", rawValue = device.AdditionalProperties };
            }
        }

        var currentAssignment = device.DeviceEmployees.FirstOrDefault(de => de.ReturnDate == null);
        CurrentEmployeeDto? currentEmployeeDto = null;
        if (currentAssignment?.Employee?.Person != null)
        {
            currentEmployeeDto = new CurrentEmployeeDto
            {
                Id = currentAssignment.EmployeeId,
                Name = $"{currentAssignment.Employee.Person.FirstName} {currentAssignment.Employee.Person.LastName}"
                    .Trim()
            };
        }

        var deviceDetailsDto = new DeviceDetailsDto
        {
            Name = device.Name,
            DeviceTypeName = device.DeviceType?.Name,
            IsDeviceEnabled = device.IsEnabled,
            AdditionalProperties = additionalPropsObject,
            CurrentEmployee = currentEmployeeDto
        };

        return Ok(deviceDetailsDto);
    }


    [HttpPost]
    public async Task<ActionResult<DeviceDetailsDto>> CreateDevice(CreateDeviceRequestDto createDeviceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var deviceType = await _context.DeviceTypes
            .FirstOrDefaultAsync(dt => dt.Name == createDeviceDto.DeviceTypeName);

        if (deviceType == null)
        {
            return BadRequest($"DeviceType with name '{createDeviceDto.DeviceTypeName}' not found.");
        }

        var device = new Device
        {
            Name = createDeviceDto.Name,
            DeviceTypeId = deviceType.Id,
            IsEnabled = createDeviceDto.IsDeviceEnabled,
            AdditionalProperties = createDeviceDto.AdditionalPropertiesJson ?? string.Empty // Сохраняем JSON как строку
        };

        _context.Devices.Add(device);
        await _context.SaveChangesAsync();

        var createdDeviceDetails = new DeviceDetailsDto
        {
            Name = device.Name,
            DeviceTypeName = deviceType.Name,
            IsDeviceEnabled = device.IsEnabled,
            AdditionalProperties = !string.IsNullOrWhiteSpace(device.AdditionalProperties)
                ? JsonSerializer.Deserialize<object>(device.AdditionalProperties)
                : null,
            CurrentEmployee = null
        };

        return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, createdDeviceDetails);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDevice(int id, UpdateDeviceRequestDto updateDeviceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var device = await _context.Devices.FindAsync(id);
        if (device == null)
        {
            return NotFound($"Device with ID {id} not found.");
        }

        var deviceType = await _context.DeviceTypes
            .FirstOrDefaultAsync(dt => dt.Name == updateDeviceDto.DeviceTypeName);
        if (deviceType == null)
        {
            return BadRequest($"DeviceType with name '{updateDeviceDto.DeviceTypeName}' not found.");
        }

        device.Name = updateDeviceDto.Name;
        device.DeviceTypeId = deviceType.Id;
        device.IsEnabled = updateDeviceDto.IsDeviceEnabled;
        device.AdditionalProperties = updateDeviceDto.AdditionalPropertiesJson ?? device.AdditionalProperties;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Devices.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(int id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device == null)
        {
            return NotFound($"Device with ID {id} not found.");
        }


        var hasAssignments = await _context.DeviceEmployees.AnyAsync(de => de.DeviceId == id);
        if (hasAssignments)
        {
            var assignmentsToRemove = _context.DeviceEmployees.Where(de => de.DeviceId == id);
            _context.DeviceEmployees.RemoveRange(assignmentsToRemove);
        }


        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}