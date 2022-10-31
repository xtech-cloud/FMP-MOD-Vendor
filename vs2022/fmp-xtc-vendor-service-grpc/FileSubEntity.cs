namespace XTC.FMP.MOD.Vendor.App.Service
{
	public class FileSubEntity
	{
		public string path { get; set; } = "";
		public string hash { get; set; } = "";
		public ulong size { get; set; } = 0;
		public string url { get; set; } = "";
    }

	public class FileSubEntityS
	{
		public FileSubEntity[] entityS { get; set; } = new FileSubEntity[0];
	}
}
