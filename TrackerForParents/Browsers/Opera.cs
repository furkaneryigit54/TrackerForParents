using System.Diagnostics;

namespace TrackerForParents.Browsers;

internal class Opera
{
    public string OperaUrl()
    {
        string url = "";
        string[] browsers = { "opera" };
        string eskiurl = url;
        foreach (string browser in browsers)
        {
            Process[] processes = Process.GetProcessesByName(browser);
            foreach (Process process in processes)
            {
                url = process.MainWindowTitle;
                if (url != "" & url != eskiurl)
                {
                    if (url == eskiurl)
                    {

                    }
                    else
                    {
                        eskiurl = url;
                        return url;
                    }
                }
            }
        }
        return String.Empty;

    }
}