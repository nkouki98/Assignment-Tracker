using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssignmentTracker.Pages.Assignments
{
    public class EditModel : PageModel
    {
        public IndexModel.AssignmentDetails assignmentDetails = new IndexModel.AssignmentDetails();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectToDB = "Server=localhost,1433;Database=Trackerdb;User Id=sa;Password=DB_SQLMac;";
                using (SqlConnection connection = new SqlConnection(connectToDB))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Assignment WHERE assignment_id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                assignmentDetails.id = "" + reader.GetInt32(0);
                                assignmentDetails.title = reader.GetString(1);
                                assignmentDetails.description = reader.GetString(2);
                                assignmentDetails.deadline = reader.GetDateTime(3);
                                assignmentDetails.course = reader.GetString(8);
                                assignmentDetails.instructor_email = reader.GetString(9);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            assignmentDetails.id = Request.Form["id"];
            assignmentDetails.title = Request.Form["title"];
            assignmentDetails.description = Request.Form["description"];

// Convert deadline to DateTime? (nullable DateTime)
            if (DateTime.TryParse(Request.Form["deadline"], out DateTime deadline))
            {
                assignmentDetails.deadline = deadline;
            }
            else
            {
                assignmentDetails.deadline = null; // Set deadline to null if parsing fails
            }

            assignmentDetails.course = Request.Form["course"];
            assignmentDetails.instructor_email = Request.Form["instructor_email"];

            if (string.IsNullOrEmpty(assignmentDetails.id) ||
                string.IsNullOrEmpty(assignmentDetails.title) ||
                string.IsNullOrEmpty(assignmentDetails.description) ||
                assignmentDetails.deadline == null || // Check if deadline is null
                string.IsNullOrEmpty(assignmentDetails.course) ||
                string.IsNullOrEmpty(assignmentDetails.instructor_email))
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                string connectToDB = "Server=localhost,1433;Database=Trackerdb;User Id=sa;Password=DB_SQLMac;";
                using (SqlConnection connection = new SqlConnection(connectToDB))
                {
                    connection.Open();
                    string sql = "UPDATE Assignment " +
                                 "SET title=@title, description=@description, deadline=@deadline, course=@course, instructor_email=@instructor_email " +
                                 "WHERE assignment_id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@title", assignmentDetails.title);
                        command.Parameters.AddWithValue("@description", assignmentDetails.description);
                        command.Parameters.AddWithValue("@deadline", assignmentDetails.deadline);
                        command.Parameters.AddWithValue("@course", assignmentDetails.course);
                        command.Parameters.AddWithValue("@instructor_email", assignmentDetails.instructor_email);
                        command.Parameters.AddWithValue("@id", assignmentDetails.id);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Assignments/Index");
        }
    }
}
