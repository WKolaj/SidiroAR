using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WebclientWithTimeout : WebClient
{
    private int _timeout = 10 * 60 * 1000;
    public int Timeout
    {
        get
        {
            return _timeout;
        }

        private set
        {
            _timeout = value;
        }
    }

    public WebclientWithTimeout(int timeout) : base()
    {
        this.Timeout = timeout;
    }

    //for sync requests
    protected override WebRequest GetWebRequest(Uri uri)
    {
        var w = base.GetWebRequest(uri);
        w.Timeout = Timeout; 
        return w;
    }

    public async Task<string> DownloadStringTaskWithTimeoutAsync(string address)
    {
        return await RunWithTimeout(base.DownloadStringTaskAsync(address));
    }

    public async Task<string> UploadStringTaskWithTimeoutAsync(string address, string data)
    {
        return await RunWithTimeout(base.UploadStringTaskAsync(address, data));
    }

    private async Task<T> RunWithTimeout<T>(Task<T> task)
    {
        if (task == await Task.WhenAny(task, Task.Delay(Timeout)))
            return await task;
        else
        {
            this.CancelAsync();
            throw new TimeoutException("Timeout elapsed");
        }
    }
}
