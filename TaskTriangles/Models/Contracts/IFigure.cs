﻿namespace TaskTriangles.Models.Contracts
{
    public interface IFigure
    {
        Point[] Points { get; set; }
        int? DepthLevel { get; set; }
    }
}
