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
            var filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Pictures\background.jpg";
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
                "http://i2oo.cn/pShKCu",
                "http://i2oo.cn/pTYJxE",
                "http://i2oo.cn/pVpJNb",
                "http://i2oo.cn/pWWHWA",
                "http://i2oo.cn/pXzGoH",
                "http://i2oo.cn/pZ7Gnd",
                "http://i2oo.cn/p2xEMy",
                "http://i2oo.cn/pcSNeK",
                "http://i2oo.cn/pdvDvC",
                "http://i2oo.cn/pCQDDr",
                "http://i2oo.cn/pgtUVM",
                "http://i2oo.cn/p54Bm6",
                "http://i2oo.cn/pjiBhs",
                "http://i2oo.cn/pmLwLP",
                "http://i2oo.cn/p8F9cj",
                "http://i2oo.cn/pFJ3un",
                "http://i2oo.cn/pq83BR",
                "http://i2oo.cn/p1GfTm",
                "http://i2oo.cn/ptkakf",
                "http://i2oo.cn/pvNapT",
                "http://i2oo.cn/pA5nJo",
                "http://i2oo.cn/pyUhb9",
                "http://i2oo.cn/pzgs1V",
                "http://i2oo.cn/spws9q",
                "http://i2oo.cn/ssepSB",
                "http://i2oo.cn/sn3r5X",
                "http://i2oo.cn/sabzz1",
                "http://i2oo.cn/s3nzHD",
                "http://i2oo.cn/s9ZyZZ",
                "http://i2oo.cn/sBsxqu",
                "http://i2oo.cn/sUXx3E",
                "http://i2oo.cn/sNrAQb",
                "http://i2oo.cn/sEVvgA",
                "http://i2oo.cn/sGyuyH",
                "http://i2oo.cn/sJTuEd",
                "http://i2oo.cn/sKAtXy",
                "http://i2oo.cn/sMR1FK",
                "http://i2oo.cn/s4u1aC",
                "http://i2oo.cn/sQPiPr",
                "http://i2oo.cn/sR1qCM",
                "http://i2oo.cn/sTMFA6",
                "http://i2oo.cn/s7qFNs",
                "http://i2oo.cn/sWKoWP",
                "http://i2oo.cn/sXo88j",
                "http://i2oo.cn/sZH8nn",
                "http://i2oo.cn/s2mmMR",
                "http://i2oo.cn/scEkdm",
                "http://i2oo.cn/sdjjvf",
                "http://i2oo.cn/sCDjUT",
                "http://i2oo.cn/sg657o",
                "http://i2oo.cn/s5B6m9",
                "http://i2oo.cn/sjC6sV",
                "http://i2oo.cn/sm9gKq",
                "http://i2oo.cn/s8dCcB",
                "http://i2oo.cn/sFfetX",
                "http://i2oo.cn/sqbew1",
                "http://i2oo.cn/s1ndTD",
                "http://i2oo.cn/stZcjZ",
                "http://i2oo.cn/svscru",
                "http://i2oo.cn/sAXbJE",
                "http://i2oo.cn/syr22b",
                "http://i2oo.cn/szVZiA",
                "http://i2oo.cn/hryZ9H",
                "http://i2oo.cn/hsTYRd",
                "http://i2oo.cn/hhAX6y",
                "http://i2oo.cn/haRWzK",
                "http://i2oo.cn/hfuWGC",
                "http://i2oo.cn/h9PVZr",
                "http://i2oo.cn/hw17qM",
                "http://i2oo.cn/hUM7f6",
                "http://i2oo.cn/hDqTQs",
                "http://i2oo.cn/hEKSgP",
                "http://i2oo.cn/hGoRxj",
                "http://i2oo.cn/hJHREn",
                "http://i2oo.cn/hKmQXR",
                "http://i2oo.cn/hMEPom",
                "http://i2oo.cn/h4jPaf",
                "http://i2oo.cn/hQD44T",
                "http://i2oo.cn/hR6Meo",
                "http://i2oo.cn/hTBLA9",
                "http://i2oo.cn/h7CLDV",
                "http://i2oo.cn/hW9KVq",
                "http://i2oo.cn/hXdJ8B",
                "http://i2oo.cn/hZfJhX",
                "http://i2oo.cn/h2bHL1",
                "http://i2oo.cn/hcnGdD",
                "http://i2oo.cn/hdZEuZ",
                "http://i2oo.cn/hCsEBu",
                "http://i2oo.cn/hgXN7E",
                "http://i2oo.cn/h5rDkb",
                "http://i2oo.cn/hjVDpA",
                "http://i2oo.cn/hkyUKH",
                "http://i2oo.cn/h8TBbd",
                "http://i2oo.cn/hoAw1y",
                "http://i2oo.cn/hqRwwK",
                "http://i2oo.cn/hiu9SC",
                "http://i2oo.cn/htP3jr",
                "http://i2oo.cn/hu13rM",
                "http://i2oo.cn/hAMfH6",
                "http://i2oo.cn/hxqa2s",
                "http://i2oo.cn/hzKniP",
                "http://i2oo.cn/nron3j",
                "http://i2oo.cn/nsHhRn",
                "http://i2oo.cn/nhms6R",
                "http://i2oo.cn/naEpym",
                "http://i2oo.cn/nfjpGf",
                "http://i2oo.cn/n9DrYT",
                "http://i2oo.cn/nwgzFo",
                "http://i2oo.cn/nUwzf9",
                "http://i2oo.cn/nDeyPV",
                "http://i2oo.cn/nE3xCq",
                "http://i2oo.cn/nGcAxB",
                "http://i2oo.cn/nJaANX",
                "http://i2oo.cn/nK2vW1",
                "http://i2oo.cn/nMhuoD",
                "http://i2oo.cn/n4YunZ",
                "http://i2oo.cn/nQptMu",
                "http://i2oo.cn/nRW1eE",
                "http://i2oo.cn/nSzivb",
                "http://i2oo.cn/n77iUA",
                "http://i2oo.cn/nVxqVH",
                "http://i2oo.cn/nXSFmd",
                "http://i2oo.cn/nYvFsy",
                "http://i2oo.cn/n2QoLK",
                "http://i2oo.cn/nbt8cC",
                "http://i2oo.cn/nd4mur",
                "http://i2oo.cn/neimBM",
                "http://i2oo.cn/ngLkT6",
                "http://i2oo.cn/n6Fjks",
                "http://i2oo.cn/njJjpP",
                "http://i2oo.cn/nk85Jj",
                "http://i2oo.cn/n8G6bn",
                "http://i2oo.cn/nokg1R",
                "http://i2oo.cn/nqNg9m",
                "http://i2oo.cn/ni5CSf",
                "http://i2oo.cn/ntUe5T",
                "http://i2oo.cn/nugdzo",
                "http://i2oo.cn/nAwdH9",
                "http://i2oo.cn/nxecZV",
                "http://i2oo.cn/nz3bqq",
                "http://i2oo.cn/arcb3B",
                "http://i2oo.cn/asa2QX",
                "http://i2oo.cn/ah2Zg1",
                "http://i2oo.cn/aahYyD",
                "http://i2oo.cn/afYYEZ",
                "http://i2oo.cn/a9pXXu",
                "http://i2oo.cn/awWWFE",
                "http://i2oo.cn/aBzWab",
                "http://i2oo.cn/aD7V4A",
                "http://i2oo.cn/aNx7CH",
                "http://i2oo.cn/aGSTAd",
                "http://i2oo.cn/aHvTDy",
                "http://i2oo.cn/aKQSWK",
                "http://i2oo.cn/aLtR8C",
                "http://i2oo.cn/a44Rnr",
                "http://i2oo.cn/aPiQMM",
                "http://i2oo.cn/aRLPd6",
                "http://i2oo.cn/aSF4vs",
                "http://i2oo.cn/a7J4UP",
                "http://i2oo.cn/aV8M7j",
                "http://i2oo.cn/aXGLmn",
                "http://i2oo.cn/aYkLsR",
                "http://i2oo.cn/a2NKKm",
                "http://i2oo.cn/ab5Jcf",
                "http://i2oo.cn/adUHtT",
                "http://i2oo.cn/aegHwo",
                "http://i2oo.cn/agwGT9",
                "http://i2oo.cn/a6eEjV",
                "http://i2oo.cn/aj3Erq",
                "http://i2oo.cn/akcNJB",
                "http://i2oo.cn/a8aD2X",
                "http://i2oo.cn/ao2Ui1",
                "http://i2oo.cn/aqhU9D",
                "http://i2oo.cn/aiYBRZ",
                "http://i2oo.cn/atpw6u",
                "http://i2oo.cn/auW9zE",
                "http://i2oo.cn/avz9Gb",
                "http://i2oo.cn/ax73YA",
                "http://i2oo.cn/ayxfqH",
                "http://i2oo.cn/frSffd",
                "http://i2oo.cn/fpvaPy",
                "http://i2oo.cn/fhQngK",
                "http://i2oo.cn/fnthxC",
                "http://i2oo.cn/ff4hEr",
                "http://i2oo.cn/f3isXM",
                "http://i2oo.cn/fwLpo6",
                "http://i2oo.cn/fBFpas",
                "http://i2oo.cn/fDJr4P",
                "http://i2oo.cn/fNmzej",
                "http://i2oo.cn/fGEyAn",
                "http://i2oo.cn/fHjyDR",
                "http://i2oo.cn/fKDxVm",
                "http://i2oo.cn/fL6A8f",
                "http://i2oo.cn/f4BAhT",
                "http://i2oo.cn/fPCvLo",
                "http://i2oo.cn/fR9ud9",
                "http://i2oo.cn/fSdtuV",
                "http://i2oo.cn/f7ftBq",
                "http://i2oo.cn/fVb17B",
                "http://i2oo.cn/fXnikX",
                "http://i2oo.cn/rsM4X",
                "http://i2oo.cn/pXLe1",
                "http://i2oo.cn/hrKAD",
                "http://i2oo.cn/nVKDZ",
                "http://i2oo.cn/ayJVu",
                "http://i2oo.cn/3TH8E",
                "http://i2oo.cn/9AHhb",
                "http://i2oo.cn/BRGLA",
                "http://i2oo.cn/UuEdH",
                "http://i2oo.cn/NPNud",
                "http://i2oo.cn/E1NBy",
                "http://i2oo.cn/HMD7K",
                "http://i2oo.cn/JqUkC",
                "http://i2oo.cn/LKUsr",
                "http://i2oo.cn/MoBKM",
                "http://i2oo.cn/PHwb6",
                "http://i2oo.cn/Qm9ts",
                "http://i2oo.cn/SE9wP",
                "http://i2oo.cn/Tj3Sj",
                "http://i2oo.cn/VDfjn",
                "http://i2oo.cn/W6frR",
                "http://i2oo.cn/YBaHm",
                "http://i2oo.cn/ZCn2f",
                "http://i2oo.cn/b9hiT",
                "http://i2oo.cn/cdh3o",
                "http://i2oo.cn/efsR9",
                "http://i2oo.cn/Cbp6V",
                "http://i2oo.cn/6nryq",
                "http://i2oo.cn/5ZrGB",
                "http://i2oo.cn/kpzYX",
                "http://i2oo.cn/mWyF1",
                "http://i2oo.cn/8zyfD",
                "http://i2oo.cn/F7xPZ",
                "http://i2oo.cn/qxACu",
                "http://i2oo.cn/1SvxE",
                "http://i2oo.cn/tvvNb",
                "http://i2oo.cn/vQuWA",
                "http://i2oo.cn/AttoH",
                "http://i2oo.cn/y4tnd",
                "http://i2oo.cn/zi1My",
                "http://i2oo.cn/ppLieK",
                "http://i2oo.cn/psFqvC",
                "http://i2oo.cn/pnJqDr",
                "http://i2oo.cn/pa8FVM",
                "http://i2oo.cn/p3Gom6",
                "http://i2oo.cn/p9kohs",
                "http://i2oo.cn/pBN8LP",
                "http://i2oo.cn/pU5mcj",
                "http://i2oo.cn/pNUkun",
                "http://i2oo.cn/pEgkBR",
                "http://i2oo.cn/pHwjTm",
                "http://i2oo.cn/pJe5kf",
                "http://i2oo.cn/pL35pT",
                "http://i2oo.cn/pMc6Jo",
                "http://i2oo.cn/pPagb9",
                "http://i2oo.cn/pQ2C1V",
                "http://i2oo.cn/pShC9q",
                "http://i2oo.cn/pTYeSB",
                "http://i2oo.cn/pVpd5X",
                "http://i2oo.cn/pWWcz1",
                "http://i2oo.cn/pXzcHD",
                "http://i2oo.cn/pZ7bZZ",
                "http://i2oo.cn/p2x2qu",
                "http://i2oo.cn/pcS23E",
                "http://i2oo.cn/pdvZQb",
                "http://i2oo.cn/pCQYgA",
                "http://i2oo.cn/pgtXyH",
                "http://i2oo.cn/p54XEd",
                "http://i2oo.cn/pjiWXy",
                "http://i2oo.cn/pmLVFK",
                "http://i2oo.cn/p8FVaC",
                "http://i2oo.cn/pFJ7Pr",
                "http://i2oo.cn/pq8TCM",
                "http://i2oo.cn/p1GSA6",
                "http://i2oo.cn/ptkSNs",
                "http://i2oo.cn/pvNRWP",
                "http://i2oo.cn/pA5Q8j",
                "http://i2oo.cn/pyUQnn",
                "http://i2oo.cn/pzgPMR",
                "http://i2oo.cn/spw4dm",
                "http://i2oo.cn/sseMvf",
                "http://i2oo.cn/sn3MUT",
                "http://i2oo.cn/sacL7o",
                "http://i2oo.cn/s3aKm9",
                "http://i2oo.cn/s92KsV",
                "http://i2oo.cn/sBhJKq",
                "http://i2oo.cn/sUYHcB",
                "http://i2oo.cn/sNpGtX",
                "http://i2oo.cn/sEWGw1",
                "http://i2oo.cn/sGzETD",
                "http://i2oo.cn/sJ7NjZ",
                "http://i2oo.cn/sKxNru",
                "http://i2oo.cn/sMSDJE",
                "http://i2oo.cn/s4vU2b",
                "http://i2oo.cn/sQQBiA",
                "http://i2oo.cn/sRtB9H",
                "http://i2oo.cn/sT4wRd",
                "http://i2oo.cn/s7i96y",
                "http://i2oo.cn/sWL3zK",
                "http://i2oo.cn/sXF3GC",
                "http://i2oo.cn/sZJfZr",
                "http://i2oo.cn/s28aqM",
                "http://i2oo.cn/scGaf6",
                "http://i2oo.cn/sdknQs",
                "http://i2oo.cn/sCNhgP",
                "http://i2oo.cn/sg5sxj",
                "http://i2oo.cn/s5UsEn",
                "http://i2oo.cn/sjgpXR",
                "http://i2oo.cn/smwrom",
                "http://i2oo.cn/s8eraf",
                "http://i2oo.cn/sFfz4T",
                "http://i2oo.cn/sqbyeo",
                "http://i2oo.cn/s1nxA9",
                "http://i2oo.cn/stZxDV",
                "http://i2oo.cn/svsAVq",
                "http://i2oo.cn/sAXv8B",
                "http://i2oo.cn/syrvhX",
                "http://i2oo.cn/szVuL1",
                "http://i2oo.cn/hrytdD",
                "http://i2oo.cn/hsT1uZ",
                "http://i2oo.cn/hhA1Bu",
                "http://i2oo.cn/haRi7E",
                "http://i2oo.cn/hfuqkb",
                "http://i2oo.cn/h9PqpA",
                "http://i2oo.cn/hw1FKH",
                "http://i2oo.cn/hUMobd",
                "http://i2oo.cn/hDq81y",
                "http://i2oo.cn/hEK8wK",
                "http://i2oo.cn/hGomSC",
                "http://i2oo.cn/hJHkjr",
                "http://i2oo.cn/hKmkrM",
                "http://i2oo.cn/hMEjH6",
                "http://i2oo.cn/h4j52s",
                "http://i2oo.cn/hQD6iP",
                "http://i2oo.cn/hR663j",
                "http://i2oo.cn/hTBgRn",
                "http://i2oo.cn/h7CC6R",
                "http://i2oo.cn/hW9eym",
                "http://i2oo.cn/hXdeGf",
                "http://i2oo.cn/hZfdYT",
                "http://i2oo.cn/h2bcFo",
                "http://i2oo.cn/hcncf9",
                "http://i2oo.cn/hdZbPV",
                "http://i2oo.cn/hCs2Cq",
                "http://i2oo.cn/hgXZxB",
                "http://i2oo.cn/h5rZNX",
                "http://i2oo.cn/hjVYW1",
                "http://i2oo.cn/hkyXoD",
                "http://i2oo.cn/h8TXnZ",
                "http://i2oo.cn/hoAWMu",
                "http://i2oo.cn/hqRVeE",
                "http://i2oo.cn/hiu7vb",
                "http://i2oo.cn/htP7UA",
                "http://i2oo.cn/hu1TVH",
                "http://i2oo.cn/hAMSmd",
                "http://i2oo.cn/hxqSsy",
                "http://i2oo.cn/hzKRLK",
                "http://i2oo.cn/nroQcC",
                "http://i2oo.cn/nsHPur",
                "http://i2oo.cn/nhmPBM",
                "http://i2oo.cn/naE4T6",
                "http://i2oo.cn/nfjMks",
                "http://i2oo.cn/n9DMpP",
                "http://i2oo.cn/nw6LJj",
                "http://i2oo.cn/nUBKbn",
                "http://i2oo.cn/nDCJ1R",
                "http://i2oo.cn/nE9J9m",
                "http://i2oo.cn/nGdHSf",
                "http://i2oo.cn/nJfG5T",
                "http://i2oo.cn/nKbEzo",
                "http://i2oo.cn/nMnEH9",
                "http://i2oo.cn/n4ZNZV",
                "http://i2oo.cn/nQsDqq",
                "http://i2oo.cn/nRXD3B",
                "http://i2oo.cn/nTrUQX",
                "http://i2oo.cn/n7VBg1",
                "http://i2oo.cn/nVywyD",
                "http://i2oo.cn/nXTwEZ",
                "http://i2oo.cn/nYA9Xu",
                "http://i2oo.cn/n2R3FE",
                "http://i2oo.cn/nbu3ab",
                "http://i2oo.cn/ndPf4A",
                "http://i2oo.cn/ne1aCH",
                "http://i2oo.cn/ngMnAd",
                "http://i2oo.cn/n6qnDy",
                "http://i2oo.cn/njKhWK",
                "http://i2oo.cn/nkos8C",
                "http://i2oo.cn/n8Hsnr",
                "http://i2oo.cn/nompMM",
                "http://i2oo.cn/nqErd6",
                "http://i2oo.cn/ni5zvs",
                "http://i2oo.cn/ntUzUP",
                "http://i2oo.cn/nugy7j",
                "http://i2oo.cn/nAwxmn",
                "http://i2oo.cn/nxexsR",
                "http://i2oo.cn/nz3AKm",
                "http://i2oo.cn/arcvcf",
                "http://i2oo.cn/asautT",
                "http://i2oo.cn/ah2uwo",
                "http://i2oo.cn/aahtT9",
                "http://i2oo.cn/afY1jV",
                "http://i2oo.cn/a9p1rq",
                "http://i2oo.cn/awWiJB",
                "http://i2oo.cn/aBzq2X",
                "http://i2oo.cn/aD7Fi1",
                "http://i2oo.cn/aNxF9D",
                "http://i2oo.cn/aGSoRZ",
                "http://i2oo.cn/aHv86u",
                "http://i2oo.cn/aKQmzE",
                "http://i2oo.cn/aLtmGb",
                "http://i2oo.cn/a44kYA",
                "http://i2oo.cn/aPijqH",
                "http://i2oo.cn/aRLjfd",
                "http://i2oo.cn/aSF5Py",
                "http://i2oo.cn/a7J6gK",
                "http://i2oo.cn/aV8gxC",
                "http://i2oo.cn/aXGgEr",
                "http://i2oo.cn/aYkCXM",
                "http://i2oo.cn/a2Neo6",
                "http://i2oo.cn/ab5eas",
                "http://i2oo.cn/adUd4P",
                "http://i2oo.cn/aegcej",
                "http://i2oo.cn/agwbAn",
                "http://i2oo.cn/a6ebDR",
                "http://i2oo.cn/aj32Vm",
                "http://i2oo.cn/akcZ8f",
                "http://i2oo.cn/a8aZhT",
                "http://i2oo.cn/ao2YLo",
                "http://i2oo.cn/aqhXd9",
                "http://i2oo.cn/aiYWuV",
                "http://i2oo.cn/atpWBq",
                "http://i2oo.cn/auWV7B",
                "http://i2oo.cn/avz7kX",
                "http://i2oo.cn/ax77p1",
                "http://i2oo.cn/ayxTKD",
                "http://i2oo.cn/frSSbZ",
                "http://i2oo.cn/fpvR1u",
                "http://i2oo.cn/fhQRwE",
                "http://i2oo.cn/fntQSb",
                "http://i2oo.cn/ff4P5A",
                "http://i2oo.cn/f3iPrH",
                "http://i2oo.cn/fwL4Hd",
                "http://i2oo.cn/fBFMZy",
                "http://i2oo.cn/fDJLiK",
                "http://i2oo.cn/fN8L3C",
                "http://i2oo.cn/fGGKRr",
                "http://i2oo.cn/fHkJ6M",
                "http://i2oo.cn/fKNHy6",
                "http://i2oo.cn/fL5HGs",
                "http://i2oo.cn/f4UGYP",
                "http://i2oo.cn/fPgEFj",
                "http://i2oo.cn/fRwEfn",
                "http://i2oo.cn/fSeNPR",
                "http://i2oo.cn/f73DCm",
                "http://i2oo.cn/fVcUxf",
                "http://i2oo.cn/fXaUNT",
                "http://i2oo.cn/rsgiT",
                "http://i2oo.cn/pXg3o",
                "http://i2oo.cn/hrCR9",
                "http://i2oo.cn/nVe6V",
                "http://i2oo.cn/aydyq",
                "http://i2oo.cn/3TdGB",
                "http://i2oo.cn/9AcYX",
                "http://i2oo.cn/BRbF1",
                "http://i2oo.cn/UubfD",
                "http://i2oo.cn/NP2PZ",
                "http://i2oo.cn/E1ZCu",
                "http://i2oo.cn/HMYxE",
                "http://i2oo.cn/JqYNb",
                "http://i2oo.cn/LKXWA",
                "http://i2oo.cn/MoWoH",
                "http://i2oo.cn/PHWnd",
                "http://i2oo.cn/QmVMy",
                "http://i2oo.cn/SE7eK",
                "http://i2oo.cn/TjTvC",
                "http://i2oo.cn/VDTDr",
                "http://i2oo.cn/W6SVM",
                "http://i2oo.cn/YBRm6",
                "http://i2oo.cn/ZCRhs",
                "http://i2oo.cn/b9QLP",
                "http://i2oo.cn/cdPcj",
                "http://i2oo.cn/ef4un",
                "http://i2oo.cn/Cb4BR",
                "http://i2oo.cn/6nMTm",
                "http://i2oo.cn/5ZLkf",
                "http://i2oo.cn/ksLpT",
                "http://i2oo.cn/mXKJo",
                "http://i2oo.cn/orJb9",
                "http://i2oo.cn/FVH1V",
                "http://i2oo.cn/qyH9q",
                "http://i2oo.cn/1TGSB",
                "http://i2oo.cn/tAE5X",
                "http://i2oo.cn/vRNz1",
                "http://i2oo.cn/AuNHD",
                "http://i2oo.cn/yPDZZ",
                "http://i2oo.cn/z1Uqu",
                "http://i2oo.cn/ppMU3E",
                "http://i2oo.cn/psqBQb",
                "http://i2oo.cn/pnKwgA",
                "http://i2oo.cn/pao9yH"
            };
        }
    }
}