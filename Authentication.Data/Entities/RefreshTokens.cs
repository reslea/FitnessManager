using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Data;

namespace Authentication.Data.Entities
{
    public class RefreshToken : BaseEntity
    {
        public User User { get; set; }

        public int UserId { get; set; }

        public Guid Refresh { get; set; }
    }
}
