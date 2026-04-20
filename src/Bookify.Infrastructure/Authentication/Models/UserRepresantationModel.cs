using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Authentication.Models
{
    internal sealed class UserRepresantationModel
    {
        public Dictionary<string, string> Access { get; set; }
        public Dictionary<string, List<string>> Attributes { get; set; }
        public Dictionary<string, string> ClientRoles { get; set; }
        public long? CreatedTimestamp { get; set; }
        public CredentialRepresentationModel[] Credentials { get; set; }
        public string[] DisableableCredentialTypes { get; set; }
        public string Email { get; set; }
        public bool? EmailVerified { get; set; }
        public bool? Enabled { get; set; }
        public string FedereationLink { get; set; }
        public string Id { get; set; }
        public string[] Groups { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? NotBefore { get; set; }
        public string Origin { get; set; }
        public string[] RealmRoles { get; set; }
        public string[] RequiredActions { get; set; }
        public string Self { get; set; }
        public string ServiceAccountClientId { get; set; }
        public string Username { get; set; }

        internal static UserRepresantationModel FromUser(User user)
        {
            var model = new UserRepresantationModel();
            model.FirstName = user.FirstName.Value;
            model.LastName = user.LastName.Value;
            model.Email = user.Email.Value;
            model.Username = user.Email.Value;
            model.Enabled = true;
            model.EmailVerified = true;
            model.CreatedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            model.Attributes = new Dictionary<string, List<string>>();
            model.RequiredActions = Array.Empty<string>();
            return model;
        }
    }
}