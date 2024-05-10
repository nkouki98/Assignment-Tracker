using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssignmentTracker.Pages.Assignments
{
    public class IndexModel : PageModel
    {
        public List<AssignmentDetails> ListAssignments = new List<AssignmentDetails>();

        public void OnGet()
        {
            try
            {
                string connectToDB = "Server=localhost,1433;Database=Trackerdb;User Id=sa;Password=DB_SQLMac;";
                using (SqlConnection connection = new SqlConnection(connectToDB))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Assignment";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AssignmentDetails assignmentDetails = new AssignmentDetails();
                                assignmentDetails.id = reader.GetInt32(0).ToString();
                                assignmentDetails.title = reader.GetString(1);
                                assignmentDetails.description = reader.GetString(2);
                                assignmentDetails.deadline = reader.GetDateTime(3); // Corrected this line
                                assignmentDetails.status = reader.GetString(4);
                                assignmentDetails.course = reader.GetString(8);
                                assignmentDetails.instructor_email = reader.GetString(9);

                                ListAssignments.Add(assignmentDetails);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error message.
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        public class AssignmentDetails
        {
            public string id;
            public string title;
            public string description;
            public DateTime? deadline; // Corrected the type here
            public string status;
            public string course;
            public string instructor_email;
        }
    }
}
