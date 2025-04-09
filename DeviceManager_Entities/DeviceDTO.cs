namespace DeviceManager_Entities;

public class DeviceDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsTurnedOn { get; set; }
    public string Type { get; set; } 

    // Optional
    public int? BatteryLevel { get; set; }
    public string OperatingSystem { get; set; }
    public string IpAddress { get; set; }
    public string NetworkName { get; set; }
}