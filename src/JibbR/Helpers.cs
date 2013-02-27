using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JibbR
{
    public static class Helpers
    {
        public static readonly string BotName = "JibbR";

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

        public static List<TResult> SelectList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select(selector).ToList();
        }

        public static string ValueFor(this Match match, string group)
        {
            return match.Groups[group].Value;
        }
    }
}