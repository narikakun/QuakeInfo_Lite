using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuakeInfo_Lite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Check();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Check();
        }

        string last_eventid = "0";
        string last_datetime = "0";
        string webapiurl = "https://dev.narikakun.net/webapi/earthquake/post_data.json";

        private void Check()
        {
            try
            {
                WebClient wc = new WebClient() { Encoding = Encoding.GetEncoding("UTF-8") };
                string str = wc.DownloadString(webapiurl);
                try
                {
                    var json = JObject.Parse(str);
                    if (json["Control"]["DateTime"].Value<string>() == last_datetime) return;
                    string cityint1 = "", cityint2 = "", cityint3 = "", cityint4 = "", cityint5 = "", cityint5plus = "", cityint6 = "", cityint6plus = "", cityint7 = "";
                    if (last_eventid == json["Head"]["EventID"].Value<string>())
                    {
                        if (json["Head"]["Title"].Value<string>() != "震源に関する情報")
                        {
                            textBox1.Text = "";
                        }
                    }
                    else
                    {
                        textBox1.Text = "";
                    }
                    if (json["Head"]["Title"].Value<string>() != "震源に関する情報")
                    {
                        foreach (JObject pref in json["Body"]["Intensity"]["Observation"]["Pref"])
                        {
                            textBox1.Text += "[" + pref["Name"].Value<string>() + "]";
                            foreach (JObject area in pref["Area"])
                            {
                                if (json["Head"]["Title"].Value<string>() == "震源・震度情報")
                                {
                                    foreach (JObject city in area["City"])
                                    {
                                        if (city["MaxInt"].Value<string>() == "1") { cityint1 += city["Name"].Value<string>() + " "; }
                                        if (city["MaxInt"].Value<string>() == "2") { cityint2 += city["Name"].Value<string>() + " "; }
                                        if (city["MaxInt"].Value<string>() == "3") { cityint3 += city["Name"].Value<string>() + " "; }
                                        if (city["MaxInt"].Value<string>() == "4") { cityint4 += city["Name"].Value<string>() + " "; }
                                        if (city["MaxInt"].Value<string>() == "5-") { cityint5 += city["Name"].Value<string>() + " "; }
                                        if (city["MaxInt"].Value<string>() == "5+") { cityint5plus += city["Name"].Value<string>() + " "; }
                                        if (city["MaxInt"].Value<string>() == "6-") { cityint6 += city["Name"].Value<string>() + " "; }
                                        if (city["MaxInt"].Value<string>() == "6+") { cityint6plus += city["Name"].Value<string>() + " "; }
                                        if (city["MaxInt"].Value<string>() == "7") { cityint7 += city["Name"].Value<string>() + " "; }
                                    }
                                }
                                else if (json["Head"]["Title"].Value<string>() == "震度速報")
                                {
                                    if (area["MaxInt"].Value<string>() == "1") { cityint1 += area["Name"].Value<string>() + " "; }
                                    if (area["MaxInt"].Value<string>() == "2") { cityint2 += area["Name"].Value<string>() + " "; }
                                    if (area["MaxInt"].Value<string>() == "3") { cityint3 += area["Name"].Value<string>() + " "; }
                                    if (area["MaxInt"].Value<string>() == "4") { cityint4 += area["Name"].Value<string>() + " "; }
                                    if (area["MaxInt"].Value<string>() == "5-") { cityint5 += area["Name"].Value<string>() + " "; }
                                    if (area["MaxInt"].Value<string>() == "5+") { cityint5plus += area["Name"].Value<string>() + " "; }
                                    if (area["MaxInt"].Value<string>() == "6-") { cityint6 += area["Name"].Value<string>() + " "; }
                                    if (area["MaxInt"].Value<string>() == "6+") { cityint6plus += area["Name"].Value<string>() + " "; }
                                    if (area["MaxInt"].Value<string>() == "7") { cityint7 += area["Name"].Value<string>() + " "; }
                                }
                            }
                            if (cityint7 != "") { textBox1.Text += "震度7: " + cityint7; }
                            if (cityint6plus != "") { textBox1.Text += "震度6強: " + cityint6plus; }
                            if (cityint6 != "") { textBox1.Text += "震度6弱: " + cityint6; }
                            if (cityint5plus != "") { textBox1.Text += "震度5強: " + cityint5plus; }
                            if (cityint5 != "") { textBox1.Text += "震度5弱: " + cityint5; }
                            if (cityint4 != "") { textBox1.Text += "震度4: " + cityint4; }
                            if (cityint3 != "") { textBox1.Text += "震度3: " + cityint3; }
                            if (cityint2 != "") { textBox1.Text += "震度2: " + cityint2; }
                            if (cityint1 != "") { textBox1.Text += "震度1: " + cityint1; }
                            cityint1 = "";
                            cityint2 = "";
                            cityint3 = "";
                            cityint4 = "";
                            cityint5 = "";
                            cityint5plus = "";
                            cityint6 = "";
                            cityint6plus = "";
                            cityint7 = "";
                            textBox1.Text += "\r\n";
                        }
                    }
                    label1.Text = json["Head"]["Title"].Value<string>();
                    if (json["Head"]["Title"].Value<string>() == "震源・震度情報")
                    {
                        string d = json["Body"]["Earthquake"]["OriginTime"].Value<string>();
                        string f = "yyyy-MM-dd HH:mm:ss";
                        DateTime dt = DateTime.ParseExact(d, f, null);
                        label6.Text = "[時間] " + dt.ToString("dd日hh時mm分");
                        label7.Text = "[震度] " + json["Body"]["Intensity"]["Observation"]["MaxInt"].Value<string>().Replace("-", "弱").Replace("+", "強");
                    }
                    else if (json["Head"]["Title"].Value<string>() == "震度速報")
                    {
                        label7.Text = "[震度] " + json["Body"]["Intensity"]["Observation"]["MaxInt"].Value<string>().Replace("-", "弱").Replace("+", "強");
                    }
                    if (json["Head"]["Title"].Value<string>() == "震源・震度情報")
                    {
                        label2.Text = "[震源] " + json["Body"]["Earthquake"]["Hypocenter"]["Name"].Value<string>();
                    }
                    else
                    {
                        label2.Text = "[震源] 調査中";
                    }
                    if (json["Head"]["Title"].Value<string>() != "震度速報")
                    {
                        label3.Text = "[深さ] " + json["Body"]["Earthquake"]["Hypocenter"]["Depth"].Value<string>() + "km";
                        label4.Text = "[規模] " + json["Body"]["Earthquake"]["Magnitude"].Value<string>();
                    }
                    else
                    {
                        label3.Text = "[深さ] 調査中";
                        label4.Text = "[規模] 調査中";
                    }
                    last_eventid = json["Head"]["EventID"].Value<string>();
                    last_datetime = json["Control"]["DateTime"].Value<string>();
                    if (timer1.Interval != 3000)
                    {
                        timer1.Stop();
                        timer1.Interval = 3000;
                        timer1.Start();
                        Console.WriteLine("更新時間を3秒に戻します。");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("例外処理 更新時間を10秒に変更します。");
                    timer1.Stop();
                    timer1.Interval = 10000;
                    timer1.Start();
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("ネットワークエラー 更新時間を10秒に変更します。" + ex);
                timer1.Stop();
                timer1.Interval = 10000;
                timer1.Start();
            }
        }
    }
}
