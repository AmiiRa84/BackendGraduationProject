using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using GraduationProject.Services;
using GraduationProject.Services.Abstraction;

namespace GraduationProject.API.Extensions
{
    public static class FirebaseServiceExtensions
    {
        public static async Task<IServiceCollection> AddFirebaseServices(this IServiceCollection services)
        {
            using var stream = new FileStream("FirebaseConfig/serviceAccountKey.json", FileMode.Open, FileAccess.Read);

            var credential = (await CredentialFactory
                .FromStreamAsync<ServiceAccountCredential>(stream, CancellationToken.None))
                .ToGoogleCredential();

            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential
            });

            services.AddScoped<INotificationService, NotificationService>();
            return services;
        }

    }
}
