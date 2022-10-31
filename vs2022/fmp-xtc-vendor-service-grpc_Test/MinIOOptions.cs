
using Microsoft.Extensions.Options;
using XTC.FMP.MOD.Vendor.App.Service;

public class MinIOOptions : IOptions<MinIOSettings>
{
    public MinIOSettings Value
    {
        get
        {
            return new MinIOSettings
            {
                AddressUrl = "localhost:9000",
                AddressSSL = false,
                Endpoint = "localhost:9000",
                Bucket = "fmp.vendor",
                AccessKey = "3KJLIOSFODNN0EXAMPO0",
                SecretKey = "vJalrXUtnFEMI/I5MDENG/bPxRfmCYEXAMPLEHEY"
            };
        }
    }
}
