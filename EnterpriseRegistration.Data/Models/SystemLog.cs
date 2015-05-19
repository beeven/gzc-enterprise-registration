using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseRegistration.Data.Models
{
    public class SystemLog
    {
        [Key]
        public Guid Id { get; set; }

        public String MailUser { get; set; }

        public String LogContext { get; set; }

        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 类型值代表含义,0：正常通过日志，1：Excel错误日志，2:错误异常日志，3:邮件回复日志,4:企业备案申请办理
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 操作数据的ID号
        /// </summary>
        public int? RecordID { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public String Operator { get; set; }
    }
}
