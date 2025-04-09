namespace DeviceManager_Logic;

using DeviceManager_Entities;

public class DeviceManager
{
    private const int Max = 15;
    private static List<Device> _devices = new List<Device>();
    
    public DeviceManager()
    {
        _devices = new List<Device>
        {
            new Smartwatch
            {
                Id = "SW-1",
                Name = "Apple Watch SE6",
                IsTurnedOn = true,
                BatteryLevel = 67
            },
            new Smartwatch
            {
                Id = "SW-2",
                Name = "Xiaomi Band 5",
                IsTurnedOn = false,
                BatteryLevel = 53
            },
            new PersonalComputer
            {
                Id = "P-1",
                Name = "LinuxPC",
                IsTurnedOn = true,
                OperatingSystem = "Linux Mint"
            },
            new EmbeddedDevice
            {
                Id = "ED-1",
                Name = "Cupcake Pi",
                IsTurnedOn = false,
                IpAddress = "192.168.1.100",
                NetworkName = "MD Ltd.Wifi-1"
            }
        };
    }

    public List<Device> GetDevices() => _devices;

    public Device? GetDeviceById(string id) =>
        _devices.FirstOrDefault(d => d.Id == id);

    public void AddDevice(Device device)
    {
        if (_devices.Any(d => d.Id == device.Id))
            throw new ArgumentException($"Device with ID {device.Id} already exists.");
        if (_devices.Count >= Max)
            throw new InvalidOperationException("Device limit reached");
        
        _devices.Add(device);
    }

    public bool RemoveDevice(string id)
    {
        var device = GetDeviceById(id);
        return device != null && _devices.Remove(device);
    }

    public void EditDevice(Device editDevice)
    {
        var targetDeviceIndex = -1;
        for (var index = 0; index < _devices.Count; index++)
        {
            if (_devices[index].Id.Equals(editDevice.Id))
            {
                targetDeviceIndex = index;
                break;
            }
        }

        if (targetDeviceIndex == -1)
        {
            throw new ArgumentException($"Device with ID {editDevice.Id} is not stored.", nameof(editDevice));
        }

        var existingDevice = _devices[targetDeviceIndex];

        if (editDevice.GetType() != existingDevice.GetType())
        {
            throw new ArgumentException($"Type mismatch: existing device is {existingDevice.GetType().Name}, but received {editDevice.GetType().Name}.");
        }

        _devices[targetDeviceIndex] = editDevice;
    }
    
    public static bool IsValidDeviceId(Device device)
    {
        return device switch
        {
            Smartwatch => device.Id.StartsWith("SW-"),
            PersonalComputer => device.Id.StartsWith("P-"),
            EmbeddedDevice => device.Id.StartsWith("ED-"),
            _ => false
        };
    }
}