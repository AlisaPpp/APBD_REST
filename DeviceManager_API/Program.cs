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
    return Results.Ok(manager.GetDevices());
});

//GetDeviceById
app.MapGet("/devices/{id}", (DeviceManager manager, string id) =>
{
    var device = manager.GetDeviceById(id);
    return device is null ? Results.NotFound() : Results.Ok(device);
});

//AddDevices
//AddSmartwatch
app.MapPost("/devices/sw", (DeviceManager manager, Smartwatch watch) =>
{
    if (!DeviceManager.IsValidDeviceId(watch))
        return Results.BadRequest("Invalid device ID format for Smartwatch.");

    try
    {
        manager.AddDevice(watch);
        return Results.Created($"/devices/{watch.Id}", watch);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

//AddPersonalComputer
app.MapPost("/devices/pc", (DeviceManager manager, PersonalComputer pc) =>
{
    if (!DeviceManager.IsValidDeviceId(pc))
        return Results.BadRequest("Invalid device ID format for PersonalComputer.");

    try
    {
        manager.AddDevice(pc);
        return Results.Created($"/devices/{pc.Id}", pc);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

//AddEmbeddedDevice
app.MapPost("/devices/ed", (DeviceManager manager, EmbeddedDevice ed) =>
{
    if (!DeviceManager.IsValidDeviceId(ed))
        return Results.BadRequest("Invalid device ID format for EmbeddedDevice.");

    try
    {
        manager.AddDevice(ed);
        return Results.Created($"/devices/{ed.Id}", ed);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});



//EditDevice
//EditSmartwatch
app.MapPut("/devices/sw/{id}", (DeviceManager manager, string id, Smartwatch updatedDevice) =>
{
    if (updatedDevice.Id != id)
        return Results.BadRequest("ID in the URL does not match the device ID.");

    try
    {
        manager.EditDevice(updatedDevice);
        return Results.Ok("Smartwatch updated successfully.");
    }
    catch (ArgumentException e)
    {
        return Results.BadRequest(e.Message);
    }
});

//EditPersonalComputer
app.MapPut("/devices/pc/{id}", (DeviceManager manager, string id, PersonalComputer updatedDevice) =>
{
    if (updatedDevice.Id != id)
        return Results.BadRequest("ID in the URL does not match the device ID.");

    try
    {
        manager.EditDevice(updatedDevice);
        return Results.Ok("PersonalComputer updated successfully.");
    }
    catch (ArgumentException e)
    {
        return Results.BadRequest(e.Message);
    }
});

//EditEmbeddedDevice
app.MapPut("/devices/ed/{id}", (DeviceManager manager, string id, EmbeddedDevice updatedDevice) =>
{
    if (updatedDevice.Id != id)
        return Results.BadRequest("ID in the URL does not match the device ID.");

    try
    {
        manager.EditDevice(updatedDevice);
        return Results.Ok("EmbeddedDevice updated successfully.");
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

