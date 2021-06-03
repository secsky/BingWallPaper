using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingWallPaper
{
    public partial class Form1 : Form
    {
        static List<MemoryStream> ms = new List<MemoryStream>();
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private async Task GetImageAsync(RestRequest request, RestClient client)
        {
            var threadid = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
            Console.WriteLine($"线程{threadid}开始运行!");
            var restResponse = await client.ExecuteAsync(request);
            Json2CsharpClass.SampleResponse1 ro = SimpleJson.DeserializeObject<Json2CsharpClass.SampleResponse1>(restResponse.Content);
            var imgurl = "https://cn.bing.com" + ro.images[0].url;
            var imagename = ro.images[0].copyright.Split(' ')[0] + ".jpg";
            //Console.WriteLine(imagename);
            request = new RestRequest(imgurl);
            var img = await client.ExecuteAsync(request);
            var imgms = new MemoryStream(img.RawBytes);
            ms.Add(imgms);
            Console.WriteLine($"{imagename}写入完毕!");
            button2.PerformClick();
            //pictureBox1.Image = Image.FromStream(imgms);
            //label1.Text = $"已载入{ms.Count}张";
        }

        private void GetBingWallPaperAsync()
        {
            var client = new RestClient();
            client.UserAgent = Spider.SpiderHelper.lastestchromeuseragent;
            var url = "https://cn.bing.com/HPImageArchive.aspx?format=js&n=1&nc=1602139804090&pid=hp";
            for (int i = 0; i < 8; i++)
            {
                var request = new RestRequest(url);
                request.AddParameter("idx", i.ToString());
                _ = GetImageAsync(request, client);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            ms.Clear();
            GetBingWallPaperAsync();
        }
        static int index = -1;
        private void button2_Click(object sender, EventArgs e)
        {
            if (++index >= ms.Count)
            {
                index = 0;
            }
            pictureBox1.Image = Image.FromStream(ms[index]);
            label1.Text = $"当前是第{index + 1}张,共{ms.Count}张";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (--index <= -1)
            {
                index = ms.Count - 1;
            }
            pictureBox1.Image = Image.FromStream(ms[index]);
            label1.Text = $"当前是第{index + 1}张,共{ms.Count}张";
        }

        private void button4_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //打开的文件选择对话框上的标题
            saveFileDialog.Title = "请选择文件";
            //设置文件类型
            saveFileDialog.Filter = "文本文件(*.jpg)|*.jpg|所有文件(*.*)|*.*";
            //设置默认文件类型显示顺序
            saveFileDialog.FilterIndex = 1;
            //保存对话框是否记忆上次打开的目录
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = $"{index+1}.jpg";
            //设置是否允许多选
            //saveFileDialog.Multiselect = false;
            //按下确定选择的按钮
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //获得文件路径
                string localFilePath = saveFileDialog.FileName.ToString();
                //获取文件路径，不带文件名
                //FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));
                //获取文件名，带后缀名，不带路径
                //string fileNameWithSuffix = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);
                //去除文件后缀名
                //string fileNameWithoutSuffix = fileNameWithSuffix.Substring(0, fileNameWithSuffix.LastIndexOf("."));
                //在文件名前加上时间
                //string fileNameWithTime = DateTime.Now.ToString("yyyy-MM-dd ") + ".jpg";
                Stream stream = new FileStream(localFilePath, FileMode.Create);
                var bs = ms[index].ToArray();
                stream.Write(bs, 0, bs.Length);
                stream.Close();

            }
        }
    }
}
