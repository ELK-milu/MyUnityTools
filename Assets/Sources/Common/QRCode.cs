
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using System.Text;
using System.IO;
using System;

namespace PlusbeQuickPlugin.QRCode
{
    public class QRCode 
    {
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="qrcodeString">地址</param>
        /// <param name="saveDir">保存路径</param>
        /// <param name="fileName">图片名称</param>
        /// <param name="size">尺寸</param>
        /// <returns>返回保存全路径</returns>
        public static string Create(string qrcodeString, string saveDir, string fileName, int size = 200)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = size < 100 ? 4 : size / 25;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeBackgroundColor = System.Drawing.Color.White;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;

            Bitmap bt = qrCodeEncoder.Encode(qrcodeString, Encoding.UTF8);
            string savePath = Path.Combine(saveDir, fileName + ".jpg");
            if (Directory.Exists(saveDir) == false)
                Directory.CreateDirectory(saveDir);
            bt.Save(savePath);
            return savePath;
        }
    }

    /// <summary>
    /// 二维码处理类
    /// </summary>
    public class QRCodeHandler
    {
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="QRString">二维码字符串</param>
        /// <param name="QRCodeEncodeMode">二维码编码(Byte、AlphaNumeric、Numeric)</param>
        /// <param name="QRCodeScale">二维码尺寸(Version为0时，1：26x26，每加1宽和高各加25</param>
        /// <param name="QRCodeVersion">二维码密集度0-40</param>
        /// <param name="QRCodeErrorCorrect">二维码纠错能力(L：7% M：15% Q：25% H：30%)</param>
        /// <param name="filePath">保存路径</param>
        /// <param name="hasLogo">是否有logo(logo尺寸50x50，QRCodeScale>=5，QRCodeErrorCorrect为H级)</param>
        /// <param name="logoFilePath">logo路径</param>
        /// <returns></returns>
        public bool CreateQRCode(string QRString, string QRCodeEncodeMode, short QRCodeScale, int QRCodeVersion, string QRCodeErrorCorrect, string filePath, bool hasLogo, string logoFilePath)
        {
            bool result = true;

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

            switch (QRCodeEncodeMode)
            {
                case "Byte":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
                case "AlphaNumeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                    break;
                case "Numeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                    break;
                default:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
            }

            qrCodeEncoder.QRCodeScale = QRCodeScale;
            qrCodeEncoder.QRCodeVersion = QRCodeVersion;

            switch (QRCodeErrorCorrect)
            {
                case "L":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case "M":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case "Q":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                case "H":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }

            try
            {
                Image image = qrCodeEncoder.Encode(QRString, System.Text.Encoding.UTF8);
                System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                fs.Close();

                if (hasLogo)
                {
                    Image copyImage = System.Drawing.Image.FromFile(logoFilePath);
                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
                    int x = image.Width / 2 - copyImage.Width / 2;
                    int y = image.Height / 2 - copyImage.Height / 2;
                    g.DrawImage(copyImage, new Rectangle(x, y, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                    g.Dispose();

                    image.Save(filePath);
                    copyImage.Dispose();
                }
                image.Dispose();

            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /* 调用
         QRCodeHandler qr = new QRCodeHandler();
            string path = Path.Combine("C:\\", "Down\\");
            string qrString = "http://blog.csdn.net/pan_junbiao";                         //二维码字符串
            string logoFilePath = path + "myLogo.jpg";                                    //Logo路径50*50
            string filePath = path + "myCode.jpg";                                        //二维码文件名
            qr.CreateQRCode(qrString, "Byte", 5, 0, "H", filePath, true, logoFilePath);   //生成
         */
    }
}


