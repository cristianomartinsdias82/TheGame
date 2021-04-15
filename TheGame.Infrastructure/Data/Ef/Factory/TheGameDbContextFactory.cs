using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Linq;
using TheGame.Data.Ef;

namespace TheGame.Infrastructure.Data.Ef.Factory
{
    internal class TheGameDbContextFactory : IDesignTimeDbContextFactory<TheGameDbContext>
    {
        public TheGameDbContext CreateDbContext(string[] args)
        {
            var connectionStringArg = args.FirstOrDefault(arg => arg.ToUpperInvariant().Contains("CONNECTIONSTRING"));
            if (connectionStringArg == null)
                throw new InvalidOperationException("In order to perform migrations, a valid connection string must be informed via --connectionstring command line argument.");

            var connectionString = connectionStringArg.ToUpperInvariant().Substring("--CONNECTIONSTRING=".Length);

            var optionsBuilder = new DbContextOptionsBuilder<TheGameDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new TheGameDbContext(optionsBuilder.Options);
        }
    }
}