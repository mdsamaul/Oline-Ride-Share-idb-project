using System.Net.NetworkInformation;

public class BaseEntity
{
    public string? CreateBy { get; set; }
    public DateTime CreateDate { get; set; }
    public string? UpdateBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool IsActive { get; set; }
    public BaseEntity()
    {
        IsActive = true; 
    }
    public void SetCreateInfo()
    {
        CreateBy = GetMacAddress();
        CreateDate = DateTime.Now;
    }
    public void SetUpdateInfo()
    {
        UpdateBy = GetMacAddress();
        UpdateDate = DateTime.Now;
    }
    private string GetMacAddress()
    {
        try
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    return networkInterface.GetPhysicalAddress().ToString();
                }
            }
            return "Unknown MAC";
        }
        catch
        {
            return "Error Retrieving MAC";
        }
    }
}
