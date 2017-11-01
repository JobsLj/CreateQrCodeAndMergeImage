using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectToObject
{
    class Program
    {
        static void Main(string[] args)
        {

            var strOne = new string(new char[] { 'a', 'b', 'c' });
            var strTwo = new string(new char[] { 'a', 'b', 'c' });
            Console.WriteLine(strTwo == strOne);

            Console.WriteLine(strTwo.Equals(strOne));

            Console.WriteLine(object.ReferenceEquals(strTwo,strOne));

            Console.ReadLine();

        }
    }
}