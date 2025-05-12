using ComputerStore.services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.services.DTOs;

namespace ComputerStore.services.Interfaces
{
    public interface Idiscountservice
    {
        Task<decimal> CalculateDiscountAsync(ShoppingbasketDTO cart);
    }
}
