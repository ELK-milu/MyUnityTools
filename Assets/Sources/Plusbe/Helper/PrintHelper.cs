using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Helper
{
    public class PrintHelper
    {
        private string path;

        private void ToPrint(string path)
        {
            try
            {
                this.path = path;
                System.Drawing.Printing.PrintDocument pri = new System.Drawing.Printing.PrintDocument();
                pri.PrintPage += Printpagetest;
                pri.Print();
            }
            catch { }
        }

        private void Printpagetest(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //144mm×105mm    3.825   550*401
            try
            {
                //string fileName = Path.GetFileName(allFiles[currLoc]);
                //string path = GlobalSetting.SignPath + fileName;
                //if (File.Exists(GlobalSetting.PrintPath + fileName))
                //{
                //    path = GlobalSetting.PrintPath + fileName;
                //}

                System.Drawing.Image image = System.Drawing.Image.FromFile(path);
                System.Drawing.Graphics g = e.Graphics;
                e.Graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, 600, 400), new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.GraphicsUnit.Pixel);
            }
            catch (Exception ee)
            {
                Debug.LogError(ee.Message);
            }
        }
    }
}
