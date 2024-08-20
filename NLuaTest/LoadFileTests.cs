using System;
using System.IO;
using NLua;

using NLuaTest.Mock;

#if WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SetUp = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestInitializeAttribute;
using TearDown = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestCleanupAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

#if MONOTOUCH
using Foundation;
using MonoTouch;
#endif

namespace LoadFileTests
{
	[TestFixture]
	#if MONOTOUCH
	[Preserve (AllMembers = true)]
	#endif
	public class LoadLuaFile
	{

        [OneTimeSetUp]
        public void Init()
        {
			Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
        }

		/*
        * Tests capturing an exception
        */
		[Test]
		public void TestLoadFile ()
        {
			using (Lua lua = new Lua ()) {
				lua.LoadCLRPackage ();

				lua.DoFile ("scripts/test.lua");

				int width = (int)(double)lua ["width"];
				int height = (int)(double)lua ["height"];
				string message = (string)lua ["message"];
				int color_g	= (int)(double)lua ["color.g"];
				LuaFunction func = (LuaFunction)lua ["func"];
				object[] res = func.Call (12, 34);
				int x = (int)(double)res [0];
				int y = (int)(double)res [1];
                Assert.Multiple(() =>
                {
                    //function func(x,y)
                    //	return x,x+y
                    //end

                    Assert.That(width, Is.EqualTo(100));
                    Assert.That(height, Is.EqualTo(200));
                    Assert.That(message, Is.EqualTo("Hello World!"));
                    Assert.That(color_g, Is.EqualTo(20));
                    Assert.That(x, Is.EqualTo(12));
                    Assert.That(y, Is.EqualTo(46));
                });
            }
		}


		[Test]
		public void TestBinaryLoadFile ()
		{
			using (Lua lua = new Lua ()) {
				lua.LoadCLRPackage ();
				if (IntPtr.Size == 4)
					lua.DoFile ("scripts/test_32.luac");
				else
					lua.DoFile ("scripts/test_64.luac");

				int width = (int)(double)lua ["width"];
				int height = (int)(double)lua ["height"];
				string message = (string)lua ["message"];
				int color_g	= (int)(double)lua ["color.g"];
				LuaFunction func = (LuaFunction)lua ["func"];
				object[] res = func.Call (12, 34);
				int x = (int)(double)res [0];
				int y = (int)(double)res [1];
                Assert.Multiple(() =>
                {
                    //function func(x,y)
                    //	return x,x+y
                    //end

                    Assert.That(width, Is.EqualTo(100));
                    Assert.That(height, Is.EqualTo(200));
                    Assert.That(message, Is.EqualTo("Hello World!"));
                    Assert.That(color_g, Is.EqualTo(20));
                    Assert.That(x, Is.EqualTo(12));
                    Assert.That(y, Is.EqualTo(46));
                });
            }
		}
	}
}

