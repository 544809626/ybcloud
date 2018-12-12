namespace ValuationAnchor.Models
{
    using System.ComponentModel.DataAnnotations;
    public class InToken
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        [StringLength(18, ErrorMessage = "appId应为18位字符"), Required(ErrorMessage = "appId不能为空")]
        public string AppId { get; set; }
        /// <summary>
        /// 签名时间
        /// </summary>
        [Required(ErrorMessage = "signTime不能为空")]
        [StringLength(14, ErrorMessage = "signTime应为14位字符")]
        public string SignTime { get; set; }
        /// <summary>
        /// 随机数
        /// </summary>
        [Required(ErrorMessage = "random不能为空")]
        [StringLength(6, ErrorMessage = "random应为6位字符")]
        public string Random { get; set; }
        /// <summary>
        /// 签名：MD5(AppId+SignDate+Random+SignKey)
        /// </summary>
        [Required(ErrorMessage = "signCode不能为空")]
        [StringLength(32, ErrorMessage = "signCode应为32位字符")]
        public string SignCode { get; set; }
    }

   
}
