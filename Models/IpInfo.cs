using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_ELearning.Models
{
    public class IpInfo
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public bool IsBlock { get; set; }
        public int StudentId { get; set; }
        public int LimitAccount { get; set; }
    }
}