using System;
using System.Collections.Generic;

namespace JibbR
{
    public static class Helpers
    {
        public static void DisposeSafely(this IDisposable source)
        {
            if (source != null)
            {
                source.Dispose();
            }
        }

        // http://stackoverflow.com/a/648240/39605
        public static T RandomElement<T>(this IEnumerable<T> source, Random random)
        {
            T current = default(T);

            int count = 0;
            foreach (T element in source)
            {
                count++;
                if (random.Next(count) == 0)
                {
                    current = element;
                }
            }

            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty");
            }

            return current;
        }
    }
}