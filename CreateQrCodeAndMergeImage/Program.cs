using System;
using System.Drawing;
using System.IO;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace CreateQrCodeAndMergeImage
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("请输入要生成物料的数量！！！");
            var readCount = Console.ReadLine();
            var checkCount = CommonHelper.IsNumeric(readCount);
            if (!checkCount)
            {
                Console.WriteLine("请输入正确数量");
                return;
            }

            Console.WriteLine("请输入生成的二维码类型（1-微信会员卡 2-收款）！！！");
            var qrcodeType = Console.ReadLine();
            var checkType = CommonHelper.IsNumeric(qrcodeType);
            if (!checkType)
            {
                Console.WriteLine("请输入要生成的正确的二维码类型（1-微信会员卡 2-收款）");
                return;
            }

            var createNum = Convert.ToInt32(readCount);
            var type = Convert.ToInt32(qrcodeType);
            var showurlArr = GetShortUrls();
            for (int i = 0; i < createNum; i++)
            {
                var jumpAddress = showurlArr[i];

                //1.生成二维码图片

                //一、高精度二维码
                //var qrCodeFilePath = GetHighLevelQrcode(jumpAddress);

                //二、普通二维码
                var qrCodeFilePath = GetCommonLeverQrCode(jumpAddress);

                //2.拼接二维码图片，生成物料图片
                var backgroundImgName = "background.png";
                var waterMark = WaterMarkImage(qrCodeFilePath,type);
                if (type == (int)QrCodeType.Colllection)
                {
                    backgroundImgName = "background-money.jpg";
                }
                DIVWaterMark(waterMark, backgroundImgName, i);
                Console.WriteLine("第{0}合成图片成功!", i + 1);
            }

            Console.ReadLine();
        }

        public enum QrCodeType
        {
            //微信会员卡
            WechatCard = 1,

            //收款
            Colllection = 2
        }

        public static string GetHighLevelQrcode(string url)
        {
            const string qrEncodingType = "BYTE";
            const string createLevel = "H";
            const int version = 8;
            const int scale = 12;
            var qrCodeFilePath = CreateCode_Choose(url, qrEncodingType, createLevel, version, scale);
            return qrCodeFilePath;
        }

        public static string GetCommonLeverQrCode(string url)
        {
            var qrCodeFilePath = QrCodeHelper.CreateImage(url, 24);
            return qrCodeFilePath;
        }

        //生成二维码方法（简单）
        private static void CreateCode_Simple(string nr)
        {
            var qrCodeEncoder = new QRCodeEncoder
            {
                QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
                QRCodeScale = 4,
                QRCodeVersion = 8,
                QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M
            };
            //System.Drawing.Image image = qrCodeEncoder.Encode("15377541070 上海 Akon_Coder");
            Image image = qrCodeEncoder.Encode(nr);
            var filename = DateTime.Now.ToString("yyyymmddhhmmssfff") + ".jpg";
            var filepath = System.Web.HttpContext.Current.Server.MapPath(@"~\Upload") + "\\" + filename;
            var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);

            fs.Close();
            image.Dispose();
            //二维码解码
            var codeDecoder = CodeDecoder(filepath);
        }

        /// <summary>
        /// 生成二维码方法（复杂）
        /// </summary>
        /// <param name="strData">要生成的文字或者数字，支持中文。如： "15377541070 上海 Akon_Coder</param>
        /// <param name="qrEncoding">三种尺寸：BYTE ，ALPHA_NUMERIC，NUMERIC</param>
        /// <param name="level">大小：L M Q H</param>
        /// <param name="version">版本：如 8</param>
        /// <param name="scale">比例：如 4</param>
        /// <returns></returns>
        public static string CreateCode_Choose(string strData, string qrEncoding, string level, int version, int scale)
        {
            var qrCodeEncoder = new QRCodeEncoder();
            string encoding = qrEncoding;
            switch (encoding)
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

            qrCodeEncoder.QRCodeScale = scale;
            qrCodeEncoder.QRCodeVersion = version;
            switch (level)
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
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }
            //文字生成图片
            Image image = qrCodeEncoder.Encode(strData);
            var filename = DateTime.Now.ToString("yyyymmddhhmmssfff") + ".jpg";
            var filepath = AppDomain.CurrentDomain.BaseDirectory + @"\UploadPic\" + filename;
            var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Close();
            image.Dispose();
            return filepath;
        }

        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <returns></returns>
        public static string CodeDecoder(string filePath)
        {
            if (!File.Exists(filePath))
                return null;
            var myBitmap = new Bitmap(Image.FromFile(filePath));
            var decoder = new QRCodeDecoder();
            var decodedString = decoder.decode(new QRCodeBitmapImage(myBitmap));
            return decodedString;
        }

        /// <summary>
        /// 生成新的二维码
        /// </summary>
        /// <returns></returns>
        public static bool CreateImg()
        {
            Image img1 = Image.FromFile(@"D:\Pictures\hwl.jpg");
            Image img2 = Image.FromFile(@"D:\Pictures\sgh.jpg");
            Image img3 = Image.FromFile(@"D:\Pictures\syf.jpg");
            Image img = ImageMergeHelper.ImgMerge(200, 300, 5, null,
                new Image[] {img1, img2, img3, img2, img3, img2});
            img.Save(@"D:\Pictures\allinone.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return true;
        }

        #region 水印预设

        /// <summary>
        /// 水印文字预设
        /// </summary>
        /// <returns></returns>
        private static WaterMark WaterMarkFont()
        {
            var waterMark = new WaterMark
            {
                WaterMarkType = WaterMarkTypeEnum.Text,
                Transparency = 0.7f,
                Text = "dunitian.cnblogs.com",
                FontStyle = System.Drawing.FontStyle.Bold,
                FontFamily = "Consolas",
                FontSize = 20f,
                BrushesColor = System.Drawing.Brushes.Black,
                WaterMarkLocation = WaterMarkLocationEnum.CenterCenter
            };
            return waterMark;
        }

        /// <summary>
        /// 图片水印预设
        /// </summary>
        /// <returns></returns>
        private static WaterMark WaterMarkImage(string filePath,int type)
        {
            WaterMarkLocationEnum location;
            switch (type)
            {
                case (int)QrCodeType.WechatCard:
                    location = WaterMarkLocationEnum.CenterCenter;
                    break;
                case (int)QrCodeType.Colllection:
                    location = WaterMarkLocationEnum.AdJust;
                    break;
                default:
                    location = WaterMarkLocationEnum.CenterCenter;
                    break;
            }
            var waterMark = new WaterMark
            {
                WaterMarkType = WaterMarkTypeEnum.Image,
                ImgPath = filePath,
                WaterMarkLocation = location,
                Transparency = 1f
            };
            return waterMark;
        }

        #endregion

        #region 水印操作

        /// <summary>
        /// 单个水印操作
        /// </summary>
        /// <param name="waterMark"></param>
        /// <param name="backgroundImgName"></param>
        /// <param name="index"></param>
        private static void DIVWaterMark(WaterMark waterMark, string backgroundImgName, int index = 0)
        {
            #region 必须参数获取

            //图片路径
            var filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Pictures\" + backgroundImgName;
            //文件名
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            //图片所处目录
            string dirPath = Path.GetDirectoryName(filePath);
            //存放目录     
            string savePath = dirPath + "\\ProductWaterMark";
            //是否存在，不存在就创建
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            #endregion

            #region 水印操作

            Image successImage = WaterMarkHelper.SetWaterMark(filePath, waterMark);
            if (successImage != null)
            {
                //保存图片（不管打不打开都保存）
                successImage.Save(savePath + "\\" + index + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
            else
            {
                Console.WriteLine("水印失败！请检查原图和水印图！");
            }

            #endregion
        }

        #endregion

        public static string GetUpYunImgUrl()
        {
            var upyunExistImgUrl = string.Empty;
            var filePath = string.Format(AppDomain.CurrentDomain.BaseDirectory + @"\PrintTemplate\{0}", "img.html");

            const string upUserName = "webi200";
            const string upPassWord = "9m3uurowhna5";
            const string bucketName = "img-i200";
            var upyun = new UpYun(bucketName, upUserName, upPassWord);
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var r = new BinaryReader(fs);
            byte[] postArray = r.ReadBytes((int) fs.Length);

            var qrImgFilePath = "/WeChatTestLucky/" + DateTime.Now.ToString("yyyymmddhhmmssfff") + ".jpg";
            bool uploadResult = upyun.WriteFile(qrImgFilePath, postArray, true);
            fs.Dispose();
            if (uploadResult)
            {
                upyunExistImgUrl = "http://img.i200.cn/" + qrImgFilePath;
            }

            return upyunExistImgUrl;
        }



        public static string[] GetShortUrls()
        {

            return new[]
            {
"http://i2oo.cn/sBCu4X",
"http://i2oo.cn/sD9te1",
"http://i2oo.cn/sNd1AD",
"http://i2oo.cn/sGf1DZ",
"http://i2oo.cn/sHbiVu",
"http://i2oo.cn/sKnq8E",
"http://i2oo.cn/sLZqhb",
"http://i2oo.cn/s4sFLA",
"http://i2oo.cn/sPXodH",
"http://i2oo.cn/sRr8ud",
"http://i2oo.cn/sSV8By",
"http://i2oo.cn/sTym7K",
"http://i2oo.cn/sVTkkC",
"http://i2oo.cn/sWAksr",
"http://i2oo.cn/sYRjKM",
"http://i2oo.cn/sZu5b6",
"http://i2oo.cn/sbP6ts",
"http://i2oo.cn/sc16wP",
"http://i2oo.cn/seMgSj",
"http://i2oo.cn/sCqCjn",
"http://i2oo.cn/s6KCrR",
"http://i2oo.cn/s5oeHm",
"http://i2oo.cn/skHd2f",
"http://i2oo.cn/smmciT",
"http://i2oo.cn/soEc3o",
"http://i2oo.cn/sFjbR9",
"http://i2oo.cn/siD26V",
"http://i2oo.cn/s16Zyq",
"http://i2oo.cn/suBZGB",
"http://i2oo.cn/svCYYX",
"http://i2oo.cn/sx9XF1",
"http://i2oo.cn/sydXfD",
"http://i2oo.cn/hrfWPZ",
"http://i2oo.cn/hpbVCu",
"http://i2oo.cn/hhn7xE",
"http://i2oo.cn/hnZ7Nb",
"http://i2oo.cn/hfsTWA",
"http://i2oo.cn/h3XSoH",
"http://i2oo.cn/hwrSnd",
"http://i2oo.cn/hBVRMy",
"http://i2oo.cn/hUyQeK",
"http://i2oo.cn/hNTPvC",
"http://i2oo.cn/hEAPDr",
"http://i2oo.cn/hHR4VM",
"http://i2oo.cn/hJuMm6",
"http://i2oo.cn/hLPMhs",
"http://i2oo.cn/hM1LLP",
"http://i2oo.cn/hPMKcj",
"http://i2oo.cn/hQqJun",
"http://i2oo.cn/hSKJBR",
"http://i2oo.cn/hToHTm",
"http://i2oo.cn/hVHGkf",
"http://i2oo.cn/hWmGpT",
"http://i2oo.cn/hYEEJo",
"http://i2oo.cn/hZjNb9",
"http://i2oo.cn/hbDD1V",
"http://i2oo.cn/hc6D9q",
"http://i2oo.cn/heBUSB",
"http://i2oo.cn/hCCB5X",
"http://i2oo.cn/h69wz1",
"http://i2oo.cn/h5dwHD",
"http://i2oo.cn/hkf9ZZ",
"http://i2oo.cn/hmb3qu",
"http://i2oo.cn/hon33E",
"http://i2oo.cn/hFZfQb",
"http://i2oo.cn/hisagA",
"http://i2oo.cn/h1XnyH",
"http://i2oo.cn/hurnEd",
"http://i2oo.cn/hvVhXy",
"http://i2oo.cn/hAysFK",
"http://i2oo.cn/hyTsaC",
"http://i2oo.cn/hzApPr",
"http://i2oo.cn/npRrCM",
"http://i2oo.cn/nstzA6",
"http://i2oo.cn/nn4zNs",
"http://i2oo.cn/naiyWP",
"http://i2oo.cn/n3Lx8j",
"http://i2oo.cn/n9Fxnn",
"http://i2oo.cn/nBJAMR",
"http://i2oo.cn/nU8vdm",
"http://i2oo.cn/nNGuvf",
"http://i2oo.cn/nEkuUT",
"http://i2oo.cn/nHNt7o",
"http://i2oo.cn/nJ51m9",
"http://i2oo.cn/nLU1sV",
"http://i2oo.cn/nMgiKq",
"http://i2oo.cn/nPwqcB",
"http://i2oo.cn/nQeFtX",
"http://i2oo.cn/nS3Fw1",
"http://i2oo.cn/nTcoTD",
"http://i2oo.cn/nVa8jZ",
"http://i2oo.cn/nW28ru",
"http://i2oo.cn/nYhmJE",
"http://i2oo.cn/nZYk2b",
"http://i2oo.cn/nbpjiA",
"http://i2oo.cn/ncWj9H",
"http://i2oo.cn/ndz5Rd",
"http://i2oo.cn/nC766y",
"http://i2oo.cn/ngxgzK",
"http://i2oo.cn/n5SgGC",
"http://i2oo.cn/njvCZr",
"http://i2oo.cn/nmQeqM",
"http://i2oo.cn/n8tef6",
"http://i2oo.cn/nF4dQs",
"http://i2oo.cn/nqicgP",
"http://i2oo.cn/n1Lbxj",
"http://i2oo.cn/ntFbEn",
"http://i2oo.cn/nvJ2XR",
"http://i2oo.cn/nA8Zom",
"http://i2oo.cn/nyGZaf",
"http://i2oo.cn/nzkY4T",
"http://i2oo.cn/apNXeo",
"http://i2oo.cn/as5WA9",
"http://i2oo.cn/anUWDV",
"http://i2oo.cn/aagVVq",
"http://i2oo.cn/a3w78B",
"http://i2oo.cn/a9e7hX",
"http://i2oo.cn/aB3TL1",
"http://i2oo.cn/aUcSdD",
"http://i2oo.cn/aNaRuZ",
"http://i2oo.cn/aE2RBu",
"http://i2oo.cn/aHhQ7E",
"http://i2oo.cn/aJYPkb",
"http://i2oo.cn/aLpPpA",
"http://i2oo.cn/aMW4KH",
"http://i2oo.cn/a4zMbd",
"http://i2oo.cn/aQ7L1y",
"http://i2oo.cn/aRxLwK",
"http://i2oo.cn/aTSKSC",
"http://i2oo.cn/a7vJjr",
"http://i2oo.cn/aWQJrM",
"http://i2oo.cn/aXtHH6",
"http://i2oo.cn/aZ4G2s",
"http://i2oo.cn/a2iEiP",
"http://i2oo.cn/acLE3j",
"http://i2oo.cn/adFNRn",
"http://i2oo.cn/aCJD6R",
"http://i2oo.cn/ag8Uym",
"http://i2oo.cn/a5GUGf",
"http://i2oo.cn/ajkBYT",
"http://i2oo.cn/amNwFo",
"http://i2oo.cn/a85wf9",
"http://i2oo.cn/aFU9PV",
"http://i2oo.cn/aqg3Cq",
"http://i2oo.cn/a1wfxB",
"http://i2oo.cn/atefNX",
"http://i2oo.cn/av3aW1",
"http://i2oo.cn/aAcnoD",
"http://i2oo.cn/ayannZ",
"http://i2oo.cn/az2hMu",
"http://i2oo.cn/fphseE",
"http://i2oo.cn/fsYpvb",
"http://i2oo.cn/fnppUA",
"http://i2oo.cn/faWrVH",
"http://i2oo.cn/ffyzmd",
"http://i2oo.cn/f9Tzsy",
"http://i2oo.cn/fwAyLK",
"http://i2oo.cn/fURxcC",
"http://i2oo.cn/fDuAur",
"http://i2oo.cn/fEPABM",
"http://i2oo.cn/fG1vT6",
"http://i2oo.cn/fJMuks",
"http://i2oo.cn/fKqupP",
"http://i2oo.cn/fMKtJj",
"http://i2oo.cn/f4o1bn",
"http://i2oo.cn/fQHi1R",
"http://i2oo.cn/fRmi9m",
"http://i2oo.cn/fTEqSf",
"http://i2oo.cn/f7jF5T",
"http://i2oo.cn/fWDozo",
"http://i2oo.cn/fX6oH9",
"http://i2oo.cn/rCHu9",
"http://i2oo.cn/s9HBV",
"http://i2oo.cn/hdGTq",
"http://i2oo.cn/afEkB",
"http://i2oo.cn/fbEpX",
"http://i2oo.cn/9nNJ1",
"http://i2oo.cn/wZDbD",
"http://i2oo.cn/UsU1Z",
"http://i2oo.cn/DXU9u",
"http://i2oo.cn/ErBSE",
"http://i2oo.cn/GVw5b",
"http://i2oo.cn/Hy9zA",
"http://i2oo.cn/KT9HH",
"http://i2oo.cn/LA3Zd",
"http://i2oo.cn/4Rfqy",
"http://i2oo.cn/Puf3K",
"http://i2oo.cn/RPaQC",
"http://i2oo.cn/S1n6r",
"http://i2oo.cn/7MhyM",
"http://i2oo.cn/VqhE6",
"http://i2oo.cn/XKsYs",
"http://i2oo.cn/YopFP",
"http://i2oo.cn/2Hpaj",
"http://i2oo.cn/bmrPn",
"http://i2oo.cn/dNzCR",
"http://i2oo.cn/e5yAm",
"http://i2oo.cn/gUyNf",
"http://i2oo.cn/6gxWT",
"http://i2oo.cn/jwA8o",
"http://i2oo.cn/keAn9",
"http://i2oo.cn/83vMV",
"http://i2oo.cn/ocudq",
"http://i2oo.cn/qatvB",
"http://i2oo.cn/i2tUX",
"http://i2oo.cn/th171",
"http://i2oo.cn/uYimD",
"http://i2oo.cn/ApisZ",
"http://i2oo.cn/xWqKu",
"http://i2oo.cn/yzFcE",
"http://i2oo.cn/pr7otb",
"http://i2oo.cn/ppxowA",
"http://i2oo.cn/phS8TH",
"http://i2oo.cn/pnvmjd",
"http://i2oo.cn/pfQmry",
"http://i2oo.cn/p3tkJK",
"http://i2oo.cn/pw4j2C",
"http://i2oo.cn/pBi51r",
"http://i2oo.cn/pDL59M",
"http://i2oo.cn/pNF6R6",
"http://i2oo.cn/pGJg5s",
"http://i2oo.cn/pH8CzP",
"http://i2oo.cn/pKGCGj",
"http://i2oo.cn/pLkeZn",
"http://i2oo.cn/p4NdqR",
"http://i2oo.cn/pP5dfm",
"http://i2oo.cn/pRUcQf",
"http://i2oo.cn/pSgbgT",
"http://i2oo.cn/p7w2xo",
"http://i2oo.cn/pVe2E9",
"http://i2oo.cn/pX3ZXV",
"http://i2oo.cn/pYcYoq",
"http://i2oo.cn/p2aYaB",
"http://i2oo.cn/pb2X4X",
"http://i2oo.cn/pdhWe1",
"http://i2oo.cn/peYVAD",
"http://i2oo.cn/pgpVDZ",
"http://i2oo.cn/p6W7Vu",
"http://i2oo.cn/p5zT8E",
"http://i2oo.cn/pk7Thb",
"http://i2oo.cn/pmxSLA",
"http://i2oo.cn/poSRdH",
"http://i2oo.cn/pFvQud",
"http://i2oo.cn/piQQBy",
"http://i2oo.cn/p1tP7K",
"http://i2oo.cn/pu44kC",
"http://i2oo.cn/pvi4sr",
"http://i2oo.cn/pxLMKM",
"http://i2oo.cn/pyFLb6",
"http://i2oo.cn/srJKts",
"http://i2oo.cn/sp8KwP",
"http://i2oo.cn/shGJSj",
"http://i2oo.cn/snkHjn",
"http://i2oo.cn/sfNHrR",
"http://i2oo.cn/s35GHm",
"http://i2oo.cn/swUE2f",
"http://i2oo.cn/sBgNiT",
"http://i2oo.cn/sDwN3o",
"http://i2oo.cn/sNeDR9",
"http://i2oo.cn/sG3U6V",
"http://i2oo.cn/sHcByq",
"http://i2oo.cn/sKaBGB",
"http://i2oo.cn/sL2wYX",
"http://i2oo.cn/s4h9F1",
"http://i2oo.cn/sPY9fD",
"http://i2oo.cn/sRp3PZ",
"http://i2oo.cn/sSWfCu",
"http://i2oo.cn/sTzaxE",
"http://i2oo.cn/sV7aNb",
"http://i2oo.cn/sWxnWA",
"http://i2oo.cn/sYShoH",
"http://i2oo.cn/sZvhnd",
"http://i2oo.cn/sbQsMy",
"http://i2oo.cn/sctpeK",
"http://i2oo.cn/se4rvC",
"http://i2oo.cn/sCirDr",
"http://i2oo.cn/s6KzVM",
"http://i2oo.cn/s5oym6",
"http://i2oo.cn/skHyhs",
"http://i2oo.cn/smmxLP",
"http://i2oo.cn/soEAcj",
"http://i2oo.cn/sFjvun",
"http://i2oo.cn/siDvBR",
"http://i2oo.cn/s16uTm",
"http://i2oo.cn/suBtkf",
"http://i2oo.cn/svCtpT",
"http://i2oo.cn/sx91Jo",
"http://i2oo.cn/sydib9",
"http://i2oo.cn/hrfq1V",
"http://i2oo.cn/hpbq9q",
"http://i2oo.cn/hhnFSB",
"http://i2oo.cn/hnZo5X",
"http://i2oo.cn/hfs8z1",
"http://i2oo.cn/h3X8HD",
"http://i2oo.cn/hwrmZZ",
"http://i2oo.cn/hBVkqu",
"http://i2oo.cn/hUyk3E",
"http://i2oo.cn/hNTjQb",
"http://i2oo.cn/hEA5gA",
"http://i2oo.cn/hHR6yH",
"http://i2oo.cn/hJu6Ed",
"http://i2oo.cn/hLPgXy",
"http://i2oo.cn/hM1CFK",
"http://i2oo.cn/hPMCaC",
"http://i2oo.cn/hQqePr",
"http://i2oo.cn/hSKdCM",
"http://i2oo.cn/hTocA6",
"http://i2oo.cn/hVHcNs",
"http://i2oo.cn/hWmbWP",
"http://i2oo.cn/hYE28j",
"http://i2oo.cn/hZj2nn",
"http://i2oo.cn/hbDZMR",
"http://i2oo.cn/hc6Ydm",
"http://i2oo.cn/heBXvf",
"http://i2oo.cn/hCCXUT",
"http://i2oo.cn/h69W7o",
"http://i2oo.cn/h5dVm9",
"http://i2oo.cn/hkfVsV",
"http://i2oo.cn/hmb7Kq",
"http://i2oo.cn/honTcB",
"http://i2oo.cn/hFZStX",
"http://i2oo.cn/hisSw1",
"http://i2oo.cn/h1XRTD",
"http://i2oo.cn/hurQjZ",
"http://i2oo.cn/hvVQru",
"http://i2oo.cn/hAyPJE",
"http://i2oo.cn/hyT42b",
"http://i2oo.cn/hzAMiA",
"http://i2oo.cn/npRM9H",
"http://i2oo.cn/nsuLRd",
"http://i2oo.cn/nnPK6y",
"http://i2oo.cn/na1JzK",
"http://i2oo.cn/n3MJGC",
"http://i2oo.cn/n9qHZr",
"http://i2oo.cn/nBKGqM",
"http://i2oo.cn/nUoGf6",
"http://i2oo.cn/nNHEQs",
"http://i2oo.cn/nEmNgP",
"http://i2oo.cn/nHEDxj",
"http://i2oo.cn/nJjDEn",
"http://i2oo.cn/nLDUXR",
"http://i2oo.cn/nM6Bom",
"http://i2oo.cn/nPBBaf",
"http://i2oo.cn/nQCw4T",
"http://i2oo.cn/nS99eo",
"http://i2oo.cn/nTd3A9",
"http://i2oo.cn/nVf3DV",
"http://i2oo.cn/nWbfVq",
"http://i2oo.cn/nYna8B",
"http://i2oo.cn/nZZahX",
"http://i2oo.cn/nbsnL1",
"http://i2oo.cn/ncXhdD",
"http://i2oo.cn/nersuZ",
"http://i2oo.cn/nCVsBu",
"http://i2oo.cn/ngyp7E",
"http://i2oo.cn/n5Trkb",
"http://i2oo.cn/njArpA",
"http://i2oo.cn/nmQzKH",
"http://i2oo.cn/n8tybd",
"http://i2oo.cn/nF4x1y",
"http://i2oo.cn/nqixwK",
"http://i2oo.cn/n1LASC",
"http://i2oo.cn/ntFvjr",
"http://i2oo.cn/nvJvrM",
"http://i2oo.cn/nA8uH6",
"http://i2oo.cn/nyGt2s",
"http://i2oo.cn/nzk1iP",
"http://i2oo.cn/apN13j",
"http://i2oo.cn/as5iRn",
"http://i2oo.cn/anUq6R",
"http://i2oo.cn/aagFym",
"http://i2oo.cn/a3wFGf",
"http://i2oo.cn/a9eoYT",
"http://i2oo.cn/aB38Fo",
"http://i2oo.cn/aUc8f9",
"http://i2oo.cn/aNamPV",
"http://i2oo.cn/aE2kCq",
"http://i2oo.cn/aHhjxB",
"http://i2oo.cn/aJYjNX",
"http://i2oo.cn/aLp5W1",
"http://i2oo.cn/aMW6oD",
"http://i2oo.cn/a4z6nZ",
"http://i2oo.cn/aQ7gMu",
"http://i2oo.cn/aRxCeE",
"http://i2oo.cn/aTSevb",
"http://i2oo.cn/a7veUA",
"http://i2oo.cn/aWQdVH",
"http://i2oo.cn/aXtcmd",
"http://i2oo.cn/aZ4csy",
"http://i2oo.cn/a2ibLK",
"http://i2oo.cn/acL2cC",
"http://i2oo.cn/adFZur",
"http://i2oo.cn/aCJZBM",
"http://i2oo.cn/ag8YT6",
"http://i2oo.cn/a5GXks",
"http://i2oo.cn/ajkXpP",
"http://i2oo.cn/amNWJj",
"http://i2oo.cn/a85Vbn",
"http://i2oo.cn/aFU71R",
"http://i2oo.cn/aqg79m",
"http://i2oo.cn/a1wTSf",
"http://i2oo.cn/ateS5T",
"http://i2oo.cn/av3Rzo",
"http://i2oo.cn/aAcRH9",
"http://i2oo.cn/ayaQZV",
"http://i2oo.cn/az2Pqq",
"http://i2oo.cn/fphP3B",
"http://i2oo.cn/fsY4QX",
"http://i2oo.cn/fnpMg1",
"http://i2oo.cn/faWLyD",
"http://i2oo.cn/ffzLEZ",
"http://i2oo.cn/f97KXu",
"http://i2oo.cn/fwxJFE",
"http://i2oo.cn/fUSJab",
"http://i2oo.cn/fDvH4A",
"http://i2oo.cn/fEQGCH",
"http://i2oo.cn/fGtEAd",
"http://i2oo.cn/fJ4EDy",
"http://i2oo.cn/fKiNWK",
"http://i2oo.cn/fMLD8C",
"http://i2oo.cn/f4FDnr",
"http://i2oo.cn/fQJUMM",
"http://i2oo.cn/fR8Bd6",
"http://i2oo.cn/fTGwvs",
"http://i2oo.cn/f7kwUP",
"http://i2oo.cn/fWN97j",
"http://i2oo.cn/fX53mn",
"http://i2oo.cn/rCdPn",
"http://i2oo.cn/s9cCR",
"http://i2oo.cn/hdbAm",
"http://i2oo.cn/afbNf",
"http://i2oo.cn/fb2WT",
"http://i2oo.cn/9nZ8o",
"http://i2oo.cn/wZZn9",
"http://i2oo.cn/UsYMV",
"http://i2oo.cn/DXXdq",
"http://i2oo.cn/ErWvB",
"http://i2oo.cn/GVWUX",
"http://i2oo.cn/HyV71",
"http://i2oo.cn/KT7mD",
"http://i2oo.cn/LA7sZ",
"http://i2oo.cn/4RTKu",
"http://i2oo.cn/PuScE",
"http://i2oo.cn/RPRtb",
"http://i2oo.cn/S1RwA",
"http://i2oo.cn/7MQTH",
"http://i2oo.cn/VqPjd",
"http://i2oo.cn/XKPry",
"http://i2oo.cn/Yo4JK",
"http://i2oo.cn/2HM2C",
"http://i2oo.cn/bmL1r",
"http://i2oo.cn/dEL9M",
"http://i2oo.cn/ejKR6",
"http://i2oo.cn/gDJ5s",
"http://i2oo.cn/66HzP",
"http://i2oo.cn/jBHGj",
"http://i2oo.cn/kCGZn",
"http://i2oo.cn/89EqR",
"http://i2oo.cn/odEfm",
"http://i2oo.cn/qfNQf",
"http://i2oo.cn/ibDgT",
"http://i2oo.cn/tnUxo",
"http://i2oo.cn/uZUE9",
"http://i2oo.cn/AsBXV",
"http://i2oo.cn/xXwoq",
"http://i2oo.cn/zrwaB",
"http://i2oo.cn/prV94X",
"http://i2oo.cn/ppy3e1",
"http://i2oo.cn/phTfAD",
"http://i2oo.cn/pnAfDZ",
"http://i2oo.cn/pfRaVu",
"http://i2oo.cn/p3un8E",
"http://i2oo.cn/pwPnhb",
"http://i2oo.cn/pB1hLA",
"http://i2oo.cn/pDMsdH",
"http://i2oo.cn/pNqpud",
"http://i2oo.cn/pGKpBy",
"http://i2oo.cn/pHor7K",
"http://i2oo.cn/pKGzkC",
"http://i2oo.cn/pLkzsr",
"http://i2oo.cn/p4NyKM",
"http://i2oo.cn/pP5xb6",
"http://i2oo.cn/pRUAts",
"http://i2oo.cn/pSgAwP",
"http://i2oo.cn/p7wvSj",
"http://i2oo.cn/pVeujn",
"http://i2oo.cn/pX3urR",
"http://i2oo.cn/pYctHm",
"http://i2oo.cn/p2a12f",
"http://i2oo.cn/pb2iiT",
"http://i2oo.cn/pdhi3o",
"http://i2oo.cn/peYqR9",
"http://i2oo.cn/pgpF6V",
"http://i2oo.cn/p6Woyq",
"http://i2oo.cn/p5zoGB",
"http://i2oo.cn/pk78YX",
"http://i2oo.cn/pmxmF1",
"http://i2oo.cn/poSmfD",
"http://i2oo.cn/pFvkPZ",
"http://i2oo.cn/piQjCu"
            };
        }
    }
}