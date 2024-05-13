namespace AqvaTaskRazorApp;
using System.Diagnostics;

public static class GlobalVariables
{
    public static string WEBSITE_URL { get; set; } = "https://www.sozcu.com.tr";

    public static void RunScraper()
    {
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            
            FileName = "cmd.exe",
            Arguments = "/C cd \"C:\\Users\\Emre Aygener\\Desktop\\aqva-task\\aqvascraper\" && python elastic.py"
        };
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
    }

    public static async Task RunScraperAsync(string path)
    {
        using (Process process = new Process())
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = "cmd.exe",
                Arguments = $"/C cd \"C:\\Users\\Emre Aygener\\Desktop\\aqva-task\\aqvascraper\" && python detailSave.py {path}"
            };
            process.StartInfo = startInfo;
            process.Start();

            await process.WaitForExitAsync();
        }
    }
}
