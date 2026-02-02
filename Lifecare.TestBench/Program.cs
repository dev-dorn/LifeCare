using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace LifeCare.TestBench
{
    public class Program
    {
        private static readonly HttpClient _httpClient = new();
        private static readonly string _baseUrl = "http://localhost:5000/api";
        private static readonly object _lock = new();
        private static int _successCount = 0;
        private static int _failureCount = 0;
        private static readonly List<long> _responseTimes = new();

        static async Task Main(string[] args)
        {
            Console.WriteLine("🏥 HMS TestBench - Hospital Management System Testing");
            Console.WriteLine("======================================================");
            
            try
            {
                // Wait for API to be ready
                await WaitForApi();
                
                // Run tests
                await RunFunctionalTests();
                await RunPerformanceTests();
                await RunConcurrencyTests();
                
                // Generate final report
                GenerateReport();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ TestBench Error: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"   Inner: {ex.InnerException.Message}");
            }
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static async Task WaitForApi()
        {
            Console.WriteLine("🔍 Checking if API is available...");
            int retries = 0;
            while (retries < 10)
            {
                try
                {
                    var response = await _httpClient.GetAsync($"{_baseUrl}/patients");
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("✅ API is ready!");
                        return;
                    }
                }
                catch
                {
                    // Ignore and retry
                }
                
                retries++;
                Console.Write($"⏳ Waiting for API ({retries}/10)...\r");
                await Task.Delay(2000);
            }
            
            throw new Exception("API is not available after 10 retries");
        }

        static async Task RunFunctionalTests()
        {
            Console.WriteLine("\n📋 FUNCTIONAL TESTING");
            Console.WriteLine("====================");
            
            var testCases = GetFunctionalTestCases();
            int testNumber = 1;
            
            foreach (var testCase in testCases)
            {
                Console.WriteLine($"\nTest {testNumber++}: {testCase.Description}");
                Console.WriteLine($"   Data: {JsonSerializer.Serialize(testCase.Data)}");
                
                try
                {
                    var stopwatch = Stopwatch.StartNew();
                    var response = await _httpClient.PostAsJsonAsync(
                        $"{_baseUrl}/patients/register", 
                        testCase.Data);
                    stopwatch.Stop();
                    
                    var content = await response.Content.ReadAsStringAsync();
                    var responseTime = stopwatch.ElapsedMilliseconds;
                    
                    lock (_lock)
                    {
                        _responseTimes.Add(responseTime);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            if (testCase.ShouldSucceed)
                            {
                                _successCount++;
                                Console.WriteLine($"   ✅ PASS - Expected success, got {response.StatusCode} ({responseTime}ms)");
                            }
                            else
                            {
                                _failureCount++;
                                Console.WriteLine($"   ❌ FAIL - Expected failure, got success ({responseTime}ms)");
                            }
                        }
                        else
                        {
                            if (!testCase.ShouldSucceed)
                            {
                                _successCount++;
                                Console.WriteLine($"   ✅ PASS - Expected failure, got {response.StatusCode} ({responseTime}ms)");
                            }
                            else
                            {
                                _failureCount++;
                                Console.WriteLine($"   ❌ FAIL - Expected success, got {response.StatusCode} ({responseTime}ms)");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ❌ ERROR - {ex.Message}");
                    _failureCount++;
                }
            }
        }

        static async Task RunPerformanceTests()
        {
            Console.WriteLine("\n⚡ PERFORMANCE TESTING");
            Console.WriteLine("=====================");
            
            var testPatients = GetPerformanceTestData();
            var stopwatch = new Stopwatch();
            var results = new List<TestResult>();
            
            Console.WriteLine($"Testing {testPatients.Count} patient registrations...");
            
            foreach (var patient in testPatients)
            {
                stopwatch.Restart();
                
                try
                {
                    var response = await _httpClient.PostAsJsonAsync(
                        $"{_baseUrl}/patients/register", 
                        patient);
                    
                    stopwatch.Stop();
                    var responseTime = stopwatch.ElapsedMilliseconds;
                    
                    results.Add(new TestResult
                    {
                        Success = response.IsSuccessStatusCode,
                        ResponseTime = responseTime,
                        StatusCode = response.StatusCode
                    });
                    
                    Console.Write($"   Processed {results.Count}/{testPatients.Count}...\r");
                }
                catch (Exception)
                {
                    stopwatch.Stop();
                    results.Add(new TestResult
                    {
                        Success = false,
                        ResponseTime = stopwatch.ElapsedMilliseconds
                    });
                }
            }
            
            DisplayPerformanceResults(results);
        }

        static async Task RunConcurrencyTests()
        {
            Console.WriteLine("\n🔀 CONCURRENCY TESTING");
            Console.WriteLine("=====================");
            
            var testCases = GetConcurrencyTestData();
            int concurrentUsers = 10;
            int requestsPerUser = 5;
            
            Console.WriteLine($"Testing {concurrentUsers} concurrent users, {requestsPerUser} requests each...");
            
            var stopwatch = Stopwatch.StartNew();
            var tasks = new List<Task>();
            
            for (int i = 0; i < concurrentUsers; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    for (int j = 0; j < requestsPerUser; j++)
                    {
                        var patient = testCases[(i * requestsPerUser + j) % testCases.Count];
                        await MakeRequestWithTiming(patient);
                    }
                }));
            }
            
            await Task.WhenAll(tasks);
            stopwatch.Stop();
            
            Console.WriteLine($"   Total time: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"   Total requests: {concurrentUsers * requestsPerUser}");
        }

        static async Task MakeRequestWithTiming(object patient)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"{_baseUrl}/patients/register", 
                    patient);
                stopwatch.Stop();
                
                lock (_lock)
                {
                    _responseTimes.Add(stopwatch.ElapsedMilliseconds);
                    if (response.IsSuccessStatusCode)
                        _successCount++;
                    else
                        _failureCount++;
                }
            }
            catch
            {
                stopwatch.Stop();
                lock (_lock)
                {
                    _responseTimes.Add(stopwatch.ElapsedMilliseconds);
                    _failureCount++;
                }
            }
        }

        static void DisplayPerformanceResults(List<TestResult> results)
        {
            var successful = results.Where(r => r.Success).ToList();
            var failed = results.Where(r => !r.Success).ToList();
            
            Console.WriteLine("\n📊 Performance Results:");
            Console.WriteLine($"   Total requests: {results.Count}");
            Console.WriteLine($"   Successful: {successful.Count}");
            Console.WriteLine($"   Failed: {failed.Count}");
            Console.WriteLine($"   Success rate: {(double)successful.Count / results.Count * 100:F2}%");
            
            if (successful.Any())
            {
                var times = successful.Select(r => r.ResponseTime).ToList();
                Console.WriteLine($"   Average response time: {times.Average():F2}ms");
                Console.WriteLine($"   Min response time: {times.Min()}ms");
                Console.WriteLine($"   Max response time: {times.Max()}ms");
                Console.WriteLine($"   Throughput: {(double)successful.Count / (times.Sum() / 1000.0):F2} req/sec");
                
                // Calculate percentiles
                times.Sort();
                Console.WriteLine($"   50th percentile (median): {GetPercentile(times, 50)}ms");
                Console.WriteLine($"   90th percentile: {GetPercentile(times, 90)}ms");
                Console.WriteLine($"   95th percentile: {GetPercentile(times, 95)}ms");
                Console.WriteLine($"   99th percentile: {GetPercentile(times, 99)}ms");
            }
        }

        static double GetPercentile(List<long> values, double percentile)
        {
            if (!values.Any()) return 0;
            var index = (int)Math.Ceiling(percentile / 100.0 * values.Count) - 1;
            return values[Math.Max(0, Math.Min(index, values.Count - 1))];
        }

        static void GenerateReport()
        {
            Console.WriteLine("\n📈 FINAL TEST REPORT");
            Console.WriteLine("===================");
            
            var totalTests = _successCount + _failureCount;
            Console.WriteLine($"Total tests executed: {totalTests}");
            Console.WriteLine($"Passed: {_successCount} ({(_successCount / (double)totalTests * 100):F2}%)");
            Console.WriteLine($"Failed: {_failureCount} ({(_failureCount / (double)totalTests * 100):F2}%)");
            
            if (_responseTimes.Any())
            {
                Console.WriteLine("\n⏱️  Response Time Statistics:");
                Console.WriteLine($"   Average: {_responseTimes.Average():F2}ms");
                Console.WriteLine($"   Minimum: {_responseTimes.Min()}ms");
                Console.WriteLine($"   Maximum: {_responseTimes.Max()}ms");
                Console.WriteLine($"   Standard Deviation: {CalculateStandardDeviation(_responseTimes):F2}ms");
                
                // Response time distribution
                var grouped = _responseTimes
                    .GroupBy(t => (int)(t / 100)) // Group by 100ms intervals
                    .OrderBy(g => g.Key)
                    .Select(g => new { Range = $"{g.Key * 100}-{(g.Key + 1) * 100}ms", Count = g.Count() })
                    .ToList();
                
                Console.WriteLine("\n📊 Response Time Distribution:");
                foreach (var group in grouped.Take(10)) // Show top 10 ranges
                {
                    Console.WriteLine($"   {group.Range}: {group.Count} requests");
                }
            }
            
            Console.WriteLine("\n" + (_failureCount == 0 ? "🎉 ALL TESTS PASSED!" : "⚠️  Some tests failed."));
        }

        static double CalculateStandardDeviation(List<long> values)
        {
            if (!values.Any()) return 0;
            var avg = values.Average();
            var sum = values.Sum(v => Math.Pow(v - avg, 2));
            return Math.Sqrt(sum / values.Count);
        }

        static List<TestCase> GetFunctionalTestCases()
        {
            return new List<TestCase>
            {
                // Valid cases
                new TestCase
                {
                    Description = "Valid adult patient with complete information",
                    ShouldSucceed = true,
                    Data = new
                    {
                        NationalId = "TEST-001",
                        FirstName = "John",
                        LastName = "Smith",
                        DateOfBirth = new DateTime(1980, 5, 15),
                        Gender = "Male",
                        PhoneNumber = "+1-555-1234",
                        Email = "john.smith@example.com",
                        Street = "123 Main St",
                        City = "Anytown",
                        State = "CA",
                        ZipCode = "12345"
                    }
                },
                new TestCase
                {
                    Description = "Valid child patient with guardian",
                    ShouldSucceed = true,
                    Data = new
                    {
                        NationalId = "TEST-002",
                        FirstName = "Emma",
                        LastName = "Johnson",
                        DateOfBirth = new DateTime(2018, 3, 10),
                        Gender = "Female",
                        PhoneNumber = "+1-555-5678",
                        Email = "emma.parent@example.com",
                        Street = "456 Oak St",
                        City = "Sometown",
                        State = "TX",
                        ZipCode = "67890",
                        Guardian = new
                        {
                            FirstName = "Sarah",
                            LastName = "Johnson",
                            Relationship = "Mother",
                            PhoneNumber = "+1-555-9012"
                        }
                    }
                },
                new TestCase
                {
                    Description = "Valid patient with minimal information",
                    ShouldSucceed = true,
                    Data = new
                    {
                        NationalId = "TEST-003",
                        FirstName = "Jane",
                        LastName = "Doe",
                        DateOfBirth = new DateTime(1990, 8, 22),
                        Gender = "Female",
                        PhoneNumber = "+1-555-3456"
                    }
                },

                // Invalid cases (should fail)
                new TestCase
                {
                    Description = "Missing National ID",
                    ShouldSucceed = false,
                    Data = new
                    {
                        FirstName = "Invalid",
                        LastName = "Patient",
                        DateOfBirth = new DateTime(1990, 1, 1),
                        Gender = "Male",
                        PhoneNumber = "+1-555-0000"
                    }
                },
                new TestCase
                {
                    Description = "Missing First Name",
                    ShouldSucceed = false,
                    Data = new
                    {
                        NationalId = "TEST-004",
                        LastName = "Patient",
                        DateOfBirth = new DateTime(1990, 1, 1),
                        Gender = "Male",
                        PhoneNumber = "+1-555-0000"
                    }
                },
                new TestCase
                {
                    Description = "Invalid date of birth (future)",
                    ShouldSucceed = false,
                    Data = new
                    {
                        NationalId = "TEST-005",
                        FirstName = "Future",
                        LastName = "Patient",
                        DateOfBirth = DateTime.Now.AddYears(1),
                        Gender = "Male",
                        PhoneNumber = "+1-555-0000"
                    }
                },
                new TestCase
                {
                    Description = "Invalid gender",
                    ShouldSucceed = false,
                    Data = new
                    {
                        NationalId = "TEST-006",
                        FirstName = "Invalid",
                        LastName = "Gender",
                        DateOfBirth = new DateTime(1990, 1, 1),
                        Gender = "InvalidGender",
                        PhoneNumber = "+1-555-0000"
                    }
                },
                new TestCase
                {
                    Description = "Invalid email format",
                    ShouldSucceed = false,
                    Data = new
                    {
                        NationalId = "TEST-007",
                        FirstName = "Invalid",
                        LastName = "Email",
                        DateOfBirth = new DateTime(1990, 1, 1),
                        Gender = "Male",
                        PhoneNumber = "+1-555-0000",
                        Email = "invalid-email"
                    }
                },
                new TestCase
                {
                    Description = "Duplicate National ID",
                    ShouldSucceed = false,
                    Data = new
                    {
                        NationalId = "TEST-001", // Duplicate of first test
                        FirstName = "Duplicate",
                        LastName = "Patient",
                        DateOfBirth = new DateTime(1995, 6, 15),
                        Gender = "Female",
                        PhoneNumber = "+1-555-9999"
                    }
                }
            };
        }

        static List<object> GetPerformanceTestData()
        {
            var patients = new List<object>();
            var random = new Random();
            
            for (int i = 1; i <= 50; i++) // 50 test patients for performance testing
            {
                patients.Add(new
                {
                    NationalId = $"PERF-{i:000}",
                    FirstName = $"Performance{i}",
                    LastName = $"Test{i}",
                    DateOfBirth = new DateTime(1950 + random.Next(50), random.Next(1, 12), random.Next(1, 28)),
                    Gender = random.Next(2) == 0 ? "Male" : "Female",
                    PhoneNumber = $"+1-555-{random.Next(1000, 9999)}",
                    Email = $"performance{i}@test.com",
                    Street = $"{random.Next(1000, 9999)} Test St",
                    City = "Test City",
                    State = "TC",
                    ZipCode = random.Next(10000, 99999).ToString()
                });
            }
            
            return patients;
        }

        static List<object> GetConcurrencyTestData()
        {
            var patients = new List<object>();
            var random = new Random();
            
            for (int i = 1; i <= 100; i++) // 100 unique patients for concurrency testing
            {
                patients.Add(new
                {
                    NationalId = $"CONC-{i:000}",
                    FirstName = $"Concurrent{i}",
                    LastName = $"User{i}",
                    DateOfBirth = new DateTime(1960 + random.Next(40), random.Next(1, 12), random.Next(1, 28)),
                    Gender = random.Next(2) == 0 ? "Male" : "Female",
                    PhoneNumber = $"+1-555-{random.Next(1000, 9999)}",
                    Email = $"concurrent{i}@test.com",
                    Street = $"{random.Next(1000, 9999)} Concurrent St",
                    City = "Concurrent City",
                    State = "CC",
                    ZipCode = random.Next(10000, 99999).ToString()
                });
            }
            
            return patients;
        }

        class TestCase
        {
            public string Description { get; set; }
            public bool ShouldSucceed { get; set; }
            public object Data { get; set; }
        }

        class TestResult
        {
            public bool Success { get; set; }
            public long ResponseTime { get; set; }
            public System.Net.HttpStatusCode? StatusCode { get; set; }
        }
    }
}