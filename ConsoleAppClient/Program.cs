using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StaffManagement.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace StaffLibClient;

internal class Program
{
    
    static string baseUrl = "http://localhost:5298";

    static async Task Main(string[] args)
    
    {
        while (true)
        {
            Console.WriteLine("Welcome To Student Management System APP!");
            
            /*Console.Write("Please choose one option (or type 'exit' to exit): ");
            string op = Console.ReadLine();
            
            if (op.ToLower() == "exit")
            {
                break;
            }*/
            Option();
            Console.Write("Please choose one option: ");
            string op = Console.ReadLine();

            switch (op)
            {
                    
                case "1":
                    await CreateStaffReportAsync();
                    break;
                case "2":
                    await GetStaffsAsync();
                    break;
                case "3":
                    await GetStaffByIdAsync();
                    break;
                case "4":
                    await UpdateStaffDataAsync();
                    break;
                case "5":
                    await DeleteStaffByIdAsync();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
           

        static void Option()
        {
            Console.WriteLine("""
                              [1]. Create Staff Record
                              [2]. Get all Staffs Records
                              [3]. Get Staff Record by ID
                              [4]. Update Staff Record
                              [5]. Delete Staff Record
                              """);
        }

        static async Task GetStaffsAsync()
            {
                // API endpoint URL
                string apiUrl = "http://localhost:5298/api/v1/staffs";

                // Create HttpClient instance
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Send GET request
                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Read and deserialize the response content
                            string jsonContent = await response.Content.ReadAsStringAsync();
                            var staffs = JsonConvert.DeserializeObject<Staff[]>(jsonContent);

                            // Display the retrieved student data
                            Console.WriteLine("Staff Data:");
                            Console.WriteLine("+" + new string('-', 125) + "+");
                            Console.WriteLine(
                                "| ID | Name            | Phone Number | Address                | Position        |   Department      |  Start Date           |");
                            Console.WriteLine("+" + new string('-', 125) + "+");

                            foreach (var staff in staffs)
                            {
                                Console.WriteLine(
                                    $"| {staff.Id,-2} | {staff.Name,-16} | {staff.PhoneNumber,-10} | {staff.Address,-25} | {staff.Position,-18} | {staff.Department,-15} | {staff.StartDay,-15} |");
                            }

                            Console.WriteLine("+" + new string('-', 125) + "+");
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                    }
                }
            }

        static async Task CreateStaffReportAsync()
        {
            string apiUrl = "http://localhost:5298/api/v1/staffs";
            try
            {
                Console.Write("Enter staff name: ");
                string name = Console.ReadLine();

                Console.Write("Enter phone number: ");
                string phoneNumber = Console.ReadLine();

                Console.Write("Enter address: ");
                string address = Console.ReadLine();

                Console.Write("Enter position: ");
                string position = Console.ReadLine();

                Console.Write("Enter department: ");
                string department = Console.ReadLine();
                
                

                var newStaff = new
                {
                    Name = name,
                    PhoneNumber = phoneNumber,
                    Address = address,
                    Position = position,
                    Department = department,
                };

                var jsonStaff = JsonSerializer.Serialize(newStaff);

                // Log the JSON data before sending the request
                Console.WriteLine("JSON Data:");
                Console.WriteLine(jsonStaff);

                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(jsonStaff, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Staff report created successfully!");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error Response: {errorResponse}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

            static async Task UpdateStaffDataAsync()
            {
                // API endpoint URL
                string apiUrl = "http://localhost:5298/api/v1/staffs";

                // Create HttpClient instance
                using (HttpClient client = new HttpClient())
                {
                    try
                    {

                        HttpResponseMessage getResponse = await client.GetAsync(apiUrl);

                        // Check if the request was successful
                        if (getResponse.IsSuccessStatusCode)
                        {
                            // Read and deserialize the response content
                            string jsonContent = await getResponse.Content.ReadAsStringAsync();
                            var staffs = JsonConvert.DeserializeObject<Staff[]>(jsonContent);

                            // Display the retrieved student data
                            Console.WriteLine("Student Data:");
                            Console.WriteLine("+" + new string('-', 125) + "+");
                            Console.WriteLine(
                                "| ID | Name            | Phone Number | Address                | Position        |   Department      |  Start Date           |");
                            Console.WriteLine("+" + new string('-', 125) + "+");

                            foreach (var staff in staffs)
                            {
                                Console.WriteLine(
                                    $"| {staff.Id,-3} | {staff.Name,-15} | {staff.PhoneNumber,-10} | {staff.Address,-8} | {staff.Position,-18} | {staff.Department,-15} | {staff.StartDay,-15} |");
                            }

                            Console.WriteLine("+" + new string('-', 125) + "+");

                            // Ask the user for the student ID to update
                            Console.Write("Enter the ID of the student you want to update: ");
                            int Id;
                            if (!int.TryParse(Console.ReadLine(), out Id))
                            {
                                Console.WriteLine("Invalid input. Please enter a valid integer.");
                                return;
                            }

                            // Find the selected student by ID
                            var selectedStaff = staffs.FirstOrDefault(s => s.Id == Id);

                            if (selectedStaff == null)
                            {
                                Console.WriteLine($"No student found with ID {Id}.");
                                return;
                            }

                            // Ask the user which fields they want to update
                            Console.WriteLine(
                                $"Selected student: ID: {selectedStaff.Id}, Name: {selectedStaff.Name}");

                            Console.Write("Enter updated Name (press Enter to skip): ");
                            string updatedName = Console.ReadLine();

                            Console.Write("Enter updated Phone Number (press Enter to skip): ");
                            string updatedPhoneNumber = Console.ReadLine();

                            Console.Write("Enter updated Address (press Enter to skip): ");
                            string updatedAddress = Console.ReadLine();

                            Console.Write("Enter updated Start Day (press Enter to skip): ");
                            string updatedStartDay = Console.ReadLine();

                            Console.Write("Enter updated Position (press Enter to skip): ");
                            string updatedPosition = Console.ReadLine();

                            Console.Write("Enter updated Department");
                            string updatedDepartment = Console.ReadLine();

                            // Update only the non-empty fields
                            if (!string.IsNullOrEmpty(updatedName))
                                selectedStaff.Name = updatedName;

                            if (!string.IsNullOrEmpty(updatedPhoneNumber))
                                selectedStaff.PhoneNumber = updatedPhoneNumber;

                            if (!string.IsNullOrEmpty(updatedAddress))
                                selectedStaff.Address = updatedAddress;

                            if (!string.IsNullOrEmpty(updatedStartDay))
                                selectedStaff.StartDay = Convert.ToDateTime(updatedStartDay);

                            if (!string.IsNullOrEmpty(updatedPosition))
                                selectedStaff.Position = updatedPosition;
                            if (!string.IsNullOrEmpty(updatedDepartment))
                                selectedStaff.Department = updatedDepartment;

                            // Convert the updated student data to JSON
                            var jsonUpdatedStaff = JsonConvert.SerializeObject(selectedStaff);

                            // Create HttpContent for the request body
                            var content = new StringContent(jsonUpdatedStaff, Encoding.UTF8, "application/json");

                            // Send PUT request to update the student data
                            HttpResponseMessage putResponse =
                                await client.PutAsync($"{apiUrl}/{selectedStaff.Id}", content);

                            // Check if the request was successful
                            if (putResponse.IsSuccessStatusCode)
                            {
                                Console.WriteLine("Staff data updated successfully!");
                            }
                            else
                            {
                                Console.WriteLine($"Error: {putResponse.StatusCode} - {putResponse.ReasonPhrase}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error: {getResponse.StatusCode} - {getResponse.ReasonPhrase}");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                    }
                }
            }

            static async Task DeleteStaffByIdAsync()
            {
                // API endpoint URL
                string apiUrl = "http://localhost:5298/api/v1/staffs";

                // Create HttpClient instance
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Ask the user for the student ID to delete
                        Console.Write("Enter the ID of the student you want to delete: ");
                        int staffId;
                        if (!int.TryParse(Console.ReadLine(), out staffId))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid integer.");
                            return;
                        }

                        // Send DELETE request to delete the specific student by ID
                        HttpResponseMessage response = await client.DeleteAsync($"{apiUrl}/{staffId}");

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Staff with ID {staffId} deleted successfully!");
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                    }
                }
            }

            static async Task GetStaffByIdAsync()
            {
                // API endpoint URL
                string apiUrl = "http://localhost:5298/api/v1/staffs";

                // Create HttpClient instance
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Ask the user for the student ID to retrieve
                        Console.Write("Enter the ID of the student you want to retrieve: ");
                        int studentId;
                        if (!int.TryParse(Console.ReadLine(), out studentId))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid integer.");
                            return;
                        }

                        // Send GET request to retrieve the specific student by ID
                        HttpResponseMessage response = await client.GetAsync($"{apiUrl}/{studentId}");

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Read and deserialize the response content
                            string jsonContent = await response.Content.ReadAsStringAsync();
                            var staff = JsonConvert.DeserializeObject<Staff>(jsonContent);

                            // Display the retrieved student data
                            Console.WriteLine($"Student Data for ID {studentId}:");
                            Console.WriteLine("+" + new string('-', 125) + "+");
                            Console.WriteLine(
                                "| ID | Name            | Phone Number | Address                | Position        |   Department      |  Start Date           |");
                            Console.WriteLine("+" + new string('-', 125) + "+");

                            Console.WriteLine(
                                $"| {staff.Id,-3} | {staff.Name,-15} | {staff.PhoneNumber,-10} | {staff.Address,-8} | {staff.Position,-18} | {staff.Department,-15} | {staff.StartDay,-15} |");

                            Console.WriteLine("+" + new string('-', 125) + "+");
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                    }
                }
            }

        }
    
    }
}




