using System.Reflection;

using DeviceManager_Logic;
using DeviceManager_Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DeviceManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//GetDevices
app.MapGet("/devices", (DeviceManager manager) =>
{
    var deviceList = manager.GetDevices().Select(d =>
    {
        var properties = d.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        var deviceInfo = properties.ToDictionary(
            prop => prop.Name,  
            prop => prop.GetValue(d) 
        );
        
        return deviceInfo;
    });

    return Results.Ok(deviceList);
});

//GetDeviceById
app.MapGet("/devices/{id}", (DeviceManager manager, string id) =>
{
    var device = manager.GetDeviceById(id);
    return device is null ? Results.NotFound() : Results.Ok(device);
});

//AddDevice
app.MapPost("/devices", (DeviceManager manager, DeviceDTO dto) =>
{
    Device device = dto.Type switch
    {
        "Smartwatch" => new Smartwatch
        {
            Id = dto.Id,
            Name = dto.Name,
            IsTurnedOn = dto.IsTurnedOn,
            BatteryLevel = dto.BatteryLevel ?? 10
        },
        "PersonalComputer" => new PersonalComputer
        {
            Id = dto.Id,
            Name = dto.Name,
            IsTurnedOn = dto.IsTurnedOn,
            OperatingSystem = dto.OperatingSystem
        },
        "EmbeddedDevice" => new EmbeddedDevice
        {
            Id = dto.Id,
            Name = dto.Name,
            IsTurnedOn = dto.IsTurnedOn,
            IpAddress = dto.IpAddress,
            NetworkName = dto.NetworkName
        },
        _ => null
    };

    if (device == null)
        return Results.BadRequest("Invalid device type.");
    
    if (!DeviceManager.IsValidDeviceId(device))
        return Results.BadRequest("Invalid device ID.");

    try
    {
        manager.AddDevice(device);
        return Results.Created($"/devices/{device.Id}", device);
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
});

//EditDevice
app.MapPut("/devices/{id}", (DeviceManager manager, string id, DeviceDTO dto) =>
{
    if (dto.Id != id)
        return Results.BadRequest("ID in the URL does not match any of the IDs");

    Device device = dto.Type switch
    {
        "Smartwatch" => new Smartwatch
        {
            Id = dto.Id,
            Name = dto.Name,
            IsTurnedOn = dto.IsTurnedOn,
            BatteryLevel = dto.BatteryLevel ?? 10
        },
        "PersonalComputer" => new PersonalComputer
        {
            Id = dto.Id,
            Name = dto.Name,
            IsTurnedOn = dto.IsTurnedOn,
            OperatingSystem = dto.OperatingSystem
        },
        "EmbeddedDevice" => new EmbeddedDevice
        {
            Id = dto.Id,
            Name = dto.Name,
            IsTurnedOn = dto.IsTurnedOn,
            IpAddress = dto.IpAddress,
            NetworkName = dto.NetworkName
        },
        _ => null
    };

    if (device == null)
        return Results.BadRequest("Invalid device type.");

    try
    {
        manager.EditDevice(device);
        return Results.Ok("Device updated successfully.");
    }
    catch (ArgumentException e)
    {
        return Results.BadRequest(e.Message);
    }
});

//RemoveDevice
app.MapDelete("/devices/{id}", (DeviceManager manager, string id) =>
{
    return manager.RemoveDevice(id) ? Results.NoContent() : Results.NotFound();
});


app.Run();

