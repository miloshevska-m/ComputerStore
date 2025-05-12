using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.services.DTOs
{
    public class BasketDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class ShoppingbasketDTO
    {
        public List<BasketDTO> Items { get; set; } = new List<BasketDTO>();
    }
}
