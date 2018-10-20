using ReposData.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReposServices.Logging
{
    public partial class DefaultLogger : ILogger
    {
        #region Fields

        private readonly IRepository<Log> _logRepository;
        private readonly IDbContext       _dbContext;
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logRepository">Log repository</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="dbContext">DB context</param>
        /// <param name="dataProvider">WeData provider</param>
        /// <param name="commonSettings">Common settings</param>
        public DefaultLogger(IRepository<Log> logRepository,
            IDbContext dbContext)
        {
            this._logRepository = logRepository;
            this._dbContext = dbContext;
            
        }

        #endregion

        #region Utitilities

        /// <summary>
        /// Gets a value indicating whether this message should not be logged
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Result</returns>
        

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether a log level is enabled
        /// </summary>
        /// <param name="level">Log level</param>
        /// <returns>Result</returns>
        public virtual bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">Log item</param>
        public virtual void DeleteLog(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            _logRepository.Delete(log);
        }

        /// <summary>
        /// Deletes a log items
        /// </summary>
        /// <param name="logs">Log items</param>
        public virtual void DeleteLogs(IList<Log> logs)
        {
            if (logs == null)
                throw new ArgumentNullException("logs");

            _logRepository.Delete(logs);
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        public virtual void ClearLog()
        {
                var log = _logRepository.Table.ToList();
                foreach (var logItem in log)
                    _logRepository.Delete(logItem);
        
        }

        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <param name="fromUtc">Log item creation from; null to load all records</param>
        /// <param name="toUtc">Log item creation to; null to load all records</param>
        /// <param name="message">Message</param>
        /// <param name="logLevel">Log level; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Log item items</returns>
  
        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>Log item</returns>
        public virtual Log GetLogById(int logId)
        {
            if (logId == 0)
                return null;

            return _logRepository.GetById(logId);
        }

        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">Log item identifiers</param>
        /// <returns>Log items</returns>
     //public virtual IList<Log> GetLogByIds(int[] logIds)
     //   {
     //       if (logIds == null || logIds.Length == 0)
     //           return new List<Log>();

     //       var query = from l in _logRepository.Table
     //                   where logIds.Contains(l.Id)
     //                   select l;
     //       var logItems = query.ToList();
     //       //sort by passed identifiers
     //       var sortedLogItems = new List<Log>();
     //       foreach (int id in logIds)
     //       {
     //           var log = logItems.Find(x => x.Id == id);
     //           if (log != null)
     //               sortedLogItems.Add(log);
     //       }
     //       return sortedLogItems;
     //   }


        public Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", ServiceInfo serviceInfo = null)
        {
            var log = new Log
            {
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                serviceInfo = serviceInfo,
                CreatedOnUtc = DateTime.UtcNow
            };

            _logRepository.Add(log);

            return log;

        }

        public IList<Log> GetLogByIds(int[] logIds)
        {
            return new List<Log>();

//            throw new NotImplementedException();
        }

        #endregion
    }
}
