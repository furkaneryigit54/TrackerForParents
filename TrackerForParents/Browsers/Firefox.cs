using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerForParents.Browsers;

internal class Firefox
{
    public string FirefoxUrl()
    {
        string url = "";
        string[] browsers = { "firefox" };
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