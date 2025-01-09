using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class AffirmationDatabase
{
    private SQLiteAsyncConnection _database;

    public AffirmationDatabase()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "affirmations.db");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Affirmation>().Wait();
    }

    public async Task<List<Affirmation>> GetAffirmationsAsync()
    {
        return await _database.Table<Affirmation>().ToListAsync();
    }

    public async Task<int> SaveAffirmationAsync(Affirmation affirmation)
    {
        if (affirmation.Id != 0)
        {
            return await _database.UpdateAsync(affirmation);
        }
        return await _database.InsertAsync(affirmation);
    }

    public async Task<int> DeleteAffirmationAsync(Affirmation affirmation)
    {
        return await _database.DeleteAsync(affirmation);
    }

    public async Task SeedDatabaseAsync()
    {
        var affirmations = new List<Affirmation>
        {
            new Affirmation { Text = "I am capable of achieving great things", CreatedDate = DateTime.Now },
            new Affirmation { Text = "I choose to be confident and self-assured and etc when the house", CreatedDate = DateTime.Now },
            new Affirmation { Text = "I am worthy of love and respect", CreatedDate = DateTime.Now },
            // Add more affirmations as needed
        };

        foreach (var affirmation in affirmations)
        {
            await SaveAffirmationAsync(affirmation);
        }
    }
} 