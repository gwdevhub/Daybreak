using Daybreak.Services.Drawing.Modules;

namespace Daybreak.Services.Drawing;
public interface IDrawingModuleProducer
{
    void RegisterDrawingModule<T>()
        where T : DrawingModuleBase;
}
