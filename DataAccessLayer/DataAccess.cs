using FoodAppWebApi.Models;
using System.Data.SqlClient;

namespace FoodAppWebApi.DataAccessLayer
{
    public class DataAccess
    {

        public virtual List<Restaurants> GetAllRestaurants()
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997; ");
            SqlCommand cmd = new SqlCommand("select * from Restaurants", conn);
            conn.Open();
            SqlDataReader sr = cmd.ExecuteReader();
            List<Restaurants> res = new List<Restaurants>();
            while (sr.Read())
            {
                Restaurants restaurant = new Restaurants((int)sr["Restaurant_Id"], sr["Restaurant_Name"].ToString(), sr["Restaurant_Image"].ToString());
                res.Add(restaurant);
            }
            return res;
        }

        public virtual List<Restaurants> GetVegRestaurants()
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997; ");
            SqlCommand cmd = new SqlCommand("select * from Restaurants where Restaurant_Id in (SELECT distinct Restaurant_Id FROM Food where Restaurant_Id NOT IN(select Restaurant_Id from Food where FoodType = 'Non Veg' ))", conn);
            conn.Open();
            SqlDataReader sr = cmd.ExecuteReader();
            List<Restaurants> res = new List<Restaurants>();
            while (sr.Read())
            {
                Restaurants restaurant = new Restaurants((int)sr["Restaurant_Id"], sr["Restaurant_Name"].ToString(), sr["Restaurant_Image"].ToString());
                res.Add(restaurant);
            }
            return res;
        }

        public virtual int AddItemsToCart(CartItems cart)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997;");
            SqlCommand cmd = new SqlCommand(String.Format("insert into AddItemToCart values('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                          cart.UserName,
                                                          cart.FoodItem,
                                                          cart.Quantity,
                                                          cart.RestaurantId,
                                                          cart.Price,
                                                          cart.FoodId), conn);
            conn.Open();
            int res = cmd.ExecuteNonQuery();
            conn.Close();
            return res;
        }

        public virtual List<Menu> GetMenuById(int Id)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997;");
            SqlCommand cmd = new SqlCommand(String.Format("select *  from Food where Restaurant_Id={0}", Id), conn);
            conn.Open();
            SqlDataReader sr = cmd.ExecuteReader();

            var model = new List<Menu>();

            while (sr.Read())
            {
                int id = (int)sr["Id"];
                Menu menu = new Menu(
                    4,
                    sr["Food_Image"].ToString(),
                    sr["Food_Item"].ToString(),
                    (int)sr["Price"],
                    (int)sr["Restaurant_Id"]);
                Console.WriteLine(id);
                menu.Id = id;
                model.Add(menu);
            }

            return model;
        }



        public virtual List<Restaurants> GetRestaurantByCuisine(string cuisine)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997; ");
            SqlCommand cmd = new SqlCommand(String.Format("select * from Restaurants where Cuisine = '{0}'", cuisine), conn);
            conn.Open();
            SqlDataReader sr = cmd.ExecuteReader();
            List<Restaurants> res = new List<Restaurants>();
            while (sr.Read())
            {
                Restaurants restaurant = new Restaurants((int)sr["Restaurant_Id"], sr["Restaurant_Name"].ToString(), sr["Restaurant_Image"].ToString(), sr["Cuisine"].ToString());
                res.Add(restaurant);
            }
            return res;
        }


        public virtual int DeleteItemByIdFromCart(int id)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997;");
            SqlCommand cmd = new SqlCommand(String.Format("delete from AddItemToCart where ItemNo = '{0}'", id), conn);
            conn.Open();
            int res = cmd.ExecuteNonQuery();
            conn.Close();

            return res;
        }


        public virtual List<Cart> GetCartByUserName(string UserName)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997;");
            SqlCommand cmd = new SqlCommand(String.Format("select A.FoodItem, A.Quantity,A.ItemNo, F.Food_Image,F.Price,F.Id,F.Restaurant_Id from AddItemToCart A inner join Food F on F.Id = A.FoodId where A.UserName = '{0}'", UserName), conn);
            conn.Open();
            SqlDataReader sr = cmd.ExecuteReader();

            List<Cart> cart = new List<Cart>();

            while (sr.Read())
            {
                Cart cartItem = new Cart(UserName, sr["FoodItem"].ToString(), (int)sr["Quantity"], sr["Food_Image"].ToString(), (int)sr["Price"], (int)sr["Id"], (int)sr["Restaurant_Id"], (int)sr["ItemNo"]);
                cart.Add(cartItem);
            }

            return cart;

        }

        public virtual int DeleteCartItemsByUserName(string UserName)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997;");
            SqlCommand cmd = new SqlCommand(String.Format("delete from AddItemToCart where UserName = '{0}'", UserName), conn);
            conn.Open();
            int res = cmd.ExecuteNonQuery();
            conn.Close();
            return res;
        }

        public virtual int SignUp(SignUp signup)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997;");
            SqlCommand cmd = new SqlCommand(String.Format("insert into Users values('{0}','{1}','{2}')", signup.UserName, signup.Email, signup.Password), conn);
            conn.Open();
            int res = cmd.ExecuteNonQuery();
            conn.Close();

            return res;
        }

        public virtual List<OrderDetails> PendingOrders(int Id, string UserName)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997;");

            var OrderList = new List<Order>();
            var orderlist = new List<OrderDetails>();

            SqlCommand cmd = new SqlCommand(String.Format("Select * from PlacedOrderDetail where UserName = '{0}' order by OrderTime desc", UserName), conn);
            conn.Open();
            SqlDataReader sr = cmd.ExecuteReader();

            while (sr.Read())
            {
                string time = sr["OrderTime"].ToString();
                DateTime orderTime = Convert.ToDateTime(time);
                OrderDetails orderDetails = new OrderDetails((int)sr["InVoiceNo"], sr["UserName"].ToString(), (int)sr["RestaurantId"], sr["FoodItem"].ToString(), (int)sr["Quantity"], (int)sr["Price"], orderTime, sr["status"].ToString());
                orderlist.Add(orderDetails);
            }
            conn.Close();
            return orderlist;
        }

        public virtual List<OrderDetails> CompletedOrders(int Id, string UserName)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997;");

            var OrderList = new List<Order>();
            var orderlist = new List<OrderDetails>();

            SqlCommand cmd1 = new SqlCommand(String.Format("Select * from CompletedOrder where UserName = '{0}' order by OrderCompletionTime desc", UserName), conn);
            conn.Open();
            SqlDataReader sr1 = cmd1.ExecuteReader();
            while (sr1.Read())
            {
                OrderDetails order = new OrderDetails((int)sr1["InVoiceNo"], sr1["UserName"].ToString(), sr1["FoodItem"].ToString(), (int)sr1["Quantity"], (int)sr1["Price"], (DateTime)sr1["OrderCompletionTime"], sr1["status"].ToString());
                orderlist.Add(order);
            }
            conn.Close();
            return orderlist;
        }

        public virtual int Orders(OrdersApi order)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997;");
            SqlCommand cmd = new SqlCommand(String.Format(
                "insert into Orders values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                order.InVoiceNo,
                order.UserName,
                order.Address,
                order.PhoneNo,
                order.OrderTime, order.City, order.State, order.Zipcode, order.CardNo, order.ExpMonth, order.ExpYear, order.CVV, order.status), conn);
            conn.Open();
            int res = cmd.ExecuteNonQuery();
            conn.Close();
            return res;

        }

        public virtual List<Menu> Search(string FoodItem)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997;");
            SqlCommand cmd = new SqlCommand(String.Format("Select * from Food where Food_Item like '%{0}%' ", FoodItem), conn);
            conn.Open();
            SqlDataReader sr = cmd.ExecuteReader();

            List<Menu> list = new List<Menu>();


            while (sr.Read())
            {
                int id = (int)sr["Id"];
                Menu menu = new Menu(
                    4,
                    sr["Food_Image"].ToString(),
                    sr["Food_Item"].ToString(),
                    (int)sr["Price"],
                    (int)sr["Restaurant_Id"]);
                Console.WriteLine(id);
                menu.Id = id;
                list.Add(menu);
            }
            return list;
        }


        public virtual int OrderDetails(List<OrderDetailsApi> order)
        {
            SqlConnection conn = new SqlConnection("Data Source = fooddeliverydatabase.ctzhubalbjxo.ap-south-1.rds.amazonaws.com,1433 ; Initial Catalog = FoodDeliveryApplication ; Integrated Security=False; User ID=admin; Password=surya1997;");
            int res = 0;
            foreach (var obj in order)
            {
                SqlCommand sqlCommand = new SqlCommand(String.Format(
                    "insert into PlacedOrderDetail values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                    obj.InVoiceNo,
                    obj.UserName,
                    obj.RestaurantId,
                    obj.FoodItem,
                    obj.Quantity,
                    obj.Price,
                    obj.OrderTime,
                    obj.Status), conn);

                conn.Open();
                res = res + sqlCommand.ExecuteNonQuery();
                conn.Close();
            }
            return res;
        }



    }
}
