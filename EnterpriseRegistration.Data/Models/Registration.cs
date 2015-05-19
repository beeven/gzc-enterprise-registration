using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace EnterpriseRegistration.Data.Models
{
    public class Registration : DynamicObject
    {
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterPriseName { get; set; }

        /// <summary>
        /// 企业简称
        /// </summary>
        public string EnterPriseAbbreviation { get; set; }

        /// <summary>
        /// 主管海关改为“拟申请开展业务的现场海关”
        /// </summary>
        public string CustomsName { get; set; }

        /// <summary>
        /// 企业海关编码
        /// </summary>
        public string CustomsID { get; set; }

        /// <summary>
        /// 网购保税进口 0为是，1为否
        /// </summary>
        public int OnlineFlag { get; set; }

        /// <summary>
        /// 直购进口 0为是，1为否
        /// </summary>
        public int DirectFlag { get; set; }

        ///// <summary>
        ///// 零售出口 0为是，1为否
        ///// </summary>
        //public int RetailFlag { get; set; }


        /// <summary>
        /// 跨境贸易电子商务经营企业 0为是，1为否
        /// </summary>
        public int ManagementFlag { get; set; }

        /// <summary>
        /// 跨境贸易电子商务交易企业 0为是，1为否
        /// </summary>
        public int TransactionFlag { get; set; }

        /// <summary>
        /// 跨境贸易电子商务物流企业 0为是，1为否
        /// </summary>
        public int LogisticsFlag { get; set; }

        /// <summary>
        /// 跨境贸易电子商务支付企业 0为是，1为否
        /// </summary>
        public int PayFlag { get; set; }

        /// <summary>
        /// 是否为境外企业 0为是，1为否
        /// </summary>
        public int Abroad { get; set; }

        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 法人代表姓名
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// 法人代表身份证号
        /// </summary>
        public string LegalID { get; set; }

        /// <summary>
        /// 营业执照编号
        /// </summary>
        public string LicenseID { get; set; }

        /// <summary>
        /// 税务登记代码
        /// </summary>
        public string TaxCode { get; set; }

        /// <summary>
        /// 企业联系电话
        /// </summary>
        public string EnterPrisePhone { get; set; }

        /// <summary>
        /// 企业联系人
        /// </summary>
        public string EnterPriseContact { get; set; }

        /// <summary>
        /// 企业地址
        /// </summary>
        public string EnterPriseAddress { get; set; }

        /// <summary>
        /// 电商交易平台名称
        /// </summary>
        public string TransactionName { get; set; }

        /// <summary>
        /// 电商交易平台网址
        /// </summary>
        public string TransactionWebsite { get; set; }

        /// <summary>
        /// 物流信息查询网址
        /// </summary>
        public string LogisticsWebsite { get; set; }

        /// <summary>
        /// 法人代表身份证复印件  0为有，1为无
        /// </summary>
        public int LegalCopyFlag { get; set; }

        /// <summary>
        /// 税务登记复印件  0为有，1为无
        /// </summary>
        public int TaxCopyFlag { get; set; }

        /// <summary>
        /// 《组织机构代码证》副本复印件  0为有，1为无
        /// </summary>
        public int OrgCopyFlag { get; set; }

        /// <summary>
        /// 支付业务批准附件  0为有，1为无
        /// </summary>
        public int PayCopyFlag { get; set; }

        /// <summary>
        /// 营业执照复印件  0为有，1为无
        /// </summary>
        public int LicenseCopyFlag { get; set; }

        /// <summary>
        /// 中华人民共和国报关单位注册登记证书 0为有，1为无
        /// </summary>
        public int UnitsFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 电商企业的交易平台、国内电商企业应提供工信部的批件及备案号
        /// </summary>
        public string RecordID { get; set; }

        /// <summary>
        /// 企业经办人手机号码
        /// </summary>
        public string AttnMobile { get; set; }

        /// <summary>
        /// 企业经办人邮箱
        /// </summary>
        public string AttnEmail { get; set; }

        /// <summary>
        /// 附件表ID
        /// </summary>
        public int AttchID { get; set; }

        /// <summary>
        /// ICP备案证号
        /// </summary>
        public string ICPCode { get; set; }

        /// <summary>
        /// 电信经营许可证
        /// </summary>
        public int OperateFlag { get; set; }
    }
}
