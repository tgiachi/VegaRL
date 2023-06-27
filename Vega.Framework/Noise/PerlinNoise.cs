namespace Vega.Framework.Noise;

public class PerlinNoise
{
    private readonly List<byte> p;

    public PerlinNoise(Random rng)
    {
        byte[] bytes = new byte[256];
        rng.NextBytes(bytes);

        p = new List<byte>(256 * 2);
        p.AddRange(bytes);
        p.AddRange(bytes);
    }

    public double Noise(double x, double y, double z)
    {
        // Find the unit cube that contains the point
        int X = (int)Math.Floor(x) & 255;
        int Y = (int)Math.Floor(y) & 255;
        int Z = (int)Math.Floor(z) & 255;

        // Find relative x, y,z of point in cube
        x -= Math.Floor(x);
        y -= Math.Floor(y);
        z -= Math.Floor(z);

        // Compute fade curves for each of x, y, z
        double u = Fade(x);
        double v = Fade(y);
        double w = Fade(z);

        // Hash coordinates of the 8 cube corners
        int A = p[X] + Y;
        int AA = p[A] + Z;
        int AB = p[A + 1] + Z;
        int B = p[X + 1] + Y;
        int BA = p[B] + Z;
        int BB = p[B + 1] + Z;

        // Add blended results from 8 corners of cube
        double res = Lerp(
            w,
            Lerp(
                v,
                Lerp(u, Grad(p[AA], x, y, z), Grad(p[BA], x - 1, y, z)),
                Lerp(u, Grad(p[AB], x, y - 1, z), Grad(p[BB], x - 1, y - 1, z))
            ),
            Lerp(
                v,
                Lerp(u, Grad(p[AA + 1], x, y, z - 1), Grad(p[BA + 1], x - 1, y, z - 1)),
                Lerp(u, Grad(p[AB + 1], x, y - 1, z - 1), Grad(p[BB + 1], x - 1, y - 1, z - 1))
            )
        );
        return (res + 1.0) / 2.0;
    }

    private static double Fade(double t) => t * t * t * (t * (t * 6 - 15) + 10);

    private static double Lerp(double t, double a, double b) => a + t * (b - a);

    private static double Grad(int hash, double x, double y, double z)
    {
        int h = hash & 15;

        // Convert lower 4 bits of hash inot 12 gradient directions
        double u = h < 8 ? x : y,
            v = h < 4 ? y :
                h == 12 || h == 14 ? x : z;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
}
