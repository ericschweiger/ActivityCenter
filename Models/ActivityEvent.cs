using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityCenter.Models
{
    public class ActivityEvent
    {
        [Key]
        public int ActvityEventId {get; set;}
        [Required(ErrorMessage="A Name is required")]
        public string ActivityName {get; set;}
        [Required(ErrorMessage="An Activity start time is required")]
        [NoPast(ErrorMessage="You can't create an activity in the past!")]
        [DataType(DataType.DateTime)]
        public DateTime ActivityStart {get; set;}
        [Required(ErrorMessage="An Activity duration is required")]
        [DataType(DataType.Duration)]
        public int ActivityDuration {get; set;}
        [Required(ErrorMessage="An Activity description is required")]
        public string ActivityDescription {get; set;}
        public User Coordinator {get; set;}
        public int UserID {get; set;}
        public List<Join> Attendees {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
    }
}