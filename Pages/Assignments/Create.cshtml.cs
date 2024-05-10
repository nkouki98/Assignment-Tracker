using System.Data.SqlClient;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;


namespace AssignmentTracker.Pages.Assignments;
public class CreateAssignment : PageModel
{
    public IndexModel.AssignmentDetails assignmentDetails = new IndexModel.AssignmentDetails();
    public String errorMessage = "";
    public String successMessage = "";
    public void OnGet()
    {
        
    }

    public void OnPost()
    {
 
            // Update assignmentDetails object with form data for each attribute
        assignmentDetails.title = Request.Form["title"];
        assignmentDetails.description = Request.Form["description"];
        assignmentDetails.deadline = Convert.ToDateTime(Request.Form["deadline"]);
        assignmentDetails.course = Request.Form["course"];
        assignmentDetails.instructor_email = Request.Form["instructor_email"];

        if (assignmentDetails.title.Length == 0 || assignmentDetails.description.Length == 0 ||
            assignmentDetails.deadline.Equals(null) || assignmentDetails.course.Length == 0 ||
            assignmentDetails.instructor_email.Length == 0)
        {
            errorMessage = "Fields are required.";
            return;
        }   
        
        // Save to DB
        try
        {
            string connectToDB = "Server=localhost,1433;Database=Trackerdb;User Id=sa;Password=DB_SQLMac;";
            using (SqlConnection connection = new SqlConnection(connectToDB))
            {
                connection.Open();
                string insertSql = "INSERT INTO Assignment " +
                                   "(title, description, deadline, course, instructor_email) VALUES " +
                                   "(@title, @description, @deadline, @course, @instructor_email)";
                using (SqlCommand command = new SqlCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@title", assignmentDetails.title);
                    command.Parameters.AddWithValue("@description", assignmentDetails.description);
                    command.Parameters.AddWithValue("@deadline", assignmentDetails.deadline);
                    command.Parameters.AddWithValue("@course", assignmentDetails.course);
                    command.Parameters.AddWithValue("@instructor_email", assignmentDetails.instructor_email);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            return;
        }
        assignmentDetails.title = string.Empty; // Or null if the property is nullable
        assignmentDetails.description = string.Empty; // Or null if the property is nullable
        assignmentDetails.deadline = DateTime.Today; // Or null if the property is nullable
        assignmentDetails.course = string.Empty; // Or null if the property is nullable
        assignmentDetails.instructor_email = string.Empty; // Or null if the property is nullable

        
        successMessage = "New Assignment added successfully";
        
        Response.Redirect("/Assignments/Index");

    }
}