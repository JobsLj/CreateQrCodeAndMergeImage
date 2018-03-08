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
                "http://i2oo.cn/njLjuy",
"http://i2oo.cn/nkFjUK",
"http://i2oo.cn/n8J57C",
"http://i2oo.cn/no86mr",
"http://i2oo.cn/nqG6sM",
"http://i2oo.cn/nikgK6",
"http://i2oo.cn/ntNCcs",
"http://i2oo.cn/nu5etP",
"http://i2oo.cn/nAUewj",
"http://i2oo.cn/nxgdTn",
"http://i2oo.cn/nzwcjR",
"http://i2oo.cn/arecrm",
"http://i2oo.cn/as3bJf",
"http://i2oo.cn/ahc22T",
"http://i2oo.cn/aaaZio",
"http://i2oo.cn/af2Z99",
"http://i2oo.cn/a9hYRV",
"http://i2oo.cn/awYX6q",
"http://i2oo.cn/aUpWzB",
"http://i2oo.cn/aDWWGX",
"http://i2oo.cn/aNzVY1",
"http://i2oo.cn/aG77qD",
"http://i2oo.cn/aHx7fZ",
"http://i2oo.cn/aKSTPu",
"http://i2oo.cn/aLvSgE",
"http://i2oo.cn/a4QRxb",
"http://i2oo.cn/aPtRNA",
"http://i2oo.cn/aR4QXH",
"http://i2oo.cn/aSiPod",
"http://i2oo.cn/a7LPny",
"http://i2oo.cn/aVF44K",
"http://i2oo.cn/aXJMeC",
"http://i2oo.cn/aY8LAr",
"http://i2oo.cn/a2GLDM",
"http://i2oo.cn/abkKV6",
"http://i2oo.cn/adNJ8s",
"http://i2oo.cn/ae5JhP",
"http://i2oo.cn/agUHLj",
"http://i2oo.cn/a6gGdn",
"http://i2oo.cn/ajwEuR",
"http://i2oo.cn/akeEBm",
"http://i2oo.cn/a83N7f",
"http://i2oo.cn/aocDkT",
"http://i2oo.cn/aqaDpo",
"http://i2oo.cn/ai2UK9",
"http://i2oo.cn/athBbV",
"http://i2oo.cn/auYw1q",
"http://i2oo.cn/aApwwB",
"http://i2oo.cn/axW9SX",
"http://i2oo.cn/ayz351",
"http://i2oo.cn/fr73rD",
"http://i2oo.cn/fpxfHZ",
"http://i2oo.cn/fhSaZu",
"http://i2oo.cn/fnvniE",
"http://i2oo.cn/ffQn3b",
"http://i2oo.cn/f3thQA",
"http://i2oo.cn/fw4s6H",
"http://i2oo.cn/fBipyd",
"http://i2oo.cn/fDLpEy",
"http://i2oo.cn/fNFrYK",
"http://i2oo.cn/fGHzFC",
"http://i2oo.cn/fHmzfr",
"http://i2oo.cn/fKEyPM",
"http://i2oo.cn/fLjxC6",
"http://i2oo.cn/f4DAxs",
"http://i2oo.cn/fP6ANP",
"http://i2oo.cn/fRBvWj",
"http://i2oo.cn/fSCuon",
"http://i2oo.cn/f79unR",
"http://i2oo.cn/fVdtMm",
"http://i2oo.cn/fXf1ef",
"http://i2oo.cn/rn4Gf",
"http://i2oo.cn/pZMYT",
"http://i2oo.cn/hsLFo",
"http://i2oo.cn/nXLf9",
"http://i2oo.cn/frKPV",
"http://i2oo.cn/3VJCq",
"http://i2oo.cn/9yHxB",
"http://i2oo.cn/BTHNX",
"http://i2oo.cn/UAGW1",
"http://i2oo.cn/NREoD",
"http://i2oo.cn/EuEnZ",
"http://i2oo.cn/HPNMu",
"http://i2oo.cn/J1DeE",
"http://i2oo.cn/LMUvb",
"http://i2oo.cn/MqUUA",
"http://i2oo.cn/PKBVH",
"http://i2oo.cn/Qowmd",
"http://i2oo.cn/SHwsy",
"http://i2oo.cn/Tm9LK",
"http://i2oo.cn/VE3cC",
"http://i2oo.cn/Wjfur",
"http://i2oo.cn/YDfBM",
"http://i2oo.cn/Z6aT6",
"http://i2oo.cn/bBnks",
"http://i2oo.cn/cCnpP",
"http://i2oo.cn/e9hJj",
"http://i2oo.cn/Cdsbn",
"http://i2oo.cn/6fp1R",
"http://i2oo.cn/5bp9m",
"http://i2oo.cn/knrSf",
"http://i2oo.cn/mYz5T",
"http://i2oo.cn/opyzo",
"http://i2oo.cn/FWyH9",
"http://i2oo.cn/qzxZV",
"http://i2oo.cn/17Aqq",
"http://i2oo.cn/txA3B",
"http://i2oo.cn/vSvQX",
"http://i2oo.cn/Avug1",
"http://i2oo.cn/yQtyD",
"http://i2oo.cn/zttEZ",
"http://i2oo.cn/pp41Xu",
"http://i2oo.cn/psiiFE",
"http://i2oo.cn/pnLiab",
"http://i2oo.cn/paFq4A",
"http://i2oo.cn/p3JFCH",
"http://i2oo.cn/p98oAd",
"http://i2oo.cn/pBGoDy",
"http://i2oo.cn/pUk8WK",
"http://i2oo.cn/pNNm8C",
"http://i2oo.cn/pE5mnr",
"http://i2oo.cn/pHUkMM",
"http://i2oo.cn/pJgjd6",
"http://i2oo.cn/pLw5vs",
"http://i2oo.cn/pMe5UP",
"http://i2oo.cn/pP367j",
"http://i2oo.cn/pQcgmn",
"http://i2oo.cn/pSagsR",
"http://i2oo.cn/pT2CKm",
"http://i2oo.cn/pVhecf",
"http://i2oo.cn/pWYdtT",
"http://i2oo.cn/pYpdwo",
"http://i2oo.cn/pZWcT9",
"http://i2oo.cn/p2zbjV",
"http://i2oo.cn/pc7brq",
"http://i2oo.cn/pdx2JB",
"http://i2oo.cn/pCSZ2X",
"http://i2oo.cn/pgvYi1",
"http://i2oo.cn/p5QY9D",
"http://i2oo.cn/pjtXRZ",
"http://i2oo.cn/pm4W6u",
"http://i2oo.cn/p8iVzE",
"http://i2oo.cn/pFLVGb",
"http://i2oo.cn/pqF7YA",
"http://i2oo.cn/p1JTqH",
"http://i2oo.cn/pt8Tfd",
"http://i2oo.cn/pvGSPy",
"http://i2oo.cn/pAkRgK",
"http://i2oo.cn/pyNQxC",
"http://i2oo.cn/pz5QEr",
"http://i2oo.cn/spUPXM",
"http://i2oo.cn/ssg4o6",
"http://i2oo.cn/snw4as",
"http://i2oo.cn/saeM4P",
"http://i2oo.cn/s33Lej",
"http://i2oo.cn/s9cKAn",
"http://i2oo.cn/sBaKDR",
"http://i2oo.cn/sU2JVm",
"http://i2oo.cn/sNhH8f",
"http://i2oo.cn/sEYHhT",
"http://i2oo.cn/sHpGLo",
"http://i2oo.cn/sJWEd9",
"http://i2oo.cn/sKzNuV",
"http://i2oo.cn/sM7NBq",
"http://i2oo.cn/s4xD7B",
"http://i2oo.cn/sQSUkX",
"http://i2oo.cn/sRvUp1",
"http://i2oo.cn/sTQBKD",
"http://i2oo.cn/s7twbZ",
"http://i2oo.cn/sW491u",
"http://i2oo.cn/sXi9wE",
"http://i2oo.cn/sZL3Sb",
"http://i2oo.cn/s2Ff5A",
"http://i2oo.cn/scJfrH",
"http://i2oo.cn/sd8aHd",
"http://i2oo.cn/sCGnZy",
"http://i2oo.cn/sgkhiK",
"http://i2oo.cn/s5Nh3C",
"http://i2oo.cn/sj5sRr",
"http://i2oo.cn/smUp6M",
"http://i2oo.cn/s8gry6",
"http://i2oo.cn/sFwrGs",
"http://i2oo.cn/sqdzYP",
"http://i2oo.cn/s1fyFj",
"http://i2oo.cn/stbyfn",
"http://i2oo.cn/svnxPR",
"http://i2oo.cn/sAZACm",
"http://i2oo.cn/sysvxf",
"http://i2oo.cn/szXvNT",
"http://i2oo.cn/hpruWo",
"http://i2oo.cn/hsVto9",
"http://i2oo.cn/hhytnV",
"http://i2oo.cn/haT1Mq",
"http://i2oo.cn/hfAieB",
"http://i2oo.cn/h9RqvX",
"http://i2oo.cn/hwuqU1",
"http://i2oo.cn/hUPFVD",
"http://i2oo.cn/hD1omZ",
"http://i2oo.cn/hEMosu",
"http://i2oo.cn/hGq8LE",
"http://i2oo.cn/hJKmcb",
"http://i2oo.cn/hKoktA",
"http://i2oo.cn/hMHkBH",
"http://i2oo.cn/h4mjTd",
"http://i2oo.cn/hQE5jy",
"http://i2oo.cn/hRj5pK",
"http://i2oo.cn/hTD6JC",
"http://i2oo.cn/h76gbr",
"http://i2oo.cn/hWBC1M",
"http://i2oo.cn/hXCC96",
"http://i2oo.cn/hZ9eSs",
"http://i2oo.cn/h2dd5P",
"http://i2oo.cn/hcfczj",
"http://i2oo.cn/hdbcHn",
"http://i2oo.cn/hCnbZR",
"http://i2oo.cn/hgZ2qm",
"http://i2oo.cn/h5s23f",
"http://i2oo.cn/hjXZQT",
"http://i2oo.cn/hmrYgo",
"http://i2oo.cn/h8VXy9",
"http://i2oo.cn/hoyXEV",
"http://i2oo.cn/hqTWXq",
"http://i2oo.cn/hiAVFB",
"http://i2oo.cn/htRVaX",
"http://i2oo.cn/huu741",
"http://i2oo.cn/hAPTCD",
"http://i2oo.cn/hx1SAZ",
"http://i2oo.cn/hzMSDu",
"http://i2oo.cn/nrqRWE",
"http://i2oo.cn/nsKQ8b",
"http://i2oo.cn/nhoQhA",
"http://i2oo.cn/naHPMH",
"http://i2oo.cn/nfm4dd",
"http://i2oo.cn/n9EMuy",
"http://i2oo.cn/nwjMUK",
"http://i2oo.cn/nUDL7C",
"http://i2oo.cn/nD6Kmr",
"http://i2oo.cn/nEBKsM",
"http://i2oo.cn/nGCJK6",
"http://i2oo.cn/nJ9Hcs",
"http://i2oo.cn/nKdGtP",
"http://i2oo.cn/nMfGwj",
"http://i2oo.cn/n4bETn",
"http://i2oo.cn/nQnNjR",
"http://i2oo.cn/nRZNrm",
"http://i2oo.cn/nTsDJf",
"http://i2oo.cn/n7XU2T",
"http://i2oo.cn/nWrBio",
"http://i2oo.cn/nXVB99",
"http://i2oo.cn/nYywRV",
"http://i2oo.cn/n2T96q",
"http://i2oo.cn/nbA3zB",
"http://i2oo.cn/ndR3GX",
"http://i2oo.cn/neufY1",
"http://i2oo.cn/ngPaqD",
"http://i2oo.cn/n61afZ",
"http://i2oo.cn/njMnPu",
"http://i2oo.cn/nkqhgE",
"http://i2oo.cn/n8Ksxb",
"http://i2oo.cn/noosNA",
"http://i2oo.cn/nqHpXH",
"http://i2oo.cn/nimrod",
"http://i2oo.cn/ntErny",
"http://i2oo.cn/nu5z4K",
"http://i2oo.cn/nAUyeC",
"http://i2oo.cn/nxgxAr",
"http://i2oo.cn/nzwxDM",
"http://i2oo.cn/areAV6",
"http://i2oo.cn/as3v8s",
"http://i2oo.cn/ahcvhP",
"http://i2oo.cn/aaauLj",
"http://i2oo.cn/af2tdn",
"http://i2oo.cn/a9h1uR",
"http://i2oo.cn/awY1Bm",
"http://i2oo.cn/aUpi7f",
"http://i2oo.cn/aDWqkT",
"http://i2oo.cn/aNzqpo",
"http://i2oo.cn/aG7FK9",
"http://i2oo.cn/aHxobV",
"http://i2oo.cn/aKS81q",
"http://i2oo.cn/aLv8wB",
"http://i2oo.cn/a4QmSX",
"http://i2oo.cn/aPtk51",
"http://i2oo.cn/aR4krD",
"http://i2oo.cn/aSijHZ",
"http://i2oo.cn/a7L5Zu",
"http://i2oo.cn/aVF6iE",
"http://i2oo.cn/aXJ63b",
"http://i2oo.cn/aY8gQA",
"http://i2oo.cn/a2GC6H",
"http://i2oo.cn/abkeyd",
"http://i2oo.cn/adNeEy",
"http://i2oo.cn/ae5dYK",
"http://i2oo.cn/agUcFC",
"http://i2oo.cn/a6gcfr",
"http://i2oo.cn/ajwbPM",
"http://i2oo.cn/ake2C6",
"http://i2oo.cn/a83Zxs",
"http://i2oo.cn/aocZNP",
"http://i2oo.cn/aqaYWj",
"http://i2oo.cn/ai2Xon",
"http://i2oo.cn/athXnR",
"http://i2oo.cn/auYWMm",
"http://i2oo.cn/aApVef",
"http://i2oo.cn/axW7vT",
"http://i2oo.cn/ayz7Uo",
"http://i2oo.cn/fr7TV9",
"http://i2oo.cn/fpxSmV",
"http://i2oo.cn/fhSSsq",
"http://i2oo.cn/fnvRLB",
"http://i2oo.cn/ffQQcX",
"http://i2oo.cn/f3tPt1",
"http://i2oo.cn/fw4PBD",
"http://i2oo.cn/fBi4TZ",
"http://i2oo.cn/fDLMju",
"http://i2oo.cn/fNFMpE",
"http://i2oo.cn/fGJLJb",
"http://i2oo.cn/fH8K2A",
"http://i2oo.cn/fKGJ1H",
"http://i2oo.cn/fLkJ9d",
"http://i2oo.cn/f4NHRy",
"http://i2oo.cn/fP5G5K",
"http://i2oo.cn/fRUEzC",
"http://i2oo.cn/fSgEHr",
"http://i2oo.cn/f7wNZM",
"http://i2oo.cn/fVeDq6",
"http://i2oo.cn/fX3D3s",
"http://i2oo.cn/rn6ks",
"http://i2oo.cn/pZ6pP",
"http://i2oo.cn/hsgJj",
"http://i2oo.cn/nXCbn",
"http://i2oo.cn/fre1R",
"http://i2oo.cn/3Ve9m",
"http://i2oo.cn/9ydSf",
"http://i2oo.cn/BTc5T",
"http://i2oo.cn/UAbzo",
"http://i2oo.cn/NRbH9",
"http://i2oo.cn/Eu2ZV",
"http://i2oo.cn/HPZqq",
"http://i2oo.cn/J1Z3B",
"http://i2oo.cn/LMYQX",
"http://i2oo.cn/MqXg1",
"http://i2oo.cn/PKWyD",
"http://i2oo.cn/QoWEZ",
"http://i2oo.cn/SHVXu",
"http://i2oo.cn/Tm7FE",
"http://i2oo.cn/VE7ab",
"http://i2oo.cn/WjT4A",
"http://i2oo.cn/YDSCH",
"http://i2oo.cn/Z6RAd",
"http://i2oo.cn/bBRDy",
"http://i2oo.cn/cCQWK",
"http://i2oo.cn/e9P8C",
"http://i2oo.cn/CdPnr",
"http://i2oo.cn/6f4MM",
"http://i2oo.cn/5bMd6",
"http://i2oo.cn/knLvs",
"http://i2oo.cn/mZLUP",
"http://i2oo.cn/osK7j",
"http://i2oo.cn/FXJmn",
"http://i2oo.cn/irJsR",
"http://i2oo.cn/1VHKm",
"http://i2oo.cn/tyGcf",
"http://i2oo.cn/vTEtT",
"http://i2oo.cn/AAEwo",
"http://i2oo.cn/yRNT9",
"http://i2oo.cn/zuDjV",
"http://i2oo.cn/ppPDrq",
"http://i2oo.cn/ps1UJB",
"http://i2oo.cn/pnMB2X",
"http://i2oo.cn/paqwi1",
"http://i2oo.cn/p3Kw9D",
"http://i2oo.cn/p9o9RZ",
"http://i2oo.cn/pBH36u",
"http://i2oo.cn/pUmfzE",
"http://i2oo.cn/pNEfGb",
"http://i2oo.cn/pEjaYA",
"http://i2oo.cn/pHDnqH",
"http://i2oo.cn/pJ6nfd",
"http://i2oo.cn/pLBhPy",
"http://i2oo.cn/pMCsgK",
"http://i2oo.cn/pP9pxC",
"http://i2oo.cn/pQdpEr",
"http://i2oo.cn/pSfrXM",
"http://i2oo.cn/pT2zo6",
"http://i2oo.cn/pVhzas",
"http://i2oo.cn/pWYy4P",
"http://i2oo.cn/pYpxej",
"http://i2oo.cn/pZWAAn",
"http://i2oo.cn/p2zADR",
"http://i2oo.cn/pc7vVm",
"http://i2oo.cn/pdxu8f",
"http://i2oo.cn/pCSuhT",
"http://i2oo.cn/pgvtLo",
"http://i2oo.cn/p5Q1d9",
"http://i2oo.cn/pjtiuV",
"http://i2oo.cn/pm4iBq",
"http://i2oo.cn/p8iq7B",
"http://i2oo.cn/pFLFkX",
"http://i2oo.cn/pqFFp1",
"http://i2oo.cn/p1JoKD",
"http://i2oo.cn/pt88bZ",
"http://i2oo.cn/pvGm1u",
"http://i2oo.cn/pAkmwE",
"http://i2oo.cn/pyNkSb",
"http://i2oo.cn/pz5j5A",
"http://i2oo.cn/spUjrH",
"http://i2oo.cn/ssg5Hd",
"http://i2oo.cn/snw6Zy",
"http://i2oo.cn/saegiK",
"http://i2oo.cn/s33g3C",
"http://i2oo.cn/s9cCRr",
"http://i2oo.cn/sBae6M",
"http://i2oo.cn/sU2dy6",
"http://i2oo.cn/sNhdGs",
"http://i2oo.cn/sEYcYP",
"http://i2oo.cn/sHpbFj",
"http://i2oo.cn/sJWbfn",
"http://i2oo.cn/sKz2PR",
"http://i2oo.cn/sM7ZCm",
"http://i2oo.cn/s4xYxf",
"http://i2oo.cn/sQSYNT",
"http://i2oo.cn/sRvXWo",
"http://i2oo.cn/sTQWo9",
"http://i2oo.cn/s7tWnV",
"http://i2oo.cn/sW4VMq",
"http://i2oo.cn/sXi7eB",
"http://i2oo.cn/sZLTvX",
"http://i2oo.cn/s2FTU1",
"http://i2oo.cn/scJSVD",
"http://i2oo.cn/sd8RmZ",
"http://i2oo.cn/sCGRsu",
"http://i2oo.cn/sgkQLE",
"http://i2oo.cn/s5NPcb",
"http://i2oo.cn/sj54tA",
"http://i2oo.cn/smU4BH",
"http://i2oo.cn/s8gMTd",
"http://i2oo.cn/sFwLjy",
"http://i2oo.cn/sqeLpK",
"http://i2oo.cn/s13KJC",
"http://i2oo.cn/stcJbr",
"http://i2oo.cn/svaH1M",
"http://i2oo.cn/sA2H96",
"http://i2oo.cn/syhGSs",
"http://i2oo.cn/szYE5P",
"http://i2oo.cn/hppNzj",
"http://i2oo.cn/hsWNHn",
"http://i2oo.cn/hhzDZR",
"http://i2oo.cn/ha7Uqm",
"http://i2oo.cn/hfxU3f",
"http://i2oo.cn/h9SBQT",
"http://i2oo.cn/hwvwgo",
"http://i2oo.cn/hUQ9y9",
"http://i2oo.cn/hDt9EV",
"http://i2oo.cn/hE43Xq",
"http://i2oo.cn/hGifFB",
"http://i2oo.cn/hJLfaX",
"http://i2oo.cn/hKFa41",
"http://i2oo.cn/hMJnCD",
"http://i2oo.cn/h48hAZ",
"http://i2oo.cn/hQGhDu",
"http://i2oo.cn/hRksWE",
"http://i2oo.cn/hTNp8b",
"http://i2oo.cn/h75phA",
"http://i2oo.cn/hWUrMH",
"http://i2oo.cn/hXCzdd",
"http://i2oo.cn/hZ9yuy",
"http://i2oo.cn/h2dyUK",
"http://i2oo.cn/hcfx7C",
"http://i2oo.cn/hdbAmr",
"http://i2oo.cn/hCnAsM",
"http://i2oo.cn/hgZvK6",
"http://i2oo.cn/h5sucs",
"http://i2oo.cn/hjXttP",
"http://i2oo.cn/hmrtwj",
"http://i2oo.cn/h8V1Tn",
"http://i2oo.cn/hoyijR",
"http://i2oo.cn/hqTirm",
"http://i2oo.cn/hiAqJf",
"http://i2oo.cn/htRF2T",
"http://i2oo.cn/huuoio",
"http://i2oo.cn/hAPo99",
"http://i2oo.cn/hx18RV",
"http://i2oo.cn/hzMm6q",
"http://i2oo.cn/nrqkzB",
"http://i2oo.cn/nsKkGX",
"http://i2oo.cn/nhojY1",
"http://i2oo.cn/naH5qD",
"http://i2oo.cn/nfm5fZ",
"http://i2oo.cn/n9E6Pu",
"http://i2oo.cn/nwjggE",
"http://i2oo.cn/nUDCxb",
"http://i2oo.cn/nD6CNA",
"http://i2oo.cn/nEBeXH",
"http://i2oo.cn/nGCdod",
"http://i2oo.cn/nJ9dny",
"http://i2oo.cn/nKdc4K",
"http://i2oo.cn/nMfbeC",
"http://i2oo.cn/n4b2Ar",
"http://i2oo.cn/nQn2DM",
"http://i2oo.cn/nRZZV6",
"http://i2oo.cn/nTsY8s",
"http://i2oo.cn/n7XYhP",
"http://i2oo.cn/nWrXLj",
"http://i2oo.cn/nXVWdn",
"http://i2oo.cn/nYyVuR",
"http://i2oo.cn/n2TVBm",
"http://i2oo.cn/nbA77f",
"http://i2oo.cn/ndRTkT",
"http://i2oo.cn/neuTpo",
"http://i2oo.cn/ngPSK9",
"http://i2oo.cn/n61RbV",
"http://i2oo.cn/njMQ1q",
"http://i2oo.cn/nkqQwB",
"http://i2oo.cn/n8KPSX",
"http://i2oo.cn/noo451",
"http://i2oo.cn/nqH4rD",
"http://i2oo.cn/nimMHZ",
"http://i2oo.cn/ntELZu",
"http://i2oo.cn/nujKiE",
"http://i2oo.cn/nADK3b",
"http://i2oo.cn/nx6JQA",
"http://i2oo.cn/nzBH6H",
"http://i2oo.cn/arCGyd",
"http://i2oo.cn/as9GEy",
"http://i2oo.cn/ahdEYK",
"http://i2oo.cn/aafNFC",
"http://i2oo.cn/afbNfr",
"http://i2oo.cn/a9nDPM",
"http://i2oo.cn/awZUC6",
"http://i2oo.cn/aUsBxs",
"http://i2oo.cn/aDXBNP",
"http://i2oo.cn/aErwWj",
"http://i2oo.cn/aGV9on",
"http://i2oo.cn/aHy9nR",
"http://i2oo.cn/aKT3Mm",
"http://i2oo.cn/aLAfef",
"http://i2oo.cn/a4RavT",
"http://i2oo.cn/aPuaUo",
"http://i2oo.cn/aRPnV9",
"http://i2oo.cn/aS1hmV",
"http://i2oo.cn/a7Mhsq",
"http://i2oo.cn/aVqsLB",
"http://i2oo.cn/aXKpcX",
"http://i2oo.cn/aYort1",
"http://i2oo.cn/a2HrBD",
"http://i2oo.cn/abkzTZ",
"http://i2oo.cn/adNyju",
"http://i2oo.cn/ae5ypE",
"http://i2oo.cn/agUxJb"
            };
        }
    }
}