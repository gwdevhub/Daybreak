namespace Daybreak.Utils;

public static class MathUtils
{
    public static uint RoundUpToTheNextHighestPowerOf2(uint n)
    {
        n--;
        n |= n >> 1;
        n |= n >> 2;
        n |= n >> 4;
        n |= n >> 8;
        n |= n >> 16;
        n++;
        return n;
    }
}
