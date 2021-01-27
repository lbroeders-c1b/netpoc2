using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CreditOne.Microservices.Tools.PerformanceCounterCategoryTool
{
    /// <summary>
	/// This program will create/delete the FNBM Middle Tier performance category and counters.
	/// </summary>
	class MainClass
    {
        #region Constants

        const string CACHE_PERFORMANCE_COUNTER_CATEGORY = "Cache Performance Counter";
        const string FDR_PERFORMANCE_COUNTER_CATEGORY = "FDR Performance Counter";

        #endregion

        #region Private members

        private static Dictionary<string, string> _categories;

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            _categories = new Dictionary<string, string>();
            _categories.Add("Database Performance Counter for Account", "Performance metrics for Account microservices database");
            _categories.Add("Database Performance Counter for Alert", "Performance metrics for Alert microservices database");
            _categories.Add("Database Performance Counter for Application Menu", "Performance metrics for Application Menu microservices database");
            _categories.Add("Database Performance Counter for Bankruptcy", "Performance metrics for Bankruptcy microservices database");
            _categories.Add("Database Performance Counter for Suppression Group", "Performance metrics for Suppression Group microservices database");
            _categories.Add("Database Performance Counter for Collection Lookup", "Performance metrics for Collection Lookup microservices database");
            _categories.Add("Database Performance Counter for Deceased", "Performance metrics for Deceased microservices database");
            _categories.Add("Database Performance Counter for Employee", "Performance metrics for Employee microservices database");
            _categories.Add("Database Performance Counter for Lookup", "Performance metrics for Lookup microservices database");
            _categories.Add("Database Performance Counter for Non Mon", "Performance metrics for Non Mon microservices database");
            _categories.Add("Database Performance Counter for Note", "Performance metrics for Note microservices database");
            _categories.Add("Database Performance Counter for Payment Rule", "Performance metrics for Payment Rule microservices database");
            _categories.Add("Database Performance Counter for Rewards", "Performance metrics for Rewards microservices database");
            _categories.Add("Database Performance Counter for Session", "Performance metrics for Session microservices database");
            _categories.Add("Database Performance Counter for Work Case", "Performance metrics for Work Case microservices database");
            _categories.Add("Database Performance Counter for Queue", "Performance metrics for queue microservices database");
            _categories.Add("Database Performance Counter for Credit Account Demographic", "Performance metrics for Credit Account Demographic microservices database");
            _categories.Add("Database Performance Counter for Communication", "Performance metrics for Communication microservices database");
            //CreateDatabaseCategory();
            //DeleteCategory();
            if (!ValidateCommandLineArguments(args))
                return 1;

            // We don't need a default case, because we are checking the parameters for validity beforehand.
            switch (args[0])
            {
                case "-h":
                    DisplayUsage("");
                    return 0;
                case "-c":
                    return CreateCategory();
                case "-cdb":
                    return CreateDatabaseCategory();
                case "-ddb":
                    DeleteDBCategories();
                    break;
                case "-d":
                    return DeleteCategory();
            }

            return 0;
        }

        /// <summary>
        /// Creates the category and counters. If they already exist, this will fail.
        /// </summary>
        static int CreateCategory()
        {
            CreateDatabaseCategory();
            CreateCacheCategory();
            CreateFDRCategory();

            return 0;
        }

        private static int CreateDatabaseCategory()
        {
            foreach (var category in _categories)
            {
                try
                {
                    // Create counter data collection
                    CounterCreationDataCollection databaseCounterCollection = new CounterCreationDataCollection();
                    CounterCreationData ccd;

                    // Add the Average Database Connection Open Time counter.
                    ccd = new CounterCreationData();
                    ccd.CounterType = PerformanceCounterType.AverageTimer32;
                    ccd.CounterName = "Average Database Connection Open Time In Milliseconds";
                    ccd.CounterHelp = @"The average amount of time it takes to open a connection to the current" +
                        @"database. This may include connections to different databases.";
                    databaseCounterCollection.Add(ccd);

                    // Add the Average Database Connection Open Time counter base.
                    ccd = new CounterCreationData();
                    ccd.CounterType = PerformanceCounterType.AverageBase;
                    ccd.CounterName = "Average Database Connection Open Time In Milliseconds Base";
                    databaseCounterCollection.Add(ccd);

                    // Add the Average Database ExecuteNonQuery counter.
                    ccd = new CounterCreationData();
                    ccd.CounterType = PerformanceCounterType.AverageTimer32;
                    ccd.CounterName = "Average Database ExecuteNonQuery Time In Milliseconds";
                    ccd.CounterHelp = @"The average amount of time it takes to complete an Database ExecuteNonQuery call.";
                    databaseCounterCollection.Add(ccd);

                    // Add the Average Database ExecuteNonQuery counter base.
                    ccd = new CounterCreationData();
                    ccd.CounterType = PerformanceCounterType.AverageBase;
                    ccd.CounterName = "Average Database ExecuteNonQuery Time In Milliseconds Base";
                    databaseCounterCollection.Add(ccd);

                    // Add the Average Database ExecuteReader counter.
                    ccd = new CounterCreationData();
                    ccd.CounterType = PerformanceCounterType.AverageTimer32;
                    ccd.CounterName = "Average Database ExecuteReader Time In Milliseconds";
                    ccd.CounterHelp = @"The average amount of time it takes to complete an Database ExecuteReader call.";
                    databaseCounterCollection.Add(ccd);

                    // Add the Average Database ExecuteReader counter base.
                    ccd = new CounterCreationData();
                    ccd.CounterType = PerformanceCounterType.AverageBase;
                    ccd.CounterName = "Average Database ExecuteReader Time In Milliseconds Base";
                    databaseCounterCollection.Add(ccd);

                    // Add the Average Database ExecuteScalar counter.
                    ccd = new CounterCreationData();
                    ccd.CounterType = PerformanceCounterType.AverageTimer32;
                    ccd.CounterName = "Average Database ExecuteScalar Time In Milliseconds";
                    ccd.CounterHelp = @"The average amount of time it takes to complete an Database ExecuteScalar call.";
                    databaseCounterCollection.Add(ccd);

                    // Add the Average Database ExecuteScalar counter base.
                    ccd = new CounterCreationData();
                    ccd.CounterType = PerformanceCounterType.AverageBase;
                    ccd.CounterName = "Average Database ExecuteScalar Time In Milliseconds Base";
                    databaseCounterCollection.Add(ccd);

                    // Add the Total Database Open Failures counter.
                    ccd = new CounterCreationData();
                    ccd.CounterType = PerformanceCounterType.NumberOfItems64;
                    ccd.CounterName = "Total Database Open Failures";
                    ccd.CounterHelp = "The total number of times a database connection Open call failed.";
                    databaseCounterCollection.Add(ccd);

                    // Add the Database Operation Time Threshold Exceeded counter.
                    ccd = new CounterCreationData();
                    ccd.CounterType = PerformanceCounterType.NumberOfItems64;
                    ccd.CounterName = "Database Operation Time Threshold Exceeded Count";
                    ccd.CounterHelp = "The total number of times a database operation exceeeded the time specified in the middle tier configuration file.";
                    databaseCounterCollection.Add(ccd);

                    // Create the category.
                    PerformanceCounterCategory.Create(category.Key, category.Value, PerformanceCounterCategoryType.SingleInstance, databaseCounterCollection);
                    Console.WriteLine();
                    Console.WriteLine($"Performance category '{category.Key}' was successfully created.");
                }
                catch (Exception e)
                {
                    Console.WriteLine();
                    Console.WriteLine($"The category '{category.Key}' was not created. See the exception text which follows.\n\n" + e.Message);
                }
            }

            Console.ReadKey();
            return 0;
        }

        private static int CreateCacheCategory()
        {
            try
            {
                // Create counter data collection
                CounterCreationDataCollection cacheCounterCollection = new CounterCreationDataCollection();
                CounterCreationData ccd;

                // Add the Average CachedDataProvider Fetch Time counter.
                ccd = new CounterCreationData();
                ccd.CounterType = PerformanceCounterType.AverageTimer32;
                ccd.CounterName = "Average CachedDataProvider Fetch Time In Milliseconds";
                ccd.CounterHelp = @"The average amount of time it takes to complete a fetch from the CachedData provider.";
                cacheCounterCollection.Add(ccd);

                // Add the Average CachedDataProvider Fetch Time counter base.
                ccd = new CounterCreationData();
                ccd.CounterType = PerformanceCounterType.AverageBase;
                ccd.CounterName = "Average CachedDataProvider Fetch Time In Milliseconds Base";
                cacheCounterCollection.Add(ccd);

                // Add the Total Items in Cached Data Store counter.
                ccd = new CounterCreationData();
                ccd.CounterType = PerformanceCounterType.NumberOfItems64;
                ccd.CounterName = "Total Items In Cached Data Store";
                ccd.CounterHelp = "The total number of items currently in the cached data store.";
                cacheCounterCollection.Add(ccd);

                ccd = new CounterCreationData();
                ccd.CounterType = PerformanceCounterType.NumberOfItems64;
                ccd.CounterName = @"CachedDataProvider Exceptions Count";
                ccd.CounterHelp = "Counts the Exceptions thrown by CachedDataProvider";
                cacheCounterCollection.Add(ccd);

                // Create the category.
                Console.WriteLine();
                PerformanceCounterCategory.Create(CACHE_PERFORMANCE_COUNTER_CATEGORY, "Performance metrics for Cache Provider", PerformanceCounterCategoryType.SingleInstance, cacheCounterCollection);
            }

            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(string.Format("The category '{0}' was not created. See the exception text which follows.\n\n" + e.Message, CACHE_PERFORMANCE_COUNTER_CATEGORY));
                return 1;
            }

            Console.ReadKey();
            return 0;
        }

        private static int CreateFDRCategory()
        {
            try
            {
                // Create counter data collection
                CounterCreationDataCollection fdrCounterCollection = new CounterCreationDataCollection();
                CounterCreationData ccd;

                // Add the Average FDR Query time counter.
                ccd = new CounterCreationData();
                ccd.CounterType = PerformanceCounterType.AverageTimer32;
                ccd.CounterName = "Average FDR Query Time in Milliseconds";
                ccd.CounterHelp = @"The average amount of time FDR queries are taking, in milliseconds." +
                    @"This measures the interval from the time the message is put in the queue," +
                    @"until a valid response is received.";
                fdrCounterCollection.Add(ccd);

                // Add the Average FDR Query time base.
                ccd = new CounterCreationData();
                ccd.CounterType = PerformanceCounterType.AverageBase;
                ccd.CounterName = "Average FDR Query Time in Milliseconds Base";
                fdrCounterCollection.Add(ccd);

                // Add the Total FDR Timeouts
                ccd = new CounterCreationData();
                ccd.CounterType = PerformanceCounterType.NumberOfItems64;
                ccd.CounterName = "Total FDR Timeouts";
                ccd.CounterHelp = "The total number of times an FDR Request Timed Out. These errors can normally be traced to an MQ Series problem.";
                fdrCounterCollection.Add(ccd);

                ccd = new CounterCreationData();
                ccd.CounterType = PerformanceCounterType.NumberOfItems64;
                ccd.CounterName = "Total FDR View Not Supported Errors";
                ccd.CounterHelp = "The total number of ViewNotSupported errors received from FDR.";
                fdrCounterCollection.Add(ccd);

                ccd = new CounterCreationData();
                ccd.CounterType = PerformanceCounterType.NumberOfItems64;
                ccd.CounterName = "Total FDR Required Program Not Located Errors";
                ccd.CounterHelp = "The total number of RequiredProgramNotLocated errors from FDR.";
                fdrCounterCollection.Add(ccd);

                // Create the category.
                Console.WriteLine();
                PerformanceCounterCategory.Create(FDR_PERFORMANCE_COUNTER_CATEGORY, "Performance metrics for FDR Provider", PerformanceCounterCategoryType.SingleInstance, fdrCounterCollection);
            }

            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(string.Format("The category '{0}' was not created. See the exception text which follows.\n\n" + e.Message, FDR_PERFORMANCE_COUNTER_CATEGORY));
                return 1;
            }

            Console.ReadKey();
            return 0;
        }

        /// <summary>
        /// Deletes the category and counters. If the category does not exist, this will fail.
        /// </summary>
        static int DeleteCategory()
        {
            DeleteDBCategories();
            DeleteCacheCategory();
            DeleteFDRCategory();

            Console.ReadKey();
            return 0;
        }

        private static void DeleteFDRCategory()
        {
            try
            {
                PerformanceCounterCategory.Delete(FDR_PERFORMANCE_COUNTER_CATEGORY);
                Console.WriteLine();
                Console.WriteLine($"Performance category '{FDR_PERFORMANCE_COUNTER_CATEGORY}' was successfully deleted.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"The category '{FDR_PERFORMANCE_COUNTER_CATEGORY}' was not deleted because it did not exist, or some other error occurred. See exception text which follows.\n\n" + e.Message);
            }
        }

        private static void DeleteCacheCategory()
        {
            try
            {
                PerformanceCounterCategory.Delete(CACHE_PERFORMANCE_COUNTER_CATEGORY);
                Console.WriteLine();
                Console.WriteLine($"Performance category '{CACHE_PERFORMANCE_COUNTER_CATEGORY}' was successfully deleted.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"The category '{CACHE_PERFORMANCE_COUNTER_CATEGORY}' was not deleted because it did not exist, or some other error occurred. See exception text which follows.\n\n" + e.Message);
            }
        }

        private static void DeleteDBCategories()
        {
            foreach (var category in _categories)
            {
                try
                {
                    PerformanceCounterCategory.Delete(category.Key);
                    Console.WriteLine();
                    Console.WriteLine($"Performance category '{category.Key}' was successfully deleted.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"The category '{category.Key}' was not deleted because it did not exist, or some other error occurred. See exception text which follows.\n\n" + e.Message);
                }
            }
        }

        /// <summary>
        /// Validates the command line arguments. Returns true if they are OK, false if not.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static bool ValidateCommandLineArguments(string[] args)
        {
            // There must be one, and only one, argument.
            if (args.Length != 1)
            {
                DisplayUsage("ERROR: Please specify a single action to be performed.");
                return false;
            }

            // The only parameters we understand are "-c", "-cdb", "-d", "-ddb", or "-h".
            if (args[0] != "-c" && args[0] != "-d" && args[0] != "-h" && args[0] != "-cdb" && args[0] != "-ddb")
            {
                DisplayUsage(string.Format("ERROR: Unrecognized Parameter: '{0}'", args[0]));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Displays the usage message.
        /// </summary>
        /// <param name="errorMessage"></param>
        static void DisplayUsage(string errorMessage)
        {
            Console.WriteLine();
            Console.WriteLine(errorMessage);
            Console.WriteLine();
            Console.WriteLine("\tusage:");
            Console.WriteLine("\t\tPerformanceCounterCategoryTool [ -d ^ -c ^ -h ]");
            Console.WriteLine();
            Console.WriteLine("\t\t-d Deletes all categories and counters on this machine.");
            Console.WriteLine("\t\t-ddb Deletes all database categories and counters on this machine.");
            Console.WriteLine("\t\t-c Creates all categories and counters on this machine.");
            Console.WriteLine("\t\t-cdb Creates all database categories and counters on this machine.");
            Console.WriteLine("\t\t-h Print this help text.");
            Console.WriteLine();
            Console.WriteLine("\t\tOnly one option may be specified at a time. To recreate a category,");
            Console.WriteLine("\t\tfirst run the tool with the -d option, then again with the -c option.");
            Console.WriteLine();
        }
    }
}
