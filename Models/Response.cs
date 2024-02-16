namespace EMedicineBE.Models
{
    public class Response
    {   public int statusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<Users> listUsers { get; set; }
        public Users user { get; set; }
        public List<Medicines> listMedicines { get; set; }
        public Medicines medicines { get; set; }
        public List<Cart> listCarts { get; set; }
        public Cart carts { get; set; }
        public List<Orders> listOrders { get; set; }
        public Orders order { get; set; }
        public List<OrderItems> listOrderItem { get; set; }
        public OrderItems orderItem { get; set; }
    }
}
