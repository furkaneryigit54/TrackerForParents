using System.Diagnostics;

namespace TrackerForParents.Browsers;

public class Chrome
{
        
       
    public string ChromeUrl()
    {
        string url = "";
        string[] browsers = { "chrome"};
        string eskiurl = url;
        foreach (string browser in browsers)
        {
            Process[] processes = Process.GetProcessesByName(browser);
            foreach (Process process in processes)
            {
                url = process.MainWindowTitle;
                if (url!=""&url!=eskiurl)
                {
                    if (url==eskiurl)
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