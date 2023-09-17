using Consumer.API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Consumer.API.Data
{
    public class ConsumerDBContext : DbContext
    {
        public ConsumerDBContext(DbContextOptions<ConsumerDBContext> options) : base(options)
        {

        }

        public DbSet<Message> Messages { get; set; }
    }
}
