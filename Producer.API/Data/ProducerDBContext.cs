using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Producer.API.Models;

namespace Producer.API.Data
{
    public class ProducerDBContext : DbContext
    {
        public ProducerDBContext(DbContextOptions<ProducerDBContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
    }
}
