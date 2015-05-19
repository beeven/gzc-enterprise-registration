using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseRegistration.Data.Models
{
    public class RegInfo
    {
        [Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public String Subject { get; set; }
        public String From { get; set; }
        public String Body { get; set; }


        public DateTime ReceivedDate { get; set; }

        public bool IsReplied { get; set; }//0:未回复 1：已回复

        public DateTime? RepliedDate { get; set; }

        public String RepliedBody { get; set; }

        public string RecordNum { get; set; }
        public int isRecordPass { get; set; }//0：未通过审核 1：已通过审核
        public int isGetEntAccess { get; set; }//是否申请过企业备案号 0：未申请 1：已申请
        public DateTime? GetEntAccessDate { get; set; }
        public virtual List<Attachment> Attachments { get; set; }

    }
}
