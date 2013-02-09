using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class OitLogoSvg
{
    public static void Main(string[] args)
    {
        var width = 800.0;
        var height = 800.0;
        var ox = width / 2;
        var oy = height / 2;
        var unit = 100.0;
        var weight = 50.0;
        var colors = new string[] { "#FF0000", "#0000FF", "#FFFF00", "#00FF00" };

        using (var writer = new StreamWriter("oitlogo.svg", false, Encoding.UTF8))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>");
            writer.WriteLine("<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">");
            writer.WriteLine("<svg width=\"" + width + "px\" height=\"" + height + "px\">");
            for (var i = 0; i < 4; i++)
            {
                var left = CreateArc(ox + (i + 1) * unit, oy, 4 * unit, Math.PI - Math.Acos(0.75), Math.PI + Math.Acos(0.75), weight);
                var right = CreateArc(ox + (i - 4) * unit, oy, 4 * unit, -Math.Acos(0.75), Math.Acos(0.75), weight);
                WritePolygon(writer, left, colors[i]);
                WritePolygon(writer, right, colors[i]);
            }
            writer.WriteLine("</svg>");
        }
    }

    private static void WritePolygon(StreamWriter writer, Tuple<double, double>[] points, string color)
    {
        writer.Write("<polygon fill=\"" + color + "\" points=\"");
        for (var i = 0; i < points.Length; i++)
        {
            writer.Write(points[i].Item1 + "," + points[i].Item2);
            if (i < points.Length - 1) writer.WriteLine();
            else writer.WriteLine("\" />");
        }
    }

    private static int N1 = 50;
    private static int N2 = 25;

    private static Tuple<double, double>[] CreateArc(double ox, double oy, double radius, double angle1, double angle2, double weight)
    {
        var points = new List<Tuple<double, double>>();
        for (var i = 0; i <= N1; i++)
        {
            double theta = angle1 + (angle2 - angle1) * i / N1;
            double x = (radius + weight / 2) * Math.Cos(theta) + ox;
            double y = (radius + weight / 2) * -Math.Sin(theta) + oy;
            points.Add(Tuple.Create<double, double>(x, y));
        }
        for (var i = 1; i < N2; i++)
        {
            double px = radius * Math.Cos(angle2) + ox;
            double py = radius * -Math.Sin(angle2) + oy;
            double theta = angle2 + Math.PI * i / N2;
            double x = weight / 2 * Math.Cos(theta) + px;
            double y = weight / 2 * -Math.Sin(theta) + py;
            points.Add(Tuple.Create<double, double>(x, y));
        }
        for (var i = 0; i <= N1; i++)
        {
            double theta = angle2 + (angle1 - angle2) * i / N1;
            double x = (radius - weight / 2) * Math.Cos(theta) + ox;
            double y = (radius - weight / 2) * -Math.Sin(theta) + oy;
            points.Add(Tuple.Create<double, double>(x, y));
        }
        for (var i = 1; i < N2; i++)
        {
            double px = radius * Math.Cos(angle1) + ox;
            double py = radius * -Math.Sin(angle1) + oy;
            double theta = angle1 + Math.PI + Math.PI * i / N2;
            double x = weight / 2 * Math.Cos(theta) + px;
            double y = weight / 2 * -Math.Sin(theta) + py;
            points.Add(Tuple.Create<double, double>(x, y));
        }
        return points.ToArray();
    }
}
