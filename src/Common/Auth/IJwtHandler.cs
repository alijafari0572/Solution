﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth
{
    public interface IJwtHandler
    {
        JsonWebToken Create(int userId);
    }
}