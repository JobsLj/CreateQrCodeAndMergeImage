using System;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;

namespace CreateQrCodeAndMergeImage
{
    public class QrCodeHelper
    {

        /// <summary>  
        /// 生成二维码图片  
        /// </summary>  
        /// <param name="codeNumber">要生成二维码的字符串</param>       
        /// <param name="size">二维码每个颗粒大小尺寸</param>  
        /// <returns>二维码图片</returns>  
        public static Bitmap CreateImgCode(string codeNumber, int size)
        {
            //创建二维码生成类  
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置编码模式  
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //设置编码测量度  
            qrCodeEncoder.QRCodeScale = size;
            //设置编码版本  
            qrCodeEncoder.QRCodeVersion = 0;
            //设置编码错误纠正  
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //生成二维码图片  
            System.Drawing.Bitmap image = qrCodeEncoder.Encode(codeNumber);

            return image;
        }

        public static string CreateImage(string content,int size)
        {
            Bitmap image = CreateImgCode(content, size); //生成二维码图片

            //保存图片，需要图片的绝对地址，这是web项目
            var filename = DateTime.Now.ToString("yyyymmddhhmmssfff") + ".jpg";
            var filepath = AppDomain.CurrentDomain.BaseDirectory + @"\UploadPic\" + filename;
            image.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
            return filepath;
        }
    }
}