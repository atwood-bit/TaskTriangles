using TaskTriangles.Models;

namespace TaskTriangles.Services.Interfaces
{
    public interface IFigureService
    {
        Task<ResultModel> BuildTree(string filePath);
    }
}
