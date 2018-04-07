using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestMain.Engine.TestBase;

namespace TestMain.Engine
{

    public static class TestLoader
    {

        private static readonly object[] EmptyArgs = new object[0];

        public static void Run()
        {
            var beforeClasses = new List<MethodInfo>();
            var befores = new List<MethodInfo>();
            var runs = new List<MethodInfo>();
            var afters = new List<MethodInfo>();

            var totalCases = 0;
            var passCases = 0;

            foreach (var type in 
                Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.GetCustomAttribute<TestClass>() != null)
                .OrderBy(type => type.Name))
            {
                Console.WriteLine($"TestClass {type}");
                foreach (var method in type.GetMethods())
                {
                    if (method.GetCustomAttribute<BeforeClass>() != null)
                    {
                        if (!method.IsStatic)
                        {
                            Console.WriteLine($"Warning: method {method} in type {type} with attribute BeforeClass is not static.");
                            continue;
                        }
                        beforeClasses.Add(method);
                    }
                    if (method.GetCustomAttribute<Before>() != null)
                    {
                        if (!method.IsStatic)
                        {
                            Console.WriteLine($"Warning: method {method} in type {type} with attribute Before is not static.");
                            continue;
                        }
                        befores.Add(method);
                    }
                    if (method.GetCustomAttribute<After>() != null)
                    {
                        if (!method.IsStatic)
                        {
                            Console.WriteLine($"Warning: method {method} in type {type} with attribute After is not static.");
                            continue;
                        }
                        afters.Add(method);
                    }
                    if (method.GetCustomAttribute<Test>() != null)
                    {
                        if (!method.IsStatic)
                        {
                            Console.WriteLine($"Warning: method {method} in type {type} with attribute Test is not static.");
                            continue;
                        }
                        runs.Add(method);
                    }
                }

                foreach (var method in beforeClasses)
                {
                    method.Invoke(null, EmptyArgs);
                }

                foreach (var run in runs.OrderBy(method => method.Name))
                {
                    Console.Write($"\tCase {run.Name}\t");
                    totalCases++;
                    foreach (var before in befores)
                    {
                        before.Invoke(null, EmptyArgs);
                    }
                    try
                    {
                        run.Invoke(null, EmptyArgs);
                        Console.WriteLine("pass");
                        passCases++;
                    }
                    catch (TargetInvocationException e)
                    {
                        Console.WriteLine("fail");
                        Console.WriteLine(e.InnerException);
                    }
                    foreach (var after in afters)
                    {
                        after.Invoke(null, EmptyArgs);
                    }
                }

                beforeClasses.Clear();
                befores.Clear();
                runs.Clear();
                afters.Clear();
            }

            if (totalCases == passCases)
            {
                Console.WriteLine("All pass!");
            }

        }
    }   
}
