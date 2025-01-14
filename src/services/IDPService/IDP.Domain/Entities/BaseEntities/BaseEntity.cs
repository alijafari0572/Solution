using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Domain.Entities.BaseEntities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            this.CreateDate = DateTime.UtcNow;
        }

        [Key]
        public int ID { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpDateTime { get; set; }
    }
}