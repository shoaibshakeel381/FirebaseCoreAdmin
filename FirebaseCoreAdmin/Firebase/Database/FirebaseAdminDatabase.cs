namespace FirebaseCoreAdmin.Firebase.Database
{
    using System;
    using FirebaseCoreAdmin.Configurations;
    using FirebaseCoreAdmin.Configurations.ServiceAccounts;
    using FirebaseCoreAdmin.Firebase.Auth;
    using FirebaseCoreAdmin.HttpClients;

    public class FirebaseAdminDatabase : IFirebaseAdminDatabase, IDisposable
    {
        private IFirebaseHttpClient _httpClient;

        public FirebaseAdminDatabase(IFirebaseAdminAuth auth, IServiceAccountCredentials credentials, IFirebaseConfiguration configuration)
        {
            IFirebaseConfiguration firebaseConfiguration = new DefaultFirebaseConfiguration(GoogleServiceAccess.DatabaseOnly);
            if (configuration != null)
            {
                firebaseConfiguration = configuration;
            }

            var firebaseAuthority = new Uri($"https://{credentials.GetProjectId()}.{firebaseConfiguration.FirebaseHost}/", UriKind.Absolute);
            _httpClient = new FirebaseHttpClient(credentials, firebaseConfiguration, firebaseAuthority);

            auth.AddFirebaseHttpClient(_httpClient);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IFirebaseAdminRef Ref(string path)
        {
            return new FirebaseAdminRef(_httpClient, path);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient.Dispose();
            }
        }

        ~FirebaseAdminDatabase()
        {
            Dispose(false);
        }


    }
}
