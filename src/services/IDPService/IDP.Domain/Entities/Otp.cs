using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Application.DTO
{
    public class Otp
    {
        public int UserId { set; get; }
        public string OtpCode { set; get; }
        public bool IsUse { set; get; }
    }
}