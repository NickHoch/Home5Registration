using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Home5Registration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string Image { get; set; }
        private void uploadImage_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog.OpenFile()) != null)
                {
                    using (myStream)
                    {
                        string sFileName = openFileDialog.FileName;
                        using (Bitmap bm = new Bitmap(sFileName))
                        {
                            using (var ms = new MemoryStream())
                            {
                                bm.Save(ms, ImageFormat.Jpeg);
                                Image = Convert.ToBase64String(ms.ToArray());
                                //fileName.Text = base64;
                                fileName.Text += openFileDialog.SafeFileName;
                            }
                        }
                    }
                }
            }

            //var fileContent = string.Empty;
            //var filePath = string.Empty;
            //using (OpenFileDialog openFileDialog = new OpenFileDialog())
            //{
            //    openFileDialog.InitialDirectory = "c:\\";
            //    openFileDialog.Filter = "Image Files(*.PNG;*.JPG;*.GIF)|*.PNG;*.JPG;*.GIF";
            //    openFileDialog.FilterIndex = 2;
            //    openFileDialog.RestoreDirectory = true;
            //    if (openFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        filePath = openFileDialog.FileName;
            //        var fileStream = openFileDialog.OpenFile();
            //        using (StreamReader reader = new StreamReader(fileStream))
            //        {
            //            fileContent = reader.ReadToEnd();
            //        }
            //        //fileName.Text += openFileDialog.SafeFileName;
            //        fileName.Text += Convert.ToBase64String(fileContent);
            //    }
            //}
        }

        private void registration_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://homewebapi.azurewebsites.net/");

            HttpResponseMessage response = client.PostAsJsonAsync("api/Account/Register", new User
            {
                Email = tbEmail.Text,
                Password = tbPassword.Text,
                ConfirmPassword = tbConfirmPassword.Text,
                Image = Image
            }).Result;
            response.EnsureSuccessStatusCode();
            MessageBox.Show("Bingo!");
        }

        public class User
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }

            public string Image { get; set; }

        }
    }
}
