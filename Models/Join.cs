using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityCenter.Models
{
    public class Join
    {
        [Key]
        public int JoinId{get;set;}
        public int UserId{get;set;}
        public int ActivityEventId{get;set;}
        public User AUser{get;set;}
        public ActivityEvent ActivityEvent{get;set;}
    }
}