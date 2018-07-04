using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
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
            var result = CommonHelper.IsNumeric(readCount);
            if (!result)
            {
                Console.WriteLine("请输入正确数量");
            }
            else
            {
                var createNum = Convert.ToInt32(readCount);
                const string qrEncodingType = "BYTE";
                const string createLevel = "H";
                const int version = 8;
                const int scale = 12;

                var showurlArr = GetShortUrl();
                for (int i = 0; i < createNum; i++)
                {
                    var jumpAddress = showurlArr[i];

                    //1.生成二维码图片
                    //var qrCodeFilePath = CreateCode_Choose(jumpAddress, qrEncodingType, createLevel, version, scale);
                    var qrCodeFilePath = QrCodeHelper.CreateImage(jumpAddress,24);

                    //2.拼接二维码图片，生成物料图片
                    var waterMark = WaterMarkImage(qrCodeFilePath);
                    DIVWaterMark(waterMark, i);
                    Console.WriteLine("第{0}合成图片成功!", i + 1);
                }
            }


            //var imgUrl = GetUpYunImgUrl();
            //var testTemp = imgUrl;

            //var orderSuccessEntity = new OrderSuccessEntity
            //{
            //    AccountId = 397,
            //    ProductName = "高级版一年",
            //    OrderId = "2343435",
            //    RealPayMoney = 23434,
            //    OrderTypeId = 4,
            //    Phone = "15377541070"
            //};
            //var sendHelper = new MessagePublisherHelper();
            //var result = sendHelper.SendMessage(orderSuccessEntity);

            //Console.WriteLine(result);

            //var calculateResult = 1 | 2;
            //Console.WriteLine(calculateResult);

            //var onFeeCount = 0;
            //var fiveFeeMaterialCount = 0;
            //var eightMaterialCoupon = 0;
            //var alllifeCount = 0;
            //var redPacketCount = 0;
            //var t1Count = 0;
            //var hunderFlowCount = 0;
            //var luckJoinCount = 0;

            //var requestUrl = "http://192.168.20.227:8092/v0/rewards/luckdraw";
            //var requestHeader = new Dictionary<string, string>
            //{
            //    {
            //        "token",
            //        "D196EE7AC3C620B6EABA08B9282FDFE5EDD0147FF2EBDDFA5E50EE2F39F5F2D60D96DD812ED240A7FE7E1140D7985340"
            //    }
            //};
            //var testCount = 5000;
            //for (int i = 0; i <= testCount; i++)
            //{
            //    var doResult = CommonHelper.RestPost(requestUrl, null, null, requestHeader);
            //    if (!string.IsNullOrWhiteSpace(doResult))
            //    {
            //        var luckDrawObj = JsonConvert.DeserializeObject<ResponseModel>(doResult);
            //        if (luckDrawObj.Code == 0)
            //        {
            //            var luckItemId = JsonConvert.DeserializeObject<LuckDrawResult>(luckDrawObj.Data.ToString())
            //                .LuckDrawItemID;
            //            if (luckItemId == 1)
            //            {
            //                onFeeCount = onFeeCount + 1;
            //            }
            //            else if (luckItemId == 2)
            //            {
            //                fiveFeeMaterialCount = fiveFeeMaterialCount + 1;
            //            }
            //            else if (luckItemId == 3)
            //            {
            //                eightMaterialCoupon = eightMaterialCoupon + 1;
            //            }
            //            else if (luckItemId == 4)
            //            {
            //                alllifeCount = alllifeCount + 1;
            //            }
            //            else if (luckItemId == 5)
            //            {
            //                redPacketCount = redPacketCount + 1;
            //            }
            //            else if (luckItemId == 6)
            //            {
            //                t1Count = t1Count + 1;
            //            }
            //            else if (luckItemId == 7)
            //            {
            //                hunderFlowCount = hunderFlowCount + 1;
            //            }
            //            else if (luckItemId == 8)
            //            {
            //                luckJoinCount = luckJoinCount + 1;
            //            }

            //            var luckItemName = JsonConvert.DeserializeObject<LuckDrawResult>(luckDrawObj.Data.ToString())
            //                .LuckDrawItemName;
            //            Console.WriteLine("恭喜您，中奖为：{0}", luckItemName);
            //        }
            //    }
            //}

            //Console.WriteLine("1元话费满减券中奖次数：{0}",onFeeCount);
            //Console.WriteLine("5元硬件满减券中奖次数：{0}", fiveFeeMaterialCount);
            //Console.WriteLine("V1商米88元满减券中奖次数：{0}", eightMaterialCoupon);
            //Console.WriteLine("终身高级版中奖次数：{0}", alllifeCount);
            //Console.WriteLine("1111元现金红包中奖次数：{0}", redPacketCount);
            //Console.WriteLine("商米T1一体机中奖次数：{0}", t1Count);
            //Console.WriteLine("100M全国流量中奖次数：{0}", hunderFlowCount);
            //Console.WriteLine("感谢参与中奖次数：{0}", luckJoinCount);


            Console.ReadLine();
        }

        public class LuckDrawResult
        {
            /// <summary>
            /// 奖品id
            /// </summary>
            public int LuckDrawItemID { get; set; }

            /// <summary>
            /// 奖品名称
            /// </summary>
            public string LuckDrawItemName { get; set; }
        }

        public class ResponseModel
        {
            /// <summary>
            /// 错误代码
            /// </summary>
            public int Code { get; set; }

            /// <summary>
            /// 错误信息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 返回的数据实体
            /// </summary>
            public object Data { get; set; }
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
        private static WaterMark WaterMarkImage(string filePath)
        {
            var waterMark = new WaterMark
            {
                WaterMarkType = WaterMarkTypeEnum.Image,
                ImgPath = filePath,
                WaterMarkLocation = WaterMarkLocationEnum.CenterCenter,
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
        /// <param name="index"></param>
        private static void DIVWaterMark(WaterMark waterMark, int index = 0)
        {
            #region 必须参数获取

            //图片路径
            var filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Pictures\background.png";
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



        public static string[] GetShortUrl()
        {

            return new[]
            {
"http://i2oo.cn/sxhVbu",
"http://i2oo.cn/syY7tE",
"http://i2oo.cn/hrp7wb",
"http://i2oo.cn/hpWTSA",
"http://i2oo.cn/hszSjH",
"http://i2oo.cn/hn7Srd",
"http://i2oo.cn/haxRHy",
"http://i2oo.cn/h3SQ2K",
"http://i2oo.cn/h9vPiC",
"http://i2oo.cn/hBQP9r",
"http://i2oo.cn/hUt4RM",
"http://i2oo.cn/hN4M66",
"http://i2oo.cn/hEiLzs",
"http://i2oo.cn/hHLLGP",
"http://i2oo.cn/hJFKYj",
"http://i2oo.cn/hLJJqn",
"http://i2oo.cn/hM8JfR",
"http://i2oo.cn/hPGHPm",
"http://i2oo.cn/hQkGgf",
"http://i2oo.cn/hSNExT",
"http://i2oo.cn/hT5ENo",
"http://i2oo.cn/hVUNX9",
"http://i2oo.cn/hWgDoV",
"http://i2oo.cn/hYwDnq",
"http://i2oo.cn/hZeU4B",
"http://i2oo.cn/hb3BeX",
"http://i2oo.cn/hccwv1",
"http://i2oo.cn/heawDD",
"http://i2oo.cn/hC29VZ",
"http://i2oo.cn/h6h3mu",
"http://i2oo.cn/h5Y3hE",
"http://i2oo.cn/hkpfLb",
"http://i2oo.cn/hmWacA",
"http://i2oo.cn/h8znuH",
"http://i2oo.cn/hF7nBd",
"http://i2oo.cn/hqxhTy",
"http://i2oo.cn/h1SskK",
"http://i2oo.cn/htvspC",
"http://i2oo.cn/hvQpKr",
"http://i2oo.cn/hAtrbM",
"http://i2oo.cn/hyMz16",
"http://i2oo.cn/hzqzws",
"http://i2oo.cn/npKySP",
"http://i2oo.cn/nsox5j",
"http://i2oo.cn/nnHxrn",
"http://i2oo.cn/namAHR",
"http://i2oo.cn/n3EvZm",
"http://i2oo.cn/n9juif",
"http://i2oo.cn/nBDu3T",
"http://i2oo.cn/nU6tQo",
"http://i2oo.cn/nNB169",
"http://i2oo.cn/nECiyV",
"http://i2oo.cn/nH9iEq",
"http://i2oo.cn/nJdqYB",
"http://i2oo.cn/nLfFFX",
"http://i2oo.cn/nMbFa1",
"http://i2oo.cn/nPnoPD",
"http://i2oo.cn/nQZ8CZ",
"http://i2oo.cn/nSsmAu",
"http://i2oo.cn/nTXmNE",
"http://i2oo.cn/nVrkWb",
"http://i2oo.cn/nWVj8A",
"http://i2oo.cn/nXyjnH",
"http://i2oo.cn/nZT5Md",
"http://i2oo.cn/n2A6dy",
"http://i2oo.cn/ncRgvK",
"http://i2oo.cn/ndugUC",
"http://i2oo.cn/nCPCVr",
"http://i2oo.cn/ng1emM",
"http://i2oo.cn/n5Mes6",
"http://i2oo.cn/njqdLs",
"http://i2oo.cn/nmKccP",
"http://i2oo.cn/n8obtj",
"http://i2oo.cn/nFHbBn",
"http://i2oo.cn/nqm2TR",
"http://i2oo.cn/n1EZjm",
"http://i2oo.cn/ntjZpf",
"http://i2oo.cn/nvDYJT",
"http://i2oo.cn/nA6X2o",
"http://i2oo.cn/nyBW19",
"http://i2oo.cn/nzCW9V",
"http://i2oo.cn/ap9VRq",
"http://i2oo.cn/asd75B",
"http://i2oo.cn/anfTzX",
"http://i2oo.cn/aabTG1",
"http://i2oo.cn/a3nSZD",
"http://i2oo.cn/a9ZRqZ",
"http://i2oo.cn/aBsRfu",
"http://i2oo.cn/aUXQQE",
"http://i2oo.cn/aNrPgb",
"http://i2oo.cn/aEV4xA",
"http://i2oo.cn/aGy4EH",
"http://i2oo.cn/aJTMXd",
"http://i2oo.cn/aKALoy",
"http://i2oo.cn/aMRLaK",
"http://i2oo.cn/a4uK4C",
"http://i2oo.cn/aQPJCr",
"http://i2oo.cn/aR1HAM",
"http://i2oo.cn/aTMHD6",
"http://i2oo.cn/a7qGWs",
"http://i2oo.cn/aWKE8P",
"http://i2oo.cn/aXoEhj",
"http://i2oo.cn/aZHNMn",
"http://i2oo.cn/a2mDdR",
"http://i2oo.cn/acEUum",
"http://i2oo.cn/adjUUf",
"http://i2oo.cn/aCDB7T",
"http://i2oo.cn/ag6wko",
"http://i2oo.cn/a5Bws9",
"http://i2oo.cn/ajC9KV",
"http://i2oo.cn/am93bq",
"http://i2oo.cn/a8dftB",
"http://i2oo.cn/aFffwX",
"http://i2oo.cn/aqbaS1",
"http://i2oo.cn/a1nnjD",
"http://i2oo.cn/atZnrZ",
"http://i2oo.cn/avshHu",
"http://i2oo.cn/aAXs2E",
"http://i2oo.cn/ayrpib",
"http://i2oo.cn/azVp3A",
"http://i2oo.cn/fryrRH",
"http://i2oo.cn/fsSz6d",
"http://i2oo.cn/fhvyyy",
"http://i2oo.cn/faQyGK",
"http://i2oo.cn/fftxYC",
"http://i2oo.cn/f94Aqr",
"http://i2oo.cn/fwiAfM",
"http://i2oo.cn/fULvP6",
"http://i2oo.cn/fDFugs",
"http://i2oo.cn/fEJtxP",
"http://i2oo.cn/fG8tNj",
"http://i2oo.cn/fJG1Xn",
"http://i2oo.cn/fKkioR",
"http://i2oo.cn/fMNinm",
"http://i2oo.cn/f45q4f",
"http://i2oo.cn/fQUFeT",
"http://i2oo.cn/fRgovo",
"http://i2oo.cn/fTwoD9",
"http://i2oo.cn/f7e8VV",
"http://i2oo.cn/fW3mmq",
"http://i2oo.cn/fXcmhB",
"http://i2oo.cn/r2EgB",
"http://i2oo.cn/shNxX",
"http://i2oo.cn/hYNN1",
"http://i2oo.cn/apDXD",
"http://i2oo.cn/fWUoZ",
"http://i2oo.cn/3zUnu",
"http://i2oo.cn/w7B4E",
"http://i2oo.cn/Bxweb",
"http://i2oo.cn/DS9vA",
"http://i2oo.cn/Nv9DH",
"http://i2oo.cn/GQ3Vd",
"http://i2oo.cn/Htfmy",
"http://i2oo.cn/K4fhK",
"http://i2oo.cn/LiaLC",
"http://i2oo.cn/4Lndr",
"http://i2oo.cn/PFhuM",
"http://i2oo.cn/RJhB6",
"http://i2oo.cn/S8s7s",
"http://i2oo.cn/7GpkP",
"http://i2oo.cn/Vkppj",
"http://i2oo.cn/XNrKn",
"http://i2oo.cn/Y6zbR",
"http://i2oo.cn/2By1m",
"http://i2oo.cn/bCywf",
"http://i2oo.cn/d9xST",
"http://i2oo.cn/edA5o",
"http://i2oo.cn/gfAr9",
"http://i2oo.cn/6bvHV",
"http://i2oo.cn/jnuZq",
"http://i2oo.cn/kZtiB",
"http://i2oo.cn/8st3X",
"http://i2oo.cn/oX1Q1",
"http://i2oo.cn/qri6D",
"http://i2oo.cn/iVqyZ",
"http://i2oo.cn/1yqEu",
"http://i2oo.cn/uTFYE",
"http://i2oo.cn/vAoFb",
"http://i2oo.cn/xRoaA",
"http://i2oo.cn/yu8PH",
"http://i2oo.cn/prPmCd",
"http://i2oo.cn/pp1kAy",
"http://i2oo.cn/phMkNK",
"http://i2oo.cn/pnqjWC",
"http://i2oo.cn/pfK5or",
"http://i2oo.cn/p3o5nM",
"http://i2oo.cn/pwH6M6",
"http://i2oo.cn/pBmges",
"http://i2oo.cn/pDECvP",
"http://i2oo.cn/pNjCUj",
"http://i2oo.cn/pGDeVn",
"http://i2oo.cn/pH6dmR",
"http://i2oo.cn/pKBdsm",
"http://i2oo.cn/pLCcLf",
"http://i2oo.cn/p49bcT",
"http://i2oo.cn/pPd2to",
"http://i2oo.cn/pRf2B9",
"http://i2oo.cn/pSbZTV",
"http://i2oo.cn/p7nYjq",
"http://i2oo.cn/pVZYpB",
"http://i2oo.cn/pXsXJX",
"http://i2oo.cn/pYXW21",
"http://i2oo.cn/p2rV1D",
"http://i2oo.cn/pbVV9Z",
"http://i2oo.cn/pcy7Ru",
"http://i2oo.cn/peTT5E",
"http://i2oo.cn/pCASzb",
"http://i2oo.cn/p6RSGA",
"http://i2oo.cn/p5uRZH",
"http://i2oo.cn/pkPQqd",
"http://i2oo.cn/pm1Qfy",
"http://i2oo.cn/poMPQK",
"http://i2oo.cn/pFq4gC",
"http://i2oo.cn/piKMyr",
"http://i2oo.cn/p1oMEM",
"http://i2oo.cn/puHLX6",
"http://i2oo.cn/pvmKFs",
"http://i2oo.cn/pxEKaP",
"http://i2oo.cn/pyjJ4j",
"http://i2oo.cn/srDHCn",
"http://i2oo.cn/sp6GAR",
"http://i2oo.cn/shBGDm",
"http://i2oo.cn/snCEWf",
"http://i2oo.cn/sf9N8T",
"http://i2oo.cn/s3dNho",
"http://i2oo.cn/swfDM9",
"http://i2oo.cn/sBbUdV",
"http://i2oo.cn/sDnBuq",
"http://i2oo.cn/sNZBUB",
"http://i2oo.cn/sGsw7X",
"http://i2oo.cn/sHX9k1",
"http://i2oo.cn/sKr9sD",
"http://i2oo.cn/sLV3KZ",
"http://i2oo.cn/sMyfbu",
"http://i2oo.cn/sPTatE",
"http://i2oo.cn/sQAawb",
"http://i2oo.cn/sSRnSA",
"http://i2oo.cn/sTuhjH",
"http://i2oo.cn/sVPhrd",
"http://i2oo.cn/sW1sHy",
"http://i2oo.cn/sYMp2K",
"http://i2oo.cn/sZqriC",
"http://i2oo.cn/sbKr9r",
"http://i2oo.cn/sc8zRM",
"http://i2oo.cn/seGy66",
"http://i2oo.cn/sCkxzs",
"http://i2oo.cn/s6NxGP",
"http://i2oo.cn/s55AYj",
"http://i2oo.cn/skUvqn",
"http://i2oo.cn/smgvfR",
"http://i2oo.cn/sowuPm",
"http://i2oo.cn/sFetgf",
"http://i2oo.cn/si31xT",
"http://i2oo.cn/s1c1No",
"http://i2oo.cn/suaiX9",
"http://i2oo.cn/sv2qoV",
"http://i2oo.cn/sxhqnq",
"http://i2oo.cn/syYF4B",
"http://i2oo.cn/hrpoeX",
"http://i2oo.cn/hpW8v1",
"http://i2oo.cn/hsz8DD",
"http://i2oo.cn/hn7mVZ",
"http://i2oo.cn/haxkmu",
"http://i2oo.cn/h3SkhE",
"http://i2oo.cn/h9vjLb",
"http://i2oo.cn/hBQ5cA",
"http://i2oo.cn/hUt6uH",
"http://i2oo.cn/hN46Bd",
"http://i2oo.cn/hEigTy",
"http://i2oo.cn/hHLCkK",
"http://i2oo.cn/hJFCpC",
"http://i2oo.cn/hLJeKr",
"http://i2oo.cn/hM8dbM",
"http://i2oo.cn/hPGc16",
"http://i2oo.cn/hQkcws",
"http://i2oo.cn/hSNbSP",
"http://i2oo.cn/hT525j",
"http://i2oo.cn/hVU2rn",
"http://i2oo.cn/hWgZHR",
"http://i2oo.cn/hYwYZm",
"http://i2oo.cn/hZeXif",
"http://i2oo.cn/hb3X3T",
"http://i2oo.cn/hccWQo",
"http://i2oo.cn/heaV69",
"http://i2oo.cn/hC27yV",
"http://i2oo.cn/h6h7Eq",
"http://i2oo.cn/h5YTYB",
"http://i2oo.cn/hkpSFX",
"http://i2oo.cn/hmWSa1",
"http://i2oo.cn/h8zRPD",
"http://i2oo.cn/hF7QCZ",
"http://i2oo.cn/hqxPAu",
"http://i2oo.cn/h1SPNE",
"http://i2oo.cn/htv4Wb",
"http://i2oo.cn/hvQM8A",
"http://i2oo.cn/hAtMnH",
"http://i2oo.cn/hy4LMd",
"http://i2oo.cn/hziKdy",
"http://i2oo.cn/npLJvK",
"http://i2oo.cn/nsFJUC",
"http://i2oo.cn/nnJHVr",
"http://i2oo.cn/na8GmM",
"http://i2oo.cn/n3GGs6",
"http://i2oo.cn/n9kELs",
"http://i2oo.cn/nBNNcP",
"http://i2oo.cn/nU5Dtj",
"http://i2oo.cn/nNUDBn",
"http://i2oo.cn/nEgUTR",
"http://i2oo.cn/nHwBjm",
"http://i2oo.cn/nJeBpf",
"http://i2oo.cn/nL3wJT",
"http://i2oo.cn/nMc92o",
"http://i2oo.cn/nPa319",
"http://i2oo.cn/nQ239V",
"http://i2oo.cn/nShfRq",
"http://i2oo.cn/nTYa5B",
"http://i2oo.cn/nVpnzX",
"http://i2oo.cn/nWWnG1",
"http://i2oo.cn/nXzhZD",
"http://i2oo.cn/nZ7sqZ",
"http://i2oo.cn/n2xsfu",
"http://i2oo.cn/ncSpQE",
"http://i2oo.cn/ndvrgb",
"http://i2oo.cn/nCPzxA",
"http://i2oo.cn/ng1zEH",
"http://i2oo.cn/n5MyXd",
"http://i2oo.cn/njqxoy",
"http://i2oo.cn/nmKxaK",
"http://i2oo.cn/n8oA4C",
"http://i2oo.cn/nFHvCr",
"http://i2oo.cn/nqmuAM",
"http://i2oo.cn/n1EuD6",
"http://i2oo.cn/ntjtWs",
"http://i2oo.cn/nvD18P",
"http://i2oo.cn/nA61hj",
"http://i2oo.cn/nyBiMn",
"http://i2oo.cn/nzCqdR",
"http://i2oo.cn/ap9Fum",
"http://i2oo.cn/asdFUf",
"http://i2oo.cn/anfo7T",
"http://i2oo.cn/aab8ko",
"http://i2oo.cn/a3n8s9",
"http://i2oo.cn/a9ZmKV",
"http://i2oo.cn/aBskbq",
"http://i2oo.cn/aUXjtB",
"http://i2oo.cn/aNrjwX",
"http://i2oo.cn/aEV5S1",
"http://i2oo.cn/aGy6jD",
"http://i2oo.cn/aJT6rZ",
"http://i2oo.cn/aKAgHu",
"http://i2oo.cn/aMRC2E",
"http://i2oo.cn/a4ueib",
"http://i2oo.cn/aQPe3A",
"http://i2oo.cn/aR1dRH",
"http://i2oo.cn/aTMc6d",
"http://i2oo.cn/a7qbyy",
"http://i2oo.cn/aWKbGK",
"http://i2oo.cn/aXo2YC",
"http://i2oo.cn/aZHZqr",
"http://i2oo.cn/a2mZfM",
"http://i2oo.cn/acEYP6",
"http://i2oo.cn/adjXgs",
"http://i2oo.cn/aCDWxP",
"http://i2oo.cn/ag6WNj",
"http://i2oo.cn/a5BVXn",
"http://i2oo.cn/ajC7oR",
"http://i2oo.cn/am97nm",
"http://i2oo.cn/a8dT4f",
"http://i2oo.cn/aFfSeT",
"http://i2oo.cn/aqbRvo",
"http://i2oo.cn/a1nRD9",
"http://i2oo.cn/atZQVV",
"http://i2oo.cn/avsPmq",
"http://i2oo.cn/aAXPhB",
"http://i2oo.cn/ayr4LX",
"http://i2oo.cn/azVMc1",
"http://i2oo.cn/fryLuD",
"http://i2oo.cn/fsTLBZ",
"http://i2oo.cn/fhAKTu",
"http://i2oo.cn/faRJkE",
"http://i2oo.cn/ffuJpb",
"http://i2oo.cn/f9PHJA",
"http://i2oo.cn/fw1GbH",
"http://i2oo.cn/fUME1d",
"http://i2oo.cn/fDqE9y",
"http://i2oo.cn/fEKNSK",
"http://i2oo.cn/fGoD5C",
"http://i2oo.cn/fJHDrr",
"http://i2oo.cn/fKmUHM",
"http://i2oo.cn/fMEBZ6",
"http://i2oo.cn/f4jwis",
"http://i2oo.cn/fQDw3P",
"http://i2oo.cn/fR69Qj",
"http://i2oo.cn/fTB36n",
"http://i2oo.cn/f7CfyR",
"http://i2oo.cn/fW9fEm",
"http://i2oo.cn/fXdaYf",
"http://i2oo.cn/r2bwf",
"http://i2oo.cn/sh2ST",
"http://i2oo.cn/hYZ5o",
"http://i2oo.cn/apZr9",
"http://i2oo.cn/fWYHV",
"http://i2oo.cn/3zXZq",
"http://i2oo.cn/w7WiB",
"http://i2oo.cn/BxW3X",
"http://i2oo.cn/DSVQ1",
"http://i2oo.cn/Nv76D",
"http://i2oo.cn/GQTyZ",
"http://i2oo.cn/HtTEu",
"http://i2oo.cn/K4SYE",
"http://i2oo.cn/LiRFb",
"http://i2oo.cn/4LRaA",
"http://i2oo.cn/PFQPH",
"http://i2oo.cn/RJPCd",
"http://i2oo.cn/S84Ay",
"http://i2oo.cn/7G4NK",
"http://i2oo.cn/VkMWC",
"http://i2oo.cn/XNLor",
"http://i2oo.cn/Y5LnM",
"http://i2oo.cn/2UKM6",
"http://i2oo.cn/bgJes",
"http://i2oo.cn/dwHvP",
"http://i2oo.cn/eeHUj",
"http://i2oo.cn/g3GVn",
"http://i2oo.cn/6cEmR",
"http://i2oo.cn/jaEsm",
"http://i2oo.cn/k2NLf",
"http://i2oo.cn/8hDcT",
"http://i2oo.cn/oYUto",
"http://i2oo.cn/qpUB9",
"http://i2oo.cn/iWBTV",
"http://i2oo.cn/1zwjq",
"http://i2oo.cn/u7wpB",
"http://i2oo.cn/vx9JX",
"http://i2oo.cn/xS321",
"http://i2oo.cn/yvf1D",
"http://i2oo.cn/prQf9Z",
"http://i2oo.cn/pptaRu",
"http://i2oo.cn/ph4n5E",
"http://i2oo.cn/pnihzb",
"http://i2oo.cn/pfLhGA",
"http://i2oo.cn/p3FsZH",
"http://i2oo.cn/pwJpqd",
"http://i2oo.cn/pB8pfy",
"http://i2oo.cn/pDGrQK",
"http://i2oo.cn/pNjzgC",
"http://i2oo.cn/pGDyyr",
"http://i2oo.cn/pH6yEM",
"http://i2oo.cn/pKBxX6",
"http://i2oo.cn/pLCAFs",
"http://i2oo.cn/p49AaP",
"http://i2oo.cn/pPdv4j",
"http://i2oo.cn/pRfuCn",
"http://i2oo.cn/pSbtAR",
"http://i2oo.cn/p7ntDm",
"http://i2oo.cn/pVZ1Wf",
"http://i2oo.cn/pXsi8T",
"http://i2oo.cn/pYXiho",
"http://i2oo.cn/p2rqM9",
"http://i2oo.cn/pbVFdV",
"http://i2oo.cn/pcyouq",
"http://i2oo.cn/peToUB",
"http://i2oo.cn/pCA87X",
"http://i2oo.cn/p6Rmk1",
"http://i2oo.cn/p5umsD",
"http://i2oo.cn/pkPkKZ",
"http://i2oo.cn/pm1jbu",
"http://i2oo.cn/poM5tE",
"http://i2oo.cn/pFq5wb",
"http://i2oo.cn/piK6SA",
"http://i2oo.cn/p1ogjH",
"http://i2oo.cn/puHgrd",
"http://i2oo.cn/pvmCHy",
"http://i2oo.cn/pxEe2K",
"http://i2oo.cn/pyjdiC",
"http://i2oo.cn/srDd9r",
"http://i2oo.cn/sp6cRM",
"http://i2oo.cn/shBb66",
"http://i2oo.cn/snC2zs",
"http://i2oo.cn/sf92GP",
"http://i2oo.cn/s3dZYj",
"http://i2oo.cn/swfYqn",
"http://i2oo.cn/sBbYfR",
"http://i2oo.cn/sDnXPm",
"http://i2oo.cn/sNZWgf",
"http://i2oo.cn/sGsVxT",
"http://i2oo.cn/sHXVNo",
"http://i2oo.cn/sKr7X9",
"http://i2oo.cn/sLVToV",
"http://i2oo.cn/sMyTnq",
"http://i2oo.cn/sPTS4B",
"http://i2oo.cn/sQAReX",
"http://i2oo.cn/sSRQv1",
"http://i2oo.cn/sTuQDD",
"http://i2oo.cn/sVPPVZ",
"http://i2oo.cn/sW14mu",
"http://i2oo.cn/sYM4hE",
"http://i2oo.cn/sZqMLb",
"http://i2oo.cn/sbKLcA",
"http://i2oo.cn/scoKuH"
            };
        }
    }
}