/* AsyncPoco is a fork of PetaPoco and is bound by the same licensing terms.
 *
 * PetaPoco - A Tiny ORMish thing for your POCO's.
 * Copyright © 2011-2012 Topten Software.  All Rights Reserved.
 * 
 * Apache License 2.0 - http://www.toptensoftware.com/petapoco/license
 * 
 * Special thanks to Rob Conery (@robconery) for original inspiration (ie:Massive) and for 
 * use of Subsonic's T4 templates, Rob Sullivan (@DataChomp) for hard core DBA advice 
 * and Adam Schroder (@schotime) for lots of suggestions, improvements and Oracle support
 */

// Define PETAPOCO_NO_DYNAMIC in your project settings on .NET 3.5

using System.Threading;
using System.Threading.Tasks;

namespace AsyncPoco
{
	/// <summary>
	/// Threadsafe wrapper for AsyncPoco.Database
	/// </summary>
	public class ThreadSafeDataBase : Database
	{
		readonly SemaphoreSlim _connectionSyncLock = new SemaphoreSlim(1);

		/// <summary>
		/// Construct a Database using a supplied connectionString Name.  The actual connection string and provider will be 
		/// read from app/web.config.
		/// </summary>
		/// <param name="connectionStringName">The name of the connection</param>
		public ThreadSafeDataBase(string connectionStringName)
			: base(connectionStringName)
		{
		}

		/// <summary>
		/// Threadsafe open shared connection
		/// </summary>
		/// <remarks>
		/// base.OpenSharedConnectionAsync() must be vitrtual.
		/// </remarks>
		public override async Task OpenSharedConnectionAsync()
		{
			await _connectionSyncLock.WaitAsync();
 
			await base.OpenSharedConnectionAsync();

			_connectionSyncLock.Release();
		}
	}
}
