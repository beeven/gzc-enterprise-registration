using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseRegistration.DataService.Models
{
    /// <summary>
    /// A mail to reply
    /// </summary>
    public class RevertMail
    {
        /// <summary>
        /// Id
        /// </summary>
        public int OID { get; set; }

        /// <summary>
        /// Physical file name
        /// </summary>
        public String FileName { get; set; }

        
        /// <summary>
        /// Number of passed records
        /// </summary>
        public int  RightNum { get; set; }

        /// <summary>
        /// Number of records not passed
        /// </summary>
        public int ErrorNum { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime InputDate { get; set; }

        /// <summary>
        /// Status of the mail
        /// </summary>
        public Status? SendFlag { get; set; }

    }

    public enum Status
    {
        /// <summary>
        /// The mail has not yet been sent.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The mail has been sent.
        /// </summary>
        Sent = 1,

        /// <summary>
        /// Something went wrong when the mail was sending.h
        /// </summary>
        Error = 2
    }
}
