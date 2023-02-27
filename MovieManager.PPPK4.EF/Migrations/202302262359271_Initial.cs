namespace MovieManager.PPPK4.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Content = c.Binary(nullable: false),
                        ContentType = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 250),
                        PublishedDate = c.DateTime(nullable: false),
                        DirectorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.DirectorId, cascadeDelete: false)
                .Index(t => t.DirectorId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonAssets",
                c => new
                    {
                        Person_Id = c.Int(nullable: false),
                        Asset_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Person_Id, t.Asset_Id })
                .ForeignKey("dbo.People", t => t.Person_Id, cascadeDelete: false)
                .ForeignKey("dbo.Assets", t => t.Asset_Id, cascadeDelete: false)
                .Index(t => t.Person_Id)
                .Index(t => t.Asset_Id);
            
            CreateTable(
                "dbo.MovieActor",
                c => new
                    {
                        MovieId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MovieId, t.PersonId })
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: false)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: false)
                .Index(t => t.MovieId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.MovieAssets",
                c => new
                    {
                        Movie_Id = c.Int(nullable: false),
                        Asset_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Movie_Id, t.Asset_Id })
                .ForeignKey("dbo.Movies", t => t.Movie_Id, cascadeDelete: false)
                .ForeignKey("dbo.Assets", t => t.Asset_Id, cascadeDelete: false)
                .Index(t => t.Movie_Id)
                .Index(t => t.Asset_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movies", "DirectorId", "dbo.People");
            DropForeignKey("dbo.MovieAssets", "Asset_Id", "dbo.Assets");
            DropForeignKey("dbo.MovieAssets", "Movie_Id", "dbo.Movies");
            DropForeignKey("dbo.MovieActor", "PersonId", "dbo.People");
            DropForeignKey("dbo.MovieActor", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.PersonAssets", "Asset_Id", "dbo.Assets");
            DropForeignKey("dbo.PersonAssets", "Person_Id", "dbo.People");
            DropIndex("dbo.MovieAssets", new[] { "Asset_Id" });
            DropIndex("dbo.MovieAssets", new[] { "Movie_Id" });
            DropIndex("dbo.MovieActor", new[] { "PersonId" });
            DropIndex("dbo.MovieActor", new[] { "MovieId" });
            DropIndex("dbo.PersonAssets", new[] { "Asset_Id" });
            DropIndex("dbo.PersonAssets", new[] { "Person_Id" });
            DropIndex("dbo.Movies", new[] { "DirectorId" });
            DropTable("dbo.MovieAssets");
            DropTable("dbo.MovieActor");
            DropTable("dbo.PersonAssets");
            DropTable("dbo.People");
            DropTable("dbo.Movies");
            DropTable("dbo.Assets");
        }
    }
}
