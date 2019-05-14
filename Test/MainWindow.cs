using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using Gtk;
using Newtonsoft.Json;
using Test;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel) => Build();

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnBtnConsultarClicked(object sender, EventArgs e)
    {

        BackgroundWorker bw = new BackgroundWorker();
        bw.DoWork += new DoWorkEventHandler(this.bw_DoWork);
        bw.ProgressChanged += new ProgressChangedEventHandler(this.bw_ProgressChanged);
        bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bw_RunWorkerCompleted);

        bw.RunWorkerAsync();
    }

    void bw_DoWork(object sender, DoWorkEventArgs e)
    {
        this.getData();
    }

    void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        btnConsultar.Label = "Espere...";
    }

    void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        btnConsultar.Label = "Consultar";
    }

    void getData()
    {
        //btnConsultar.Label = "Espere...";

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
                    byte[] bytes = Encoding.Default.GetBytes(post.title);
                    string Title = Encoding.UTF8.GetString(bytes);
                    Console.WriteLine(Title);
                    //listPosts.AppendText(Title);


                }
            }
        }
    }
}
