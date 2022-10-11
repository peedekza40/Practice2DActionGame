using Core.Constants;

namespace Infrastructure.InputSystem
{
    public interface IUIPersistence
    {
        UINumber Number { get; }
        bool IsOpen { get; }
        MouseEvent MouseEvent { get; }
    }
}

