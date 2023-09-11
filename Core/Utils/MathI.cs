namespace ZLC.Utils;

/// <summary>
/// 整型的数学方法
/// </summary>
public static class MathI
{
    /// <summary>
    /// <paramref name="f"/>的<paramref name="p"/>次方
    /// </summary>
    /// <param name="f"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static int Pow(int f, int p)
    {
        int result = 1;
        if (p == 0) {
            return 1;
        }
        else if(p < 0)
        {
            f = 1 / f;
            p = -p; 
        }

        while(p > 0)
        {
            if((p & 1) == 1)
                result *= f; 

            f = f * f;
            p = p >> 1;
        }

        return result;
    }
}