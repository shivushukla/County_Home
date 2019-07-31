using Infrastructure.ORM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Application.Impl
{
    public abstract class BaseRepository
    {
        protected ApplicationDbContext DbContext { get; set; }
    }
}
