using System.Linq;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;

namespace Generics2;

static class EnumerableExtensions
{
    public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> that, Func<T,bool> predicate)
    {
        foreach (var item in that)
        {
            if(predicate(item)) 
                yield return item;
        }
    }

    public static IEnumerable<TResult> MySelect<TSource,TResult>(this IEnumerable<TSource> that , Func<TSource,TResult> selector)
    {
        foreach (var item in that)
        {
            yield return selector(item);
        }
    }

    public static int MyCount<T>(this IEnumerable<T> that)
    {
        var collections = that as ICollection<T>;
        if (collections != null)
            return collections.Count;

        return 0;
    }

    public static int MyCount<T>(this IEnumerable<T> that , Func<T,bool> predicate)
    {
        var cnt = 0;
        foreach(var item in that)
        {
            if (predicate(item))
                cnt++;
        }
        return cnt;
    }

    //public static IEnumerable<TResult> MyAny<TSource,TResult>(this IEnumerable<TSource> that, Func<TSource,bool> predicate)
    //{
    //    foreach(var item in that)
    //    {
    //        return item;
    //    }
    //}

    public static T MyFirst<T>(this IEnumerable<T> that)
    {
        using (var enumerable = that.GetEnumerator())
        { 
            if (!enumerable.MoveNext())
                throw new InvalidOperationException();

            return enumerable.Current;
        }
        //foreach (var item in that)
        //   return item;
        
        //throw new InvalidOperationException();
    }

    public static T MyFirst<T>(this IEnumerable<T> that, Func<T, bool> predicate)
    {
        foreach (var item in that)
        {
            if (predicate(item))
                return item;
        }

        throw new InvalidOperationException();
    }

}

class Generics2
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //IEnumerable<int> filteredList = numbers.Where(i => i > 4);    
        IEnumerable<int> filteredList = numbers.MyWhere(i => i > 4);
        //IEnumerable<int> squared = filteredList.Select(i => i * i);
        IEnumerable<int> squared = filteredList.MySelect(i => i * i);

        

        foreach (var i in squared)
        {
            Console.WriteLine(i);
        }

        numbers.Add(10);
        Console.WriteLine("------------------------");
        foreach (var i in squared)
        {
            Console.WriteLine(i);
        }

        int first = squared.MyFirst();
        Console.WriteLine(first);

        int firstFunc = squared.MyFirst(i => i > 50);

        Console.WriteLine(firstFunc);

        Console.Write(squared.MyCount(i => i > 50));

    }

}