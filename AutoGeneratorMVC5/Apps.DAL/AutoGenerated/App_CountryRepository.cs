using Apps.Models;
using Apps.Models.App;
using Apps.IDAL.App;
using System;
namespace Apps.DAL.App
{
	public partial class App_CountryRepository:BaseRepository<App_Country>,IApp_CountryRepository,IDisposable
	{
	    public App_CountryRepository(DBContainer db):base(db)
        {
        
        }
	}
}
