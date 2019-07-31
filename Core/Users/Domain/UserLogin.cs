using Core.Application.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Users.Domain
{
    public class UserLogin : DomainBase
    {
        public UserLogin()
        {
            IsActive = true;
            IsSuspended = false;
        }

        public static int MaxAttempts { get; internal set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsLocked { get; set; }
        public int LoginAttemptCount { get; set; }
        public DateTime? LastLogginDate { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenIssueDateTime { get; set; }
        public string LastPassword { get; set; }
        public DateTime? LastPasswordChanged { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovedBy { get; set; }
        public DateTime ApprovedDateTime { get; set; }

        [ForeignKey("Id")]
        public virtual Person Person { get; set; }
    }
}
