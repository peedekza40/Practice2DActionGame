using System.Collections.Generic;
using System.Linq;
using Core.Constants;
using Infrastructure.InputSystem;

namespace Infrastructure.Extensions
{
    public static class IUIPersistenceExtension
    {
        public static IUIPersistence GetByUiNumber(this List<IUIPersistence> uiPersistences, UINumber number)
        {
            return uiPersistences.FirstOrDefault(x => x.Number == number);
        }
    }
}
