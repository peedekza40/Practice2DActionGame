using Core.Constants;

namespace Character.Inventory
{
    public class EquipmentSlot : ItemSlot
    {
        public EquipmentType Type;
        public EquipmentModel Equipment { get; private set; }
    }
}