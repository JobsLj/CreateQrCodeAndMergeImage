using Yuanbei.Messaging.SDK;

namespace CreateQrCodeAndMergeImage
{
    [MessageEventInfo(EventName = "main-web.order.ordersuccess")]
    public class OrderSuccessEntity
    {
        /// <summary>
        /// 店铺的id
        /// </summary>
        public int AccountId { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 订单支付金额
        /// </summary>
        public decimal RealPayMoney { get; set; }

        /// <summary>
        /// 订单产品类别 1业务 2实物 3第三方 4 手机充值
        /// </summary>
        public int OrderTypeId { get; set; }
    }
}
