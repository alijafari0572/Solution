using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMessages.Events
{
    public class BaseEvents
    {
        public BaseEvents()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
    }
}