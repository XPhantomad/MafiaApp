using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace MafiaApp.Daten
{
    public class MafiaItemDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() => { 
       
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags); 
        
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public MafiaItemDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }
        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(PlayerItem).Name))
                {
                    await Database.CreateTableAsync(CreateFlags.None, typeof(PlayerItem)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

    }
}
