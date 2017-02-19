using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyBlog.Infrustructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.ComponentModel.Composition;

namespace MyBlog.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    [Export(typeof(IDbContext))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>,IDbContext
    {
        public IDbSet<Post> Posts { get; set; }
        public IDbSet<PostContent> PostContents { get; set; }
        public IDbSet<PostView> PostViews { get; set; }
        public IDbSet<PostComment> PostComments { get; set; }
        public IDbSet<PostTag> PostTags { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        
        
        public ApplicationDbContext()
            : base("SiteDBConnection", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public override int SaveChanges()
        {
            PostContents.Local.Where(x => x.Post == null).ToList().ForEach(x=>PostContents.Remove(x));
            return base.SaveChanges();
        }
        

    }
}