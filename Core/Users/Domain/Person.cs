using System;
using Core.Application.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.Users.Domain
{
    public class Person : DomainBase
    {
        public Person()
        {
            IsActive = true;
            IsSuspended = false;
            PersonAddresses = new Collection<PersonAddress>();
        }
        [StringLength(3)]
        public string Salutation { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string Alias { get; set; }
        public string Email { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{DD/MM/YYYY}")]
        public DateTime? DateOfBirth { get; set; }
        public long? GenderId { get; set; }
        [ForeignKey("GenderId")]
        public virtual Lookup Gender { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuspended { get; set; }

        public virtual UserLogin UserLogin { get; set; }
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; }

    }
}
