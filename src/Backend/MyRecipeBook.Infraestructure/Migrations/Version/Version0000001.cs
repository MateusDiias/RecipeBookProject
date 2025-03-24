using FluentMigrator;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace MyRecipeBook.Infrastructure.Migrations.Version
{
    [Migration(DatabaseVersions.TABLE_USER, "Create table to sabe the user's information")]
    public class Version0000001 : VersionBase
    {
        public override void Up()
        {
            CreateTable("User")
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("Password").AsString(2000).NotNullable();
        }
    }
}
