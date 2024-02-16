using System.Data;
using System.Data.SqlClient;

namespace EMedicineBE.Models
{
    public class DAL
    {
        public Response register(Users users, SqlConnection connection)
        {
            Response response = new Response();
            //SqlCommand cmd = new SqlCommand("sp_register", connection);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
            //cmd.Parameters.AddWithValue("@LastName", users.LastName);
            //cmd.Parameters.AddWithValue("@Passowrd", users.Password);
            //cmd.Parameters.AddWithValue("@Email", users.Email);
            //cmd.Parameters.AddWithValue("@Fund", 0);
            //cmd.Parameters.AddWithValue("@Type", "Users");
            //cmd.Parameters.AddWithValue("@Status", "Pending");
            //cmd.Parameters.AddWithValue("@CreateOn",users.CreateOn);
            //connection.Open();
            //int i = cmd.ExecuteNonQuery();
            //connection.Close();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Users (FirstName, LastName, Email, Passowrd, Fund, Type, Status, CreateOn) " +
                    "VALUES (@FirstName, @LastName, @Email, @Passowrd, @Fund, @Type, @Status, @CreateOn)", connection);
                cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = users.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = users.LastName;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 255).Value = users.Email;
                cmd.Parameters.Add("@Passowrd", SqlDbType.VarChar, 100).Value = users.Password;
                cmd.Parameters.Add("@Fund", SqlDbType.Decimal, 50).Value = 0;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar, 50).Value = "User";
                cmd.Parameters.Add("@Status", SqlDbType.VarChar, 255).Value ="Pending";
                cmd.Parameters.Add("@CreateOn", SqlDbType.DateTime, 100).Value = users.CreateOn;

                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    response.statusCode = 200;
                    response.StatusMessage = "User registered successfully";
                }
                else
                {
                    response.statusCode = 100;
                    response.StatusMessage = "User registration failed";

                }
            }
            catch (Exception ex)
            {
                response.statusCode = 100;
                response.StatusMessage= ex.Message;
            }
            finally
            {
                connection.Close();
            }
            
            return response;
        }
        public Response login(Users users, SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("sp_login", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@Email", users.Email);
            da.SelectCommand.Parameters.AddWithValue("@Passowrd", users.Password);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            Users user = new Users();
            if (dt.Rows.Count > 0)
            {
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Type = Convert.ToString(dt.Rows[0]["Type"]);
                response.statusCode = 200;
                response.StatusMessage = "User is valid";
                response.user = user;
            }
            else
            {
                response.statusCode = 100;
                response.StatusMessage = "User is invalid";
                response.user = null;
            }
            return response;
        }
        public Response viewUser(Users users, SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("p_viewUser", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@ID", users.ID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            Users user = new Users();
            if (dt.Rows.Count > 0)
            {
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Fund = Convert.ToDecimal(dt.Rows[0]["Fund"]);
                user.Type = Convert.ToString(dt.Rows[0]["Type"]);
                user.CreateOn = Convert.ToDateTime(dt.Rows[0]["CreateOn"]);
                user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                response.statusCode = 200;
                response.StatusMessage = "User Exists";

            }
            else
            {
                response.statusCode = 100;
                response.StatusMessage = "User does not exist";
                response.user = user;
            }
            return response;
        }
        public Response updateProfile(Users users, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_updateProfile", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
            cmd.Parameters.AddWithValue("@LastName", users.LastName);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            cmd.Parameters.AddWithValue("@Passowrd", users.Password);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.statusCode = 200;
                response.StatusMessage = "Record update successfully";
            }
            else
            {
                response.statusCode = 100;
                response.StatusMessage = "Some Error occurred. Try after sometime";

            }
            return response;

        }
        public Response addToCart(Cart cart, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_AddToCart", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", cart.UserId);
            cmd.Parameters.AddWithValue("@MedicineID", cart.MedicineID);
            cmd.Parameters.AddWithValue("@UnitPrice", cart.UnitPrice);
            cmd.Parameters.AddWithValue("@Discount", cart.Discount);
            cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
            cmd.Parameters.AddWithValue("@TotalPrice", cart.TotalPrice);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.statusCode = 200;
                response.StatusMessage = "Item added successfully";
            }
            else
            {
                response.statusCode = 100;
                response.StatusMessage = "item could not added";

            }
            return response;
        }
        public Response placeOrder(Users users, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_PlaceOrder", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", users.ID);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.statusCode = 200;
                response.StatusMessage = "Order has been placed successfully";
            }
            else
            {
                response.statusCode = 100;
                response.StatusMessage = "Order could not be placed";

            }
            return response;
        }
        public Response orderList(Users users, SqlConnection connection)
        {
            Response response = new Response();
            List<Orders> listOrder = new List<Orders>();
            SqlDataAdapter da = new SqlDataAdapter("sp_OrderList", connection);
            da.SelectCommand.CommandType= CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@Type", users.Type);
            da.SelectCommand.Parameters.AddWithValue("@ID", users.ID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    Orders order = new Orders();
                    order.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    order.OrderNo = Convert.ToString(dt.Rows[i]["OrderNo"]);
                    order.OrderTotal = Convert.ToDecimal(dt.Rows[i]["OrderTotal"]);
                    order.OrderStatus = Convert.ToString(dt.Rows[i]["OrderStatus"]);
                    listOrder.Add(order);
                }
                if(listOrder.Count > 0)
                {
                    response.statusCode = 200;
                    response.StatusMessage = "Order details fetched";
                    response.listOrders = listOrder;
                }
                else
                {
                    response.statusCode = 100;
                    response.StatusMessage = "Order details are not available";
                    response.listOrders = null;
                }
            }
            else
            {
                response.statusCode = 100;
                response.StatusMessage = "Order details are not available";
                response.listOrders = null;
            }

            return response;
        }
        public Response addUpdateMedicine(Medicines medicines, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_AddUpdateMedicine", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", medicines.Name);
            cmd.Parameters.AddWithValue("@Manufacturer", medicines.Manufacturer);
            cmd.Parameters.AddWithValue("@UnitPrice", medicines.UnitPrice);
            cmd.Parameters.AddWithValue("@Discount", medicines.Discount);
            cmd.Parameters.AddWithValue("@Quantity", medicines.Quantity);
            cmd.Parameters.AddWithValue("@ExpDate", medicines.ExpDate);
            cmd.Parameters.AddWithValue("@ImageURL", medicines.ImageURL);
            cmd.Parameters.AddWithValue("@Status", medicines.Status);
            cmd.Parameters.AddWithValue("@Type", medicines.Type);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.statusCode = 200;
                response.StatusMessage = "Medicine inserted successfully";
            }
            else
            {
                response.statusCode = 100;
                response.StatusMessage = "Medicine did not save. Try again.";

            }
            return response;
        }
        public Response userList(SqlConnection connection)
        {
            Response response = new Response();
            List<Users> listUsers = new List<Users>();
            SqlDataAdapter da = new SqlDataAdapter("sp_UserList", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Users user = new Users();
                    user.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    user.FirstName = Convert.ToString(dt.Rows[i]["FirstName"]);
                    user.LastName = Convert.ToString(dt.Rows[i]["LastName"]);
                    user.Password = Convert.ToString(dt.Rows[i]["Password"]);
                    user.Email = Convert.ToString(dt.Rows[i]["Email"]);
                    user.Fund = Convert.ToDecimal(dt.Rows[i]["Fund"]);
                    user.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    user.Status = Convert.ToString(dt.Rows[i]["Status"]);
                    user.CreateOn = Convert.ToDateTime(dt.Rows[i]["CreateOn"]);
                    listUsers.Add(user);
                }
                if (listUsers.Count > 0)
                {
                    response.statusCode = 200;
                    response.StatusMessage = "Users details fetched";
                    response.listUsers = listUsers;
                }
                else
                {
                    response.statusCode = 100;
                    response.StatusMessage = "Users details are not available";
                    response.listUsers = null;
                }
            }
            else
            {
                response.statusCode = 100;
                response.StatusMessage = "Users details are not available";
                response.listUsers = null;
            }

            return response;
        }
    }
}
