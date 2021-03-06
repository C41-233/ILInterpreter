﻿using System;

namespace TestMain.Engine.TestBase
{

    [AttributeUsage(AttributeTargets.Class)]
    public class TestClass : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Test : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Before : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class After : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeClass : Attribute
    {
    }

}
