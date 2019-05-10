using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Gtk;
using Newtonsoft.Json;
using Test;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnBtnConsultarClicked(object sender, EventArgs e)
    {

        String Url = "https://jsonplaceholder.typicode.com/posts";
        var webRequest = WebRequest.Create(Url) as HttpWebRequest;
        if (webRequest == null)
        {
            return;
        }

        webRequest.ContentType = "application/json";
        webRequest.UserAgent = "Nothing";

        using (var s = webRequest.GetResponse().GetResponseStream())
        {
            using (var sr = new StreamReader(s))
            {
                var postAsJson = sr.ReadToEnd();
                var posts = JsonConvert.DeserializeObject<List<Post>>(postAsJson);

                foreach (var post in posts)
                {
                    textview2.Buffer.Text += post.title + "\n";
                }

            }
        }
    }
}
